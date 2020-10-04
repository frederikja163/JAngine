using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine
{
    public abstract class BufferObject<T> : IDisposable
        where T : unmanaged
    {
        protected readonly int _handle;
        protected abstract BufferTarget Target { get; }

        public unsafe BufferObject(T[] data)
        {
            _handle = GL.GenBuffer();
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
        
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}