using JAngine.Platform.X11Backend;

namespace JAngine.Platform;

public sealed class Window : IDisposable
{
    private static IPlatformBackend _platformBackend;

    static Window()
    {
        if (OperatingSystem.IsLinux())
        {
            _platformBackend = new X11Platform();
            return;
        }

        throw new PlatformNotSupportedException("Platform not supported.");
    }
    
    private IWindowBackend _windowBackend;

    public Window(string title, int width, int height)
    {
        WindowInfo info = new WindowInfo(title, width, height);
        _windowBackend = _platformBackend.CreateWindow(info);
    }

    public void Run()
    {
        while (true)
        {
            _windowBackend.HandleEvents();
        }
    }

    public void Dispose()
    {
        _windowBackend.Dispose();
    }
}