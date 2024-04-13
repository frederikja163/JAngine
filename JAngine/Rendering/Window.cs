using System.Diagnostics;
using System.Numerics;
using JAngine.Rendering.Gui;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public delegate void WindowResizeDelegate(Window window, int width, int height);
public delegate void MouseMovedDelegate(Window window, Vector2 mouseDelta);

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable, IGuiElement
{
    private static readonly HashSet<Window> s_windows = new();

    private readonly Glfw.Window _handle;
    private readonly List<(IGlObject, IGlEvent)> _updateQueue = new();
    private readonly Dictionary<(IGlObject, Type), int> _uniqueUpdates = new();
    private readonly Dictionary<Key, List<KeyBinding>> _keyBindings = new();
    private HashSet<VertexArray> _vaos = new HashSet<VertexArray>();
    private readonly HashSet<Key> _keysDown = new HashSet<Key>();
    private readonly Thread _renderingThread;
    private WindowResizeDelegate? _onWindowResize;
    private MouseMovedDelegate? _onMouseMoved;
    private int _viewportWidth;
    private int _viewportHeight;
    private Matrix4x4 _guiMatrix;
    private event Action? _positionChanged;


    private readonly Glfw.WindowSizeCallback _sizeCallback;
    private readonly Glfw.KeyCallback _keyCallback;
    private readonly Glfw.MouseCallback _mouseCallback;
    private readonly Glfw.CursorPositionCallback _cursorPositionCallback;
    
    static Window()
    {
        Glfw.GetVersion(out int major, out int minor, out int rev);
        Log.Info($"Glfw version {major}.{minor}.{rev}");
        if (!Glfw.Init())
        {
            throw new Exception("Failed to initialize Glfw");
        }
        
        Glfw.WindowHint(Glfw.Hint.ContextVersionMajor, 4);
        Glfw.WindowHint(Glfw.Hint.ContextVersionMinor, 6);
        Glfw.WindowHint(Glfw.Hint.OpenglProfile, Glfw.OpenGL.CoreProfile);
    }
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <param name="title">The title of the new window.</param>
    // TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, Glfw.Window.Null);
        _keyCallback = KeyCallback;
        Glfw.SetKeyCallback(_handle, _keyCallback);
        _mouseCallback = MouseCallback;
        Glfw.SetMouseCallback(_handle, _mouseCallback);
        _cursorPositionCallback = CursorPositionCallback;
        Glfw.SetCursorPosCallback(_handle, _cursorPositionCallback);
        Glfw.GetCursorPos(_handle, out double x, out double y);
        CursorPosition = new Vector2((float)x, (float)y);
        _sizeCallback = SizeCallback;
        Glfw.SetWindowSizeCallback(_handle, _sizeCallback);
        _renderingThread = new Thread(RenderThread);
        _renderingThread.IsBackground = true;
        _renderingThread.Start();
        s_windows.Add(this);
    }
    public int Width { get; private set; }
    public int Height { get; private set; }

    private void SizeCallback(Glfw.Window window, int width, int height)
    {
        Width = width;
        Height = height;
        _guiMatrix = Matrix4x4.CreateScale(2f/width, 2f/height, 1f) * Matrix4x4.CreateTranslation(-1f, -1f, 0f);
        _positionChanged?.Invoke();
    }

    public event WindowResizeDelegate? OnWindowResize
    {
        add => _onWindowResize += value;
        remove => _onWindowResize -= value;
    }

    public event MouseMovedDelegate? OnMouseMoved
    {
        add => _onMouseMoved += value;
        remove => _onMouseMoved -= value;
    }

    /// <summary>
    /// Holds the thread and runs while windows are still open.
    /// Checks for events for each of the windows.
    /// </summary>
    public static void Run()
    {
        while (s_windows.Any())
        {
            Glfw.WaitEventsTimeout(0.001);
            foreach (Window window in s_windows)
            {
                window.SendHeldKeys();
            }
        }
    }

    private void CursorPositionCallback(Glfw.Window window, double xpos, double ypos)
    {
        Vector2 position = new Vector2((float)xpos, (float)ypos);
        Vector2 delta = CursorPosition - position;
        CursorPosition = position;
        _onMouseMoved?.Invoke(this, delta);
    }

    public Vector2 CursorPosition
    {
        get;
        private set;
    }

    private void MouseCallback(Glfw.Window window, Glfw.MouseButton button, Glfw.Action action, Glfw.Mods mods)
    {
        if (action == Glfw.Action.Repeat)
        {
            return;
        }
        
        Key key = GetMouseButtonFromGlfw(button) | GetKeyMod();
        if (action == Glfw.Action.Release && _keysDown.Remove(key))
        {
            key |= Key.Release;
        }
        else if (action == Glfw.Action.Press && _keysDown.Add(key))
        {
            key |= Key.Press;
        }
        else
        {
            throw new UnreachableException();
        }
        
        SimulateKeyPress(key);
    }

    private void KeyCallback(Glfw.Window window, Glfw.Key gKey, int scancode, Glfw.Action action, Glfw.Mods mods)
    {
        if (action == Glfw.Action.Repeat)
        {
            return;
        }

        Key key = GetKeyFromGlfw(gKey) | GetKeyMod();
        if (action == Glfw.Action.Release && _keysDown.Remove(key))
        {
            key |= Key.Release;
        }
        else if (action == Glfw.Action.Press && _keysDown.Add(key))
        {
            key |= Key.Press;
        }
        else
        {
            throw new UnreachableException();
        }
        
        SimulateKeyPress(key);
    }

    private void SendHeldKeys()
    {
        Key mods = GetKeyMod();
        foreach (Key k in _keysDown)
        {
            Key key = mods | k | Key.Held;

            SimulateKeyPress(key);
        }
    }

    public void SimulateKeyPress(Key key)
    {
        if (_keyBindings.TryGetValue(key, out List<KeyBinding>? bindings))
        {
            foreach (KeyBinding binding in bindings.Where(b => b.Enabled).ToList())
            {
                binding.SimulatePress();
            }
        }
    }

    private Key GetKeyMod()
    {
        Key mods = 0;
        foreach (Key key in _keysDown)
        {
            mods |= key;
        }
        mods &= (Key)0x0f_ff_00_00;
        return mods;
    }
    
    private static Key GetMouseButtonFromGlfw(Glfw.MouseButton key)
    {
        return (Key)((int)key << 12);
    }

    private static Key GetKeyFromGlfw(Glfw.Key key)
    {
        return key switch
        {
            Glfw.Key.LeftShift => Key.LShift,
            Glfw.Key.RightShift => Key.RShift,
            Glfw.Key.LeftControl => Key.LControl,
            Glfw.Key.RightControl => Key.RControl,
            Glfw.Key.LeftAlt => Key.LAlt,
            Glfw.Key.RightAlt => Key.RAlt,
            Glfw.Key.LeftSuper => Key.LSuper,
            Glfw.Key.RightSuper => Key.RSuper,
            _ => (Key)key,
        };
    }

    public KeyBinding AddKeyBinding(Key key, Action onActivate)
    {
        KeyBinding binding = new KeyBinding(key, onActivate);
        AddKeyBinding(binding);
        return binding;
    }
    
    public void AddKeyBinding(KeyBinding binding)
    {
        if (!_keyBindings.TryGetValue(binding.Key, out List<KeyBinding>? bindings))
        {
            bindings = new List<KeyBinding>();
            _keyBindings.Add(binding.Key, bindings);
        }
        bindings.Add(binding);
    }
    
    private void RenderThread()
    {
        Glfw.MakeContextCurrent(_handle);

        Gl.Enable(Gl.EnableCap.Blend);
        Gl.ClearColor(1, 0, 1, 1);
        while (IsOpen)
        {
            if (_viewportWidth != Width || _viewportHeight != Height)
            {
                _viewportWidth = Width;
                _viewportHeight = Height;
                Gl.Viewport(0, 0, _viewportWidth, _viewportHeight);
            }
            
            Gl.Clear(Gl.ClearBufferMask.ColorBuffer | Gl.ClearBufferMask.DepthBuffer);
            
            List<(IGlObject, IGlEvent)> objects;
            lock (_updateQueue)
            {
                objects = _updateQueue.ToList();
                _updateQueue.Clear();
                _uniqueUpdates.Clear();
            }
            foreach ((IGlObject glObject, IGlEvent glEvent) in objects)
            {
                glObject.DispatchEvent(glEvent);
            }

            foreach (VertexArray vao in _vaos)
            {
                vao.Bind();
                Gl.DrawElementsInstanced(Gl.PrimitiveType.Triangles, vao.PointCount, Gl.DrawElementsType.UnsignedInt, 0, vao.InstanceCount);
            }
            
            Glfw.SwapBuffers(_handle);
        }
    }

    /// <summary>
    /// Gets or sets whether this window is open.
    /// False indicates the window should close as soon as possible or is already closed.
    /// True indicates the window will open as soon as possible or is already opened.
    /// </summary>
    public bool IsOpen
    {
        get => !Glfw.WindowShouldClose(_handle);
        set => Glfw.WindowSetShouldClose(_handle, !value);
    }

    internal void QueueUpdate(IGlObject glObject, IGlEvent glEvent)
    {
        lock (_updateQueue)
        {
            _updateQueue.Add((glObject, glEvent));
        }
    }

    internal void QueueUpdateUnique(IGlObject glObject, IGlEvent glEvent)
    {
        lock (_updateQueue)
        {
            if (_uniqueUpdates.ContainsKey((glObject, glEvent.GetType())))
            {
                return;
            }

            _uniqueUpdates.Add((glObject, glEvent.GetType()), _updateQueue.Count);
            _updateQueue.Add((glObject, glEvent));
        }
    }

    internal bool ReplaceUpdateUnique(IGlObject glObject, IGlEvent glEvent)
    {
        lock (_updateQueue)
        {
            if (!_uniqueUpdates.TryGetValue((glObject, glEvent.GetType()), out int index))
            {
                return false;
            }
            _updateQueue[index] = (glObject, glEvent);
        }
        return true;
    }

    internal void AttachVao(VertexArray vao)
    {
        _vaos.Add(vao);
    }

    internal void DetachVao(VertexArray vao)
    {
        _vaos.Remove(vao);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        s_windows.Remove(this);
    }

    IGuiElement IGuiElement.Parent => this;
    Matrix4x4 IGuiElement.TransformMatrix => _guiMatrix;

    float IGuiElement.Width => Width;
    float IGuiElement.Height => Height;
    float IGuiElement.X => 0;
    float IGuiElement.Y => 0;
    float IGuiElement.Layer => -1f;
    
    event Action? IGuiElement.PositionChanged
    {
        add => _positionChanged += value;
        remove => _positionChanged -= value;
    }
}
