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
        
        private readonly int _handle;
        private int _vertexBufferCount = 0;
        internal int Handle => _handle;

        public VertexArray()
        {
            GL.CreateVertexArrays(1, out _handle);
        }

        public void AddVertexBuffer<T>(Buffer<T> buffer, int offset, int stride, params Attribute[] attributes)
            where T : unmanaged
        {
            GL.VertexArrayVertexBuffer(_handle, _vertexBufferCount, buffer.Handle, (IntPtr)offset, stride);
            foreach (var attribute in attributes)
            {
                GL.VertexArrayAttribBinding(_handle, attribute.Index, _vertexBufferCount);
                GL.EnableVertexArrayAttrib(_handle, attribute.Index);
                GL.VertexArrayAttribFormat(_handle, attribute.Index, attribute.Count, attribute.Type, false, attribute.RelativeOffset);
            }
            _vertexBufferCount++;
        }

        public void SetElementBuffer<T>(Buffer<T> buffer)
            where T : unmanaged
        {
            GL.VertexArrayElementBuffer(_handle, buffer.Handle);
        }

        public void Draw()
        {
            GL.BindVertexArray(_handle);
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }
        
        public void Dispose()
        {
            GL.DeleteVertexArray(_handle);
        }
    }
}