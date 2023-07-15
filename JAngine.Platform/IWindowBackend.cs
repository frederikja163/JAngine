namespace JAngine.Platform;

internal interface IWindowBackend : IDisposable
{
    void HandleEvents();
}