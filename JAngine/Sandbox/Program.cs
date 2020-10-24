using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
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
                
                
                
                window.SwapBuffers();
            }
        }
    }
}