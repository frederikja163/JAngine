using System.Collections;

namespace JAngine.Rendering.OpenGL;

public sealed class Buffer<T> : IBuffer<T>
    where T : unmanaged
{
    private readonly Gl.BufferUsage _mask;
    private readonly Window _window;
    private uint _handle;
    private T[] _data;
    private int _firstUpdateIndex = -1;
    private int _lastUpdateIndex = -1;

    internal Buffer(Window window, string name, Gl.BufferUsage mask, params T[] data)
    {
        _window = window;
        Name = name;
        _mask = mask;
        _data = data;
        Count = _data.Length;
        _window.QueueUpdate(this, CreateEvent.Singleton);
        _window.QueueUpdateUnique(this, UpdateCapacityEvent.Singleton);
        _window.QueueUpdateUnique(this, UpdateDataEvent.SkipInstance);
    }
    
    public Buffer(Window window, string name, params T[] data) : this(window, name, Gl.BufferUsage.DynamicDraw, data)
    {
    }

    internal Buffer(Window window, string name, Gl.BufferUsage mask, int count)
    {
        _window = window;
        Name = name;
        _mask = mask;
        _data = new T[count];
        Count = 0;
        _window.QueueUpdate(this, CreateEvent.Singleton);
        _window.QueueUpdateUnique(this, UpdateCapacityEvent.Singleton);
        _window.QueueUpdateUnique(this, UpdateDataEvent.SkipInstance);
    }

    public Buffer(Window window, string name, int count) : this(window, name, Gl.BufferUsage.DynamicDraw, count)
    {
        
    }

    public void EnsureCapacity(int size)
    {
        if (size > Capacity)
        {
            int capacity = Math.Max(Capacity, 1);
            while (size > capacity)
            {
                capacity *= 2;
            }
            _data = new T[capacity];
            if (!_window.ReplaceUpdateUnique(this, UpdateDataEvent.SkipInstance))
            {
                _window.QueueUpdateUnique(this, UpdateDataEvent.SkipInstance);
            }
            _window.QueueUpdateUnique(this, UpdateCapacityEvent.Singleton);
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
        EnsureCapacity(offset + data.Length);
        Array.Copy(data, 0, _data, offset, data.Length);
        int lastIndex = offset + data.Length;
        if (Count < lastIndex)
        {
            Count = lastIndex;
        }

        _firstUpdateIndex = Math.Min(_firstUpdateIndex, offset);
        _lastUpdateIndex = Math.Max(_lastUpdateIndex, lastIndex);
        _window.QueueUpdateUnique(this, UpdateDataEvent.Default);
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

    public string Name { get; }
    Window IGlObject.Window => _window;
    uint IGlObject.Handle => _handle;
    private int Capacity => _data.Length;
    int IBuffer<T>.Capacity => _data.Length;

    public int Count { get; private set; }

    unsafe void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                _handle = Gl.CreateBuffer();
                Gl.ObjectLabel(Gl.ObjectIdentifier.Buffer, _handle, Name);
                break;
            case UpdateCapacityEvent:
                Gl.NamedBufferData<T>(_handle, _data, _mask);
                (_firstUpdateIndex, _lastUpdateIndex) = (_data.Length - 1, 0);
                break;
            case UpdateDataEvent ev:
                if (ev.Skip)
                {
                    return;
                }
                
                Gl.NamedBufferSubData<T>(_handle, (IntPtr)_firstUpdateIndex * sizeof(T),
                    (IntPtr)(_firstUpdateIndex - _lastUpdateIndex) * sizeof(T), ref _data[_firstUpdateIndex]);
                (_firstUpdateIndex, _lastUpdateIndex) = (_data.Length - 1, 0);
                break;
            case DisposeEvent:
                Gl.DeleteBuffer(_handle);
                break;
        }
    }
    
    public void Dispose()
    {
        _window.QueueUpdate(this, DisposeEvent.Singleton);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _data.ToList().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private bool Equals(Buffer<T> other)
    {
        return _window.Equals(other._window) && _handle == other._handle;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Buffer<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_window, _handle);
    }
}
