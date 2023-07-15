using System.Runtime.Versioning;

namespace JAngine.Platform.X11Backend;

[SupportedOSPlatform("linux")]
internal sealed class X11Platform : IPlatformBackend, IDisposable
{
    private readonly DisplayHandle _displayHandle;
    public X11Platform()
    {
        _displayHandle = X11.OpenDisplay();
    }

    public IWindowBackend CreateWindow(WindowInfo windowInfo)
    {
        return new X11Window(_displayHandle, windowInfo);
    }

    public void Dispose()
    {
        X11.CloseDisplay(_displayHandle);
    }
}