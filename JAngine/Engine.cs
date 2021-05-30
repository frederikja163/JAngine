using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace JAngine
{
    public sealed class Engine : IDisposable
    {
        public Engine()
        {
            GlfwTracker.StartTracking();
        }

        public void Dispose()
        {
            GlfwTracker.StopTracking();
        }
    }
}