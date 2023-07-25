using JAngine.Core;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable, IEquatable<Window>
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
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <param name="title">The title of the new window.</param>
    /// TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, ShareContext);
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

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        Windows.Remove(this);
    }

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals(Window? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _handle.Equals(other._handle);
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Window other && Equals(other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return _handle.GetHashCode();
    }
}