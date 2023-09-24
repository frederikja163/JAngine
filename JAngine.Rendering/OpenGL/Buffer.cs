namespace JAngine.Rendering.OpenGL;

internal sealed class Buffer<T> : IGlObject, IDisposable
    where T : unmanaged
{
    private readonly Gl.BufferStorageMask _mask;
    private readonly T[] _data;

    internal Buffer(Window window, Gl.BufferStorageMask mask, params T[] data)
    {
        Window = window;
        _mask = mask;
        _data = data;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }
    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;

    public void DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateBuffer();
                Gl.NamedBufferStorage<T>(Handle, _data, _mask);
                break;
            case DisposeEvent:
                Gl.DeleteBuffer(Handle);
                break;
        }
    }

    public void Dispose()
    {
        Window.QueueUpdate(this, DisposeEvent.Singleton);
    }
}