#nullable enable
using System;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public sealed unsafe class VertexArray : GlObject<VertexArrayHandle>
    {
        public record Attribute(uint Location, int ValueCount, VertexAttribType Type);
        
        private uint _vertexBufferCount = 0;
        public readonly ElementBuffer ElementBuffer;

        public VertexArray(ElementBuffer elementBuffer) : base(elementBuffer.Window, GL.CreateVertexArray, GL.DeleteVertexArray)
        {
            ElementBuffer = elementBuffer;
            Window.Queue(() =>
            {
                GL.VertexArrayElementBuffer(Handle, elementBuffer.Handle);
            });
        }

        public void AddVertexBuffer<T>(VertexBuffer<T> buffer, uint divisor, params Attribute[] attributes)
            where T : unmanaged, IVertex
        {
            if (buffer.Window != Window)
            {
                throw new Exception("Vertex buffer must be from the same window context as the Vertex Array");
            }

            Window.Queue(() =>
            {
                GL.VertexArrayVertexBuffer(Handle, ++_vertexBufferCount, buffer.Handle, IntPtr.Zero, sizeof(T));
                GL.VertexArrayBindingDivisor(Handle, _vertexBufferCount, divisor);
                int offset = 0;
                foreach (Attribute attribute in attributes)
                {
                    GL.VertexArrayAttribBinding(Handle, attribute.Location, _vertexBufferCount);
                    GL.VertexArrayAttribFormat(Handle, attribute.Location, attribute.ValueCount, attribute.Type, false,
                        (uint)offset);
                    GL.EnableVertexArrayAttrib(Handle, attribute.Location);
                    int attributeSize = attribute.Type switch
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
                    offset += attributeSize * attribute.ValueCount;
                }
            });
        }
    }
}