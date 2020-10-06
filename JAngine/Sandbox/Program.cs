using System.IO;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using Window = JAngine.Window;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(800, 600, "Sandbox");
            GL.ClearColor(1, 0, 1, 1);

            using var shader = new Shader(File.OpenText("shader.vert"), File.OpenText("shader.frag"));
            shader.Bind();
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                window.Clear();
                
                Quad.Draw();
                
                window.SwapBuffers();
            }
        }
    }
}