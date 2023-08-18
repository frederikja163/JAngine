using System.Numerics;
using JAngine.Core;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable
{
    private static readonly Glfw.Window ShareContext;
    private static readonly HashSet<Window> Windows = new();

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
        Glfw.WindowHint(Glfw.Hint.Visible, false);
        ShareContext = Glfw.CreateWindow(0, 0, "__share_context__", Glfw.Monitor.Null, Glfw.Window.Null);
        Glfw.WindowHint(Glfw.Hint.Visible, true);
    }

    private readonly Glfw.Window _handle;
    private readonly Thread _renderingThread;
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <param name="title">The title of the new window.</param>
    /// TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, ShareContext);
        _renderingThread = new Thread(RenderThread);
        _renderingThread.Start();
        Windows.Add(this);
    }

    /// <summary>
    /// Holds the thread and runs while windows are still open.
    /// Checks for events for each of the windows.
    /// </summary>
    public static void Run()
    {
        while (Windows.Any())
        {
            Glfw.PollEvents();
        }
    }

    private void RenderThread()
    {
        Glfw.MakeContextCurrent(_handle);
        Renderer.ClearColor(1, 0, 1, 1);
        while (IsOpen)
        {
            Renderer.Clear();
            
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
        get => Glfw.WindowShouldClose(_handle);
        set => Glfw.WindowSetShouldClose(_handle, value);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        Windows.Remove(this);
    }
}