using System;
using System.IO;
using JAngine;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
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
            var q = new Quad();
            var angle = 0f;
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                window.Clear();

                if (window.Keyboard[Key.A] == KeyState.JustPressed)
                {
                    angle += 0.01f;
                }
                if (window.Keyboard[Key.D] == KeyState.JustPressed)
                {
                    angle -= 0.01f;
                }

                var mat = Matrix4.CreateRotationY(angle);
                shader.SetUniform("uView", ref mat);
                
                q.Draw();
                
                window.SwapBuffers();
            }
        }
    }
}