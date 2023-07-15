namespace JAngine.Platform;

internal interface IPlatformBackend
{
    public IWindowBackend CreateWindow(WindowInfo windowInfo);
}