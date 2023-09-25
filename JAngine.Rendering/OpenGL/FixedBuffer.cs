using System.Collections;
using System.Drawing;

namespace JAngine.Rendering.OpenGL;

internal sealed class FixedBuffer<T> : IBuffer<T>
    where T : unmanaged
{
    private readonly Gl.BufferStorageMask _mask;
    private readonly T[] _data;
    private readonly List<int> _indicesToUpdate = new List<int>();

    internal FixedBuffer(Window window, Gl.BufferStorageMask mask, params T[] data)
    {
        Window = window;
        _mask = mask;
        _data = data;
        Count = _data.Length;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }

    internal FixedBuffer(Window window, Gl.BufferStorageMask mask, int count)
    {
        Window = window;
        _mask = mask;
        _data = new T[count];
        Count = 0;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }

    public void EnsureCapacity(int size)
    {
        if (size > Capacity)
        {
            throw new InvalidOperationException("Cannot resize a fixed sized buffer.");
        }
    }

    public T this[Index index]
    {
        get => _data[index];
        set => SetSubData(index.GetOffset(_data.Length), value);
    }

    public Span<T> this[Range range] => _data[range];

    public void SetSubData(int offset, params T[] data)
    {
        lock (_data)
        {
            Array.Copy(data, 0, _data, offset, data.Length);
        }
        EnsureCapacity(offset + data.Length);
        lock (_indicesToUpdate)
        {
            if (Count < offset + data.Length)
            {
                Count = offset + data.Length;
            }
            _indicesToUpdate.AddRange(Enumerable.Range(offset, data.Length));
            if (_indicesToUpdate.Count == data.Length)
            {
                Window.QueueUpdate(this, UpdateDataEvent.Singleton);
            }
        }
    }

    public int FindIndex(T value)
    {
        lock (_data)
        {
            for (var i = 0; i < _data.Length; i++)
            {
                T data = _data[i];
                if (data.Equals(value))
                {
                    return i;
                }
            }
        }

        return -1;
    }

    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;
    private int Capacity => _data.Length;
    int IBuffer<T>.Capacity => _data.Length;

    public int Count { get; private set; }
    public bool IsReadOnly { get; } = false;
    
    unsafe void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateBuffer();
                Gl.NamedBufferStorage<T>(Handle, _data, _mask);
                break;
            case UpdateDataEvent:
                List<int> sortedIndices;
                lock (_indicesToUpdate)
                {
                    if (_indicesToUpdate.Count == 0)
                    {
                        return;
                    }
                    sortedIndices = _indicesToUpdate.Order().ToList();
                    _indicesToUpdate.Clear();
                }
                ReadOnlySpan<T> data = new ReadOnlySpan<T>(_data);
                int length = 1;
                int lastIndex = sortedIndices.First();
                foreach (int index in sortedIndices.Skip(1))
                {
                    if (lastIndex + 1 != index)
                    {
                        data.Slice(lastIndex - length, length);
                        Gl.NamedBufferSubData<T>(Handle, (IntPtr)lastIndex - length, (IntPtr)length * sizeof(T), data.Slice(lastIndex - 1, length));
                        length = 0;
                    }
                    lastIndex = index;
                    length += 1;
                }
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

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T data in _data)
        {
            yield return data;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}