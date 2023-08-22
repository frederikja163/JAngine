using System.Numerics;
using JAngine.Core;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable
{
    private static readonly HashSet<Window> Windows = new();

    private readonly Glfw.Window _handle;
    private readonly Thread _renderingThread;
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <param name="title">The title of the new window.</param>
    /// TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, Renderer.ShareContext);
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
        get => !Glfw.WindowShouldClose(_handle);
        set => Glfw.WindowSetShouldClose(_handle, !value);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        Windows.Remove(this);
    }
}