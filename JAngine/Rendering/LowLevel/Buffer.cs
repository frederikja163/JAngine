using System;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public abstract class Buffer<T> : GlObject
        where T : unmanaged
    {
        public int Size { get; }

        private Buffer(Window window) : base(window, GL.CreateBuffer) { }

        protected Buffer(Window window, int size) : this(window)
        {
            Size = size;
            Window.Queue(() =>
            {
                GL.NamedBufferStorage(Handle, size, IntPtr.Zero, BufferStorageMask.MapWriteBit);
            });
        }

        protected Buffer(Window window, T[] data) : this(window)
        {
            Size = data.Length;
            Window.Queue(() =>
            {
                GL.NamedBufferStorage(Handle, data, BufferStorageMask.MapWriteBit);
            });
        }
        
        public override void Dispose()
        {
            GL.DeleteBuffer(Handle);
        }
    }

    public class VertexBuffer<T> : Buffer<T>
        where T : unmanaged
    {
        public VertexBuffer(Window window, int size) : base(window, size)
        {
        }

        public VertexBuffer(Window window, params T[] data) : base(window, data)
        {
        }
    }

    public class ElementBuffer : Buffer<uint>
    {
        public ElementBuffer(Window window, int size) : base(window, size)
        {
        }

        public ElementBuffer(Window window, params uint[] data) : base(window, data)
        {
        }
    }
}