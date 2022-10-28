using System.Collections;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.OpenGL;

public abstract unsafe class Buffer : IDisposable
{
    internal BufferHandle Handle { get; private init; }

    public Buffer()
    {
        Handle = GL.CreateBuffer();
    }

    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}

public sealed unsafe class Buffer<T> : Buffer, IReadOnlyCollection<T>
    where T : unmanaged
{
    public int Count => _data.Length;

    private readonly T[] _data;

    public Buffer(params T[] data) : base()
    {
        _data = data;
        GL.NamedBufferStorage(Handle, _data, BufferStorageMask.DynamicStorageBit);
    }

    public Buffer(int capacity)
    {
        _data = new T[capacity];
        GL.NamedBufferStorage(Handle, sizeof(T) * capacity, IntPtr.Zero, BufferStorageMask.DynamicStorageBit);
    }

    public void SetRange(int offset, params T[] newData)
    {
        GL.NamedBufferSubData(Handle, (IntPtr)(sizeof(T) * offset), sizeof(T) * newData.Length, newData);
    }

    public T this[int index]
    {
        get => _data[index];
    }

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _data)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}