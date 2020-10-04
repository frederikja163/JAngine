using System;
using System.IO;
using JAngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = JAngine.Window;

namespace Sandbox
{
    class Program
    {
        private static readonly float[] _verts = new []
        {
            0f, 0.5f, 0f,
            0.5f, 0.5f, 0f,
            0.5f, 0f, 0f
        };
        
        static void Main(string[] args)
        {
            Window window = new Window(800, 600, "Sandbox");
            GL.LoadBindings(new GLFWBindingsContext());
            GL.ClearColor(1, 0, 1, 1);

            var shader = new Shader(File.OpenText("shader.vert"), File.OpenText("shader.frag"));
            shader.Bind();

            var vbo = new VertexBufferObject(_verts);
            vbo.Bind();
            vbo.SetAttributes();
            vbo.Unbind();
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
                
                window.SwapBuffers();
            }
        }
    }
}