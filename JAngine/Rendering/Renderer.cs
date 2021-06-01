using System;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering
{
    public sealed class Renderer : IDisposable
    {
        public void Draw(VertexArray vao, ShaderProgram shader)
        {
            vao.Bind();
            shader.Bind();
            
            GL.DrawElements(PrimitiveType.Triangles, vao.ElementBuffer.Size, DrawElementsType.UnsignedInt, 0);
        }
        
        public void Dispose()
        {
            
        }
    }
}