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
                GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
                GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 6);
                GLFW.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGlApi);
                GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
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
        private readonly List<IDrawable> _drawables = new ();
        
        public void Queue(Action command)
        {
            _queue.Enqueue(command);
        }
        
        public Window(IContainer<Window> container, int width, int height, string title)
        {
            _container = container;
            Handle = GLFW.CreateWindow(width, height, title, null, null);
            _container.Add(this);

            _thread = new Thread(Run);
            _thread.Start();
        }

        private void Run()
        {
            try
            {
                GLFW.MakeContextCurrent(Handle);
                GLLoader.LoadBindings(new GLFWBindingsContext());
                while (IsOpen)
                {
                    while (_queue.TryDequeue(out Action? command))
                    {
                        command();
                    }

                    foreach (IDrawable drawable in _drawables)
                    {
                        GL.BindVertexArray(drawable.VertexArray.Handle);
                        GL.UseProgram(drawable.Shader.Handle);

                        int texLocation = GL.GetUniformLocation(drawable.Shader.Handle, "uTexture[0]");
                        GL.BindTextures(0, drawable.Textures.TextureHandles);
                        for (int i = 0; i < 32; i++)
                        {
                            GL.ProgramUniform1i(drawable.Shader.Handle, texLocation + i, i);
                        }

                        GL.DrawElementsInstanced(PrimitiveType.Triangles, drawable.VertexArray.ElementBuffer.Size,
                            DrawElementsType.UnsignedInt, 0, drawable.InstanceCount);
                    }

                    GLFW.SwapBuffers(Handle);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        internal void AddDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }

        internal void RemoveDrawable(IDrawable drawable)
        {
            _drawables.Remove(drawable);
        }

        public bool IsOpen => !GLFW.WindowShouldClose(Handle);

        public void Dispose()
        {
            GLFW.DestroyWindow(Handle);
            _container.Remove(this);
        }
    }
}