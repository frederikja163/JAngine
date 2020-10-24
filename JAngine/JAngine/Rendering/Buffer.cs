using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class Buffer<T> : IDisposable
        where T : unmanaged
    {
        internal readonly int Handle;

        public Buffer(int size, BufferStorageFlags flags = BufferStorageFlags.None) : this(new T[size],
            flags)
        { }
        
        public Buffer(T[] data, BufferStorageFlags flags = BufferStorageFlags.None)
        {
            unsafe
            {
                GL.CreateBuffers(1, out Handle);
                GL.NamedBufferStorage(Handle, sizeof(T) * data.Length, data, flags);
            }
        }

        public void SetData(params T[] data) => SetData(0, data);
        
        public void SetData(int offset, params T[] data)
        {
            unsafe
            {
                GL.NamedBufferSubData(Handle, (IntPtr)offset, sizeof(T) * data.Length, data);
            }
        }
        
        public void Dispose()
        {
            GL.DeleteBuffer(Handle);
        }
    }
}