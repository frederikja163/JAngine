#nullable enable
using System;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public sealed unsafe class VertexArray : IDisposable
    {
        public record Attribute(uint Location, int ValueCount, VertexAttribType Type);
        
        internal readonly uint Handle;
        private uint _vertexBufferCount = 0;
        public readonly ElementBuffer ElementBuffer;

        public VertexArray(ElementBuffer elementBuffer)
        {
            Handle = GL.CreateVertexArray();
            ElementBuffer = elementBuffer;
            GL.VertexArrayElementBuffer(Handle, elementBuffer.Handle);
        }

        public void AddVertexBuffer<T>(VertexBuffer<T> buffer, params Attribute[] attributes)
            where T : unmanaged
        {
            GL.VertexArrayVertexBuffer(Handle, ++_vertexBufferCount, buffer.Handle, IntPtr.Zero, sizeof(T));
            uint offset = 0;
            foreach (Attribute attribute in attributes)
            {
                GL.VertexArrayAttribBinding(Handle, attribute.Location, _vertexBufferCount);
                GL.VertexArrayAttribFormat(Handle, attribute.Location, attribute.ValueCount, attribute.Type, false, offset);
                GL.EnableVertexArrayAttrib(Handle, attribute.Location);
                offset += attribute.Type switch
                {
                    VertexAttribType.Byte => sizeof(sbyte),
                    VertexAttribType.UnsignedByte => sizeof(byte),
                    VertexAttribType.Short => sizeof(short),
                    VertexAttribType.UnsignedShort => sizeof(ushort),
                    VertexAttribType.Int => sizeof(int),
                    VertexAttribType.UnsignedInt => sizeof(uint),
                    VertexAttribType.Float => sizeof(float),
                    VertexAttribType.Double => sizeof(double),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        internal void Bind()
        {
            GL.BindVertexArray(Handle);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(Handle);
        }
    }
}