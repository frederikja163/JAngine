using System;
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
        internal readonly GlfwWindow* Handle;

        public Window(int width, int height, string title)
        {
            GlfwTracker.StartTracking();

            Handle = GLFW.CreateWindow(width, height, title, null, null);
        }

        public void MakeCurrent()
        {
            GLFW.MakeContextCurrent(Handle);
        }

        public void PollInput()
        {
            GLFW.PollEvents();
        }

        public bool IsOpen => !GLFW.WindowShouldClose(Handle);

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(Handle);
        }

        public void Dispose()
        {
            GLFW.DestroyWindow(Handle);
            GlfwTracker.StopTracking();
        }
    }
}