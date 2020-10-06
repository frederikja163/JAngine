using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public abstract class Buffer<T> : IDisposable
        where T : unmanaged
    {
        protected readonly int _handle;
        protected readonly T[] _data;
        protected abstract BufferTarget Target { get; }

        public unsafe Buffer(T[] data)
        {
            _handle = GL.GenBuffer();
            _data = data;
            Bind();
            GL.BufferData(Target, sizeof(T) * data.Length, data, BufferUsageHint.StaticDraw);
            Unbind();
        }

        public void Bind()
        {
            GL.BindBuffer(Target, _handle);
        }

        public void Unbind()
        {
            GL.BindBuffer(Target, 0);
        }

        public int Count => _data.Length;

        public T this[int i] => _data[i];
        
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}