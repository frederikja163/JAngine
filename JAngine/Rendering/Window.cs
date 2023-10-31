using JAngine;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable
{
    private static readonly HashSet<Window> s_windows = new();

    private readonly List<(IGlObject, IGlEvent)> _updateableObjects = new List<(IGlObject, IGlEvent)>();
    private readonly Glfw.Window _handle;
    private HashSet<IRenderable> _renderables = new HashSet<IRenderable>();
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
            lock (_updateableObjects)
            {
                objects = _updateableObjects.ToList();
                _updateableObjects.Clear();
            }
            foreach ((IGlObject glObject, IGlEvent glEvent) in objects)
            {
                glObject.DispatchEvent(glEvent);
            }

            foreach (IRenderable renderable in _renderables)
            {
                renderable.Render();
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
        lock (_updateableObjects)
        {
            _updateableObjects.Add((glObject, glEvent));
        }
    }

    public IRenderable AttachRender(VertexArray vao, Shader shader)
    {
        IRenderable renderable = new Renderable(vao, shader);
        _renderables.Add(renderable);
        return renderable;
    }

    public void DetachRender(IRenderable renderable)
    {
        _renderables.Remove(renderable);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        s_windows.Remove(this);
    }
}
