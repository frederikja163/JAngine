using System;
using System.Collections.Generic;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace JAngine
{
    public sealed class Engine : IDisposable, IContainer<Window>, IContainer<GameLoop>
    {
        private readonly List<Window> _windows = new ();
        private readonly List<GameLoop> _gameLoops = new();

        public Engine()
        {
            GlfwTracker.StartTracking();
        }

        public void Run()
        {
            while (_windows.Count != 0)
            {
                GLFW.PollEvents();
            }
        }

        public void Dispose()
        {
            
            GlfwTracker.StopTracking();
        }

        List<Window> IContainer<Window>.Items => _windows;

        List<GameLoop> IContainer<GameLoop>.Items => _gameLoops;
    }
}