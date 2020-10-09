using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class VertexArray : IDisposable, IDrawable
    {
        private readonly int _handle;
        private ElementBuffer _ebo;

        public VertexArray(ElementBuffer ebo)
        {
            _handle = GL.GenVertexArray();
            _ebo = ebo;
            SetElementBuffer(ebo);
        }

        public void AddAttribute<T>(VertexBuffer<T> vbo, int count, int location, int divisor = 0)
            where T : unmanaged
        {
            AddAttributes(vbo, new AttributeLayout(1).AddAttribute<T>(count, location, divisor));
        }

        public void AddAttributes<T>(VertexBuffer<T> vbo, AttributeLayout layout)
            where T : unmanaged
        {
            Bind();
            vbo.Bind();
            int offSet = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                var attr = layout[i];
                GL.VertexAttribPointer(attr.Location, attr.Count, attr.Type, false, layout.Stride, offSet);
                GL.EnableVertexAttribArray(attr.Location);
                if (attr.Divisor != 0)
                {
                    GL.VertexAttribDivisor(attr.Location, attr.Divisor);
                }
                offSet += attr.Count * attr.Size;
            }
            vbo.Unbind();
            Unbind();
        }

        public void SetElementBuffer(ElementBuffer ebo)
        {
            _ebo = ebo;
            Bind();
            ebo.Bind();
            Unbind();
            ebo.Unbind();
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            Draw(_ebo.Count, 0);
        }

        public void Draw(int count, int start = 0, int instances = 1)
        {
            Bind();
            // for (int i = 0; i < instances; i++)
            // {
            //     
            // GL.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, (IntPtr)start);
            // }
            GL.DrawElementsInstanced(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, (IntPtr)start, instances);
            Unbind();
        }
        
        public void Dispose()
        {
            GL.DeleteVertexArray(_handle);
        }
    }
}