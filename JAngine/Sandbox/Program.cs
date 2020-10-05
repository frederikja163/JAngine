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

            var layout = new VertexLayout(1);
            layout.AddAttribute<float>(3);
            var vbo = new VertexBufferObject(_verts, layout);
            vbo.Bind();
            
            var ebo = new ElementBufferObject(_indices);
            ebo.Bind();
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                
                GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
                
                window.SwapBuffers();
            }
        }
    }
}