using JAngine.ECS;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable
{
    private static readonly HashSet<Window> s_windows = new();

    private readonly Glfw.Window _handle;
    private readonly List<(IGlObject, IGlEvent)> _updateQueue = new();
    private readonly Dictionary<(IGlObject, Type), int> _uniqueUpdates = new();
    private readonly List<World> _worlds = new List<World>();
    private HashSet<VertexArray> _vaos = new HashSet<VertexArray>();
    private readonly Thread _renderingThread;

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
    /// TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, Glfw.Window.Null);
        _renderingThread = new Thread(RenderThread);
        _renderingThread.Start();
        s_windows.Add(this);
    }

    /// <summary>
    /// Holds the thread and runs while windows are still open.
    /// Checks for events for each of the windows.
    /// </summary>
    public static void Run()
    {
        while (s_windows.Any())
        {
            Glfw.PollEvents();
        }
    }
    
    private void RenderThread()
    {
        Glfw.MakeContextCurrent(_handle);

        Gl.ClearColor(1, 0, 1, 1);
        while (IsOpen)
        {
            Gl.Clear(Gl.ClearBufferMask.ColorBuffer);
            
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
                vao.Shader.Bind();
                Gl.DrawElementsInstanced(Gl.PrimitiveType.Triangles, vao.PointCount, Gl.DrawElementsType.UnsignedInt, 0, 1);
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
}
