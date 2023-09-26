using System.Collections;
using System.Drawing;

namespace JAngine.Rendering.OpenGL;

internal sealed class FixedBuffer<T> : IBuffer<T>
    where T : unmanaged
{
    private readonly Gl.BufferStorageMask _mask;
    private readonly T[] _data;
    private bool _isUpdating = true;
    private int _firstUpdateIndex = -1;
    private int _lastUpdateIndex = -1;

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
        Array.Copy(data, 0, _data, offset, data.Length);
        EnsureCapacity(offset + data.Length);
        int lastIndex = offset + data.Length;
        if (Count < lastIndex)
        {
            Count = lastIndex;
        }

        _firstUpdateIndex = Math.Min(_firstUpdateIndex, offset);
        _lastUpdateIndex = Math.Max(_lastUpdateIndex, lastIndex);
        if (!_isUpdating)
        {
            _isUpdating = true;
            Window.QueueUpdate(this, UpdateDataEvent.Singleton);
        }
    }

    public int FindIndex(T value)
    {
        for (var i = 0; i < _data.Length; i++)
        {
            T data = _data[i];
            if (data.Equals(value))
            {
                return i;
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

    unsafe void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateBuffer();
                Gl.NamedBufferStorage<T>(Handle, _data, _mask);
                (_firstUpdateIndex, _lastUpdateIndex) = (_data.Length, 0);
                _isUpdating = false;
                break;
            case UpdateDataEvent:
                Gl.NamedBufferSubData<T>(Handle, (IntPtr)_firstUpdateIndex * sizeof(T),
                    (IntPtr)(_firstUpdateIndex - _lastUpdateIndex) * sizeof(T), ref _data[_firstUpdateIndex]);
                (_firstUpdateIndex, _lastUpdateIndex) = (_data.Length, 0);
                _isUpdating = false;
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
        return _data.ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}