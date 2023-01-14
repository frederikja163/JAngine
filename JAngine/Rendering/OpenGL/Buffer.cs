using System.Collections;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.OpenGL;

/// <summary>
/// A Buffer on the GPU for storing data.
/// </summary>
/// <typeparam name="T">The type of data to store with this Buffer.</typeparam>
public sealed unsafe class Buffer<T> : ObjectBase<BufferHandle>, IList<T> where T : unmanaged
{
    private T[] _data;

    public void WriteBufferToLog()
    {
        Log.Info($"Array[{Count}]:  {{{string.Join(", ", _data)}}}");

        T[] data = new T[Capacity];
        T* ptr = (T*)GL.MapNamedBuffer(Handle, BufferAccessARB.ReadOnly);
        for (int i = 0; i < Capacity; i++)
        {
            data[i] = ptr[i];
        }
        GL.UnmapNamedBuffer(Handle);
        Log.Info($"Buffer[{Count}]: {{{string.Join(", ", data)}}}");
    }

    /// <summary>
    /// Create a Buffer with starting data.
    /// </summary>
    /// <param name="data">The starting data of the Buffer.</param>
    public Buffer(params T[] data) : base(GL.CreateBuffer)
    {
        _data = data;
        _capacity = data.Length;
        Count = data.Length;
        if (data.Length == 0)
        {
            return;
        }
        
        Game.Instance.QueueCommand(() =>
        {
            GL.NamedBufferData(Handle, sizeof(T) * data.Length, _data[0], VertexBufferObjectUsage.DynamicDraw);
        });
    }

    /// <summary>
    /// Create a Buffer with a specific amount of capacity.
    /// </summary>
    /// <param name="capacity">Starting capacity of the Buffer.</param>
    public Buffer(int capacity) : base(GL.CreateBuffer)
    {
        _capacity = capacity;
        _data = new T[capacity];
        Count = 0;
        
        Game.Instance.QueueCommand(() =>
        {
            GL.NamedBufferData(Handle, sizeof(T) * _data.Length, _data[0], VertexBufferObjectUsage.DynamicDraw);
        });
    }

    public override void Dispose()
    {
        Game.Instance.QueueCommand(() =>
        {
            GL.DeleteBuffer(Handle);
        });
    }

    /// <inheritdoc cref="List{T}.EnsureCapacity"/>
    public void EnsureCapacity(int capacity)
    {
        if (Capacity >= capacity)
        {
            return;
        }

        int curCap = Capacity;
        while (curCap < capacity)
        {
            curCap *= 2;
        }
        
        Capacity = curCap;
    }

    private int _capacity;

    /// <inheritdoc cref="List{T}.Capacity"/>
    public int Capacity
    {
        get => _capacity;
        set
        {
            
            T[] data = _data;
            _data = new T[value];
            Array.Copy(data, _data, _capacity);
            _capacity = value;
            
            Game.Instance.QueueCommand(() =>
            {
                GL.NamedBufferData(Handle, sizeof(T) * _capacity, _data[0], VertexBufferObjectUsage.DynamicDraw);
            });
        }
    }

    public int Count { get; private set; }
    public bool IsReadOnly { get; } = false;

    public T this[int index]
    {
        get => _data[index];
        set
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException();
            }
            
            _data[index] = value;
            Game.Instance.QueueCommand(() =>
            {
                GL.NamedBufferSubData(Handle, (IntPtr)index, sizeof(T), _data[index]);
            });
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _data.Length; i++)
        {
            yield return _data[0];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        Insert(Count, item);
    }

    public void Clear()
    {
        Array.Clear(_data);
        Count = 0;
        Game.Instance.QueueCommand(() =>
        {
            GL.NamedBufferSubData(Handle, IntPtr.Zero, sizeof(T) * Count, _data);
        });
    }

    public bool Contains(T item)
    {
        return _data.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Array.Copy(_data, 0, array, arrayIndex, Count);
    }

    public bool Remove(T item)
    {
        int index = IndexOf(item);
        if (index == -1)
        {
            return false;
        }
        
        RemoveAt(index);
        return true;
    }
    public int IndexOf(T item)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i].Equals(item))
            {
                return i;
            }
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        Count += 1;
        EnsureCapacity(Count);
        Array.Copy(_data, index, _data, index + 1, Count - index - 1);
        _data[index] = item;
        
        Game.Instance.QueueCommand(() =>
        {
            GL.NamedBufferSubData(Handle, (IntPtr)(index * sizeof(T)), (Count - index) * sizeof(T), _data[index]);
        });
    }

    public void RemoveAt(int index)
    {
        Count -= 1;
        Array.Copy(_data, index + 1, _data, index, Count - index);
        _data[Count] = default;
        Game.Instance.QueueCommand(() =>
        {
            GL.NamedBufferSubData(Handle, (IntPtr)(index * sizeof(T)), (Count - index + 1) * sizeof(T), _data[index]);
        });
    }
}