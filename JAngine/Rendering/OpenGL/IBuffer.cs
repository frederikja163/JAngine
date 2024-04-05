using System.Collections;

namespace JAngine.Rendering.OpenGL;

public interface IBuffer: IGlObject, IEnumerable, IDisposable
{
    public Type UnderlyingType { get; }
    public int Capacity { get; }
    public int Count { get; }
    public void EnsureCapacity(int size);
}

public interface IBuffer<T> : IBuffer, IEnumerable<T>
    where T : unmanaged
{
    Type IBuffer.UnderlyingType => typeof(T);
    public T this[Index index] { get; set; }
    public Span<T> this[Range range] { get; }
    public void SetSubData(int offset, T data);
    public void SetSubData(int offset, ReadOnlySpan<T> data);
    public void Clear();
    public int FindIndex(T value);
}

internal sealed class UpdateDataEvent : IGlEvent
{
    internal static UpdateDataEvent Default { get; } = new UpdateDataEvent(false);
    internal static UpdateDataEvent SkipInstance { get; } = new UpdateDataEvent(true);

    private UpdateDataEvent(bool skip)
    {
        Skip = skip;
    }
    
    internal bool Skip { get; }
}
internal sealed class UpdateCapacityEvent : IGlEvent
{
    internal static UpdateCapacityEvent Singleton { get; } = new UpdateCapacityEvent();

    private UpdateCapacityEvent()
    {
        
    }
}
