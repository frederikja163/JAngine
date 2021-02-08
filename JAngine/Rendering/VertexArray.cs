using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class VertexArray : IDisposable
    {
        public readonly struct Attribute
        {
            internal readonly int Index;
            internal readonly int Count;
            internal readonly VertexAttribType Type;
            internal readonly int RelativeOffset;
            internal readonly int Divisor;

            public Attribute(int index, int count, VertexAttribType type, int relativeOffset, int divisor = 0)
            {
                Index = index;
                Count = count;
                Type = type;
                RelativeOffset = relativeOffset;
                Divisor = divisor;
            }

            public static implicit operator Attribute((int index, int count, VertexAttribType type, int relativeOffset) values)
                => new Attribute(values.index, values.count, values.type, values.relativeOffset, 0);
            public static implicit operator Attribute((int index, int count, VertexAttribType type, int relativeOffset, int divisor) values)
                => new Attribute(values.index, values.count, values.type, values.relativeOffset, values.divisor);
        }
        
        internal readonly int Handle;
        private int _vertexBufferCount = 0;

        public VertexArray()
        {
            GL.CreateVertexArrays(1, out Handle);
        }

        public void AddVertexBuffer<T>(Buffer<T> buffer, int offset, int stride, params Attribute[] attributes)
            where T : unmanaged
        {
            GL.VertexArrayVertexBuffer(Handle, _vertexBufferCount, buffer.Handle, (IntPtr)offset, stride);
            foreach (var attribute in attributes)
            {
                GL.VertexArrayAttribBinding(Handle, attribute.Index, _vertexBufferCount);
                GL.EnableVertexArrayAttrib(Handle, attribute.Index);
                GL.VertexArrayAttribFormat(Handle, attribute.Index, attribute.Count, attribute.Type, false, attribute.RelativeOffset);
            }
            _vertexBufferCount++;
        }

        public void SetElementBuffer(Buffer<uint> buffer)
        {
            GL.VertexArrayElementBuffer(Handle, buffer.Handle);
        }

        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(Handle);
        }
    }
}