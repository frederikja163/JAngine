using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class Buffer<T> : IDisposable
        where T : unmanaged
    {
        internal readonly int Handle;
        
        public Buffer(int size) : this(new T[size])
        { }
        
        public Buffer(params T[] data)
        {
            unsafe
            {
                GL.CreateBuffers(1, out Handle);
                GL.NamedBufferStorage(Handle, sizeof(T) * data.Length, data, BufferStorageFlags.None);
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