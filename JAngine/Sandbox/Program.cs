using System;
using System.IO;
using JAngine;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = JAngine.Window;

namespace Sandbox
{
    class Program
    {
        private static readonly float[] _verts = {
            0f, 0.5f, 0f,
            0.5f, 0.5f, 0f,
            0.5f, 0f, 0f
        };

        private static readonly uint[] _indices = {
            0, 1, 2
        };
        
        static void Main(string[] args)
        {
            Window window = new Window(800, 600, "Sandbox");
            GL.ClearColor(1, 0, 1, 1);

            var shader = new Shader(File.OpenText("shader.vert"), File.OpenText("shader.frag"));
            shader.Bind();

            var vbo = new VertexBuffer(_verts);
            var layout = new AttributeLayout(1);
            layout.AddAttribute<float>(0, 3);
            
            var ebo = new ElementBuffer(_indices);
            var vao = new VertexArray();
            vao.SetElementBuffer(ebo);
            vao.AddAttributes(vbo, layout);
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                vao.Bind();
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                vao.Unbind();
                
                window.SwapBuffers();
            }
        }
    }
}