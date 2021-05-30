using System;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public abstract class Buffer<T> : IDisposable
        where T : unmanaged
    {
        internal readonly uint Handle;

        private Buffer()
        {
            Handle = GL.CreateBuffer();
        }

        protected Buffer(int size) : this()
        {
            GL.NamedBufferStorage(Handle, size, IntPtr.Zero, BufferStorageMask.MapWriteBit);
        }

        protected Buffer(T[] data) : this()
        {
            GL.NamedBufferStorage(Handle, data, BufferStorageMask.MapWriteBit);
        }
        
        public void Dispose()
        {
            GL.DeleteBuffer(Handle);
        }
    }

    public class VertexBuffer<T> : Buffer<T>
        where T : unmanaged
    {
        public VertexBuffer(int size) : base(size)
        {
        }

        public VertexBuffer(params T[] data) : base(data)
        {
        }
    }

    public class ElementBuffer : Buffer<uint>
    {
        public ElementBuffer(int size) : base(size)
        {
        }

        public ElementBuffer(params uint[] data) : base(data)
        {
        }
    }
}