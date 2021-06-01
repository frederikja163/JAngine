using System;
using JAngine.Rendering;
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
        public Renderer Renderer { get; }
        
        public Window(IContainer<Window> container, int width, int height, string title)
        {
            _container = container;
            _container.Add(this);
            Handle = GLFW.CreateWindow(width, height, title, null, null);
            
            Renderer = new ();
        }

        public void Bind()
        {
            GLFW.MakeContextCurrent(Handle);
        }

        public bool IsOpen => !GLFW.WindowShouldClose(Handle);

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(Handle);
        }

        public void Dispose()
        {
            GLFW.DestroyWindow(Handle);
            _container.Remove(this);
        }
    }
}