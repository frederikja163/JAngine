#nullable enable
using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public sealed unsafe class VertexArray : GlObject
    {
        public record Attribute(uint Location, int ValueCount, VertexAttribType Type);
        
        private uint _vertexBufferCount = 0;
        public readonly ElementBuffer ElementBuffer;

        public VertexArray(Window window, ElementBuffer elementBuffer) : base(window, GL.CreateVertexArray)
        {
            ElementBuffer = elementBuffer;
            window.Queue(() =>
            {
                GL.VertexArrayElementBuffer(Handle, elementBuffer.Handle);
            });
        }

        public void AddVertexBuffer<T>(VertexBuffer<T> buffer, params Attribute[] attributes)
            where T : unmanaged
        {
            Window.Queue(() =>
            {
                GL.VertexArrayVertexBuffer(Handle, ++_vertexBufferCount, buffer.Handle, IntPtr.Zero, sizeof(T));
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

        public override void Dispose()
        {
            Window.Queue(() =>
            {
                GL.DeleteVertexArray(Handle);
            });
        }
    }
}