using System;
using System.Collections.Generic;
using System.Threading;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    internal static class GlfwTracker
    {
        private static int _glfwReferences;
        
        public static void StartTracking()
        {
            if (_glfwReferences++ <= 0)
            {
                if (!GLFW.Init())
                {
                    throw new Exception("Glfw failed to init");
                }
            }
        }

        public static void StopTracking()
        {
            if (--_glfwReferences <= 0)
            {
                GLFW.Terminate();
            }
        }
    }
    
    // TODO: Create better windowing.
    public sealed unsafe class Window : IDisposable
    {
        private readonly IContainer<Window> _container;
        internal readonly GlfwWindow* Handle;
        private readonly Queue<Action> _queue = new();
        private readonly Thread _thread;
        private readonly List<(VertexArray vao, ShaderProgram shader)> _drawables = new ();
        
        public void Queue(Action command)
        {
            _queue.Enqueue(command);
        }
        
        public Window(IContainer<Window> container, int width, int height, string title)
        {
            _container = container;
            _container.Add(this);
            Handle = GLFW.CreateWindow(width, height, title, null, null);
            
            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            GLFW.MakeContextCurrent(Handle);
            GLLoader.LoadBindings(new GLFWBindingsContext());
            while (IsOpen)
            {
                while (_queue.TryDequeue(out Action? command))
                {
                    command();
                }

                foreach ((VertexArray vao, ShaderProgram shader) in _drawables)
                {
                    vao.Bind();
                    shader.Bind();
            
                    GL.DrawElements(PrimitiveType.Triangles, vao.ElementBuffer.Size, DrawElementsType.UnsignedInt, 0);
                }
                GLFW.SwapBuffers(Handle);
            }
        }
        
        public void Draw(VertexArray vao, ShaderProgram shader)
        {
            _drawables.Add((vao, shader));
        }

        public bool IsOpen => !GLFW.WindowShouldClose(Handle);

        public void Dispose()
        {
            GLFW.DestroyWindow(Handle);
            _container.Remove(this);
        }
    }
}