using System;
using System.Collections.Generic;
using System.Reflection;
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
            
            // TODO: The loggers should be based off of some options.
            Log.AddLogger(new ConsoleLogger());
            Log.AddLogger(new FileLogger("Log.txt"));
            
            Log.Info("Initialising engine.");
        }

        public void Run()
        {
            Log.Info("Running engine.");
            while (_windows.Count != 0)
            {
                GLFW.PollEvents();
            }
        }

        public void Dispose()
        {
            Log.Dispose();
            
            GlfwTracker.StopTracking();
        }

        List<Window> IContainer<Window>.Items => _windows;

        List<GameLoop> IContainer<GameLoop>.Items => _gameLoops;
    }
}