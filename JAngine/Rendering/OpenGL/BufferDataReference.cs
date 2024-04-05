namespace JAngine.Rendering.OpenGL;

public sealed class BufferDataReference<T>
    where T : unmanaged
{
    private readonly IBuffer<T> _buffer;

    public BufferDataReference(IBuffer<T> buffer, int index)
    {
        _buffer = buffer;
        Index = index;
    }
    
    public int Index { get; }
    public T Data { get => _buffer[Index]; set => _buffer.SetSubData(Index, value); }
}
