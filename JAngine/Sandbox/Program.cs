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

            window.Keyboard.OnKey += (sender, eventArgs) =>
            {
                if (eventArgs.State != KeyState.Released)
                {
                    Console.WriteLine(eventArgs.Key + " - " + eventArgs.State);
                }
                return false;
            };
            window.Mouse.OnMouseButton += (sender, eventArgs) =>
            {
                if (eventArgs.State != MouseButtonState.Released)
                {
                    Console.WriteLine(eventArgs.Button + " - " + eventArgs.State);
                }
                return false;
            };
            
            while (window.IsOpen)
            {
                window.PollInput();
                
                window.Clear();

                if (window.Keyboard.IsPressed(Key.A))
                {
                    angle += 0.1f;
                }
                if (window.Keyboard.IsPressed(Key.D))
                {
                    angle -= 0.1f;
                }

                var mat = Matrix4.CreateRotationY(angle);
                shader.SetUniform("uView", ref mat);
                
                q.Draw();
                
                window.SwapBuffers();
            }
        }
    }
}