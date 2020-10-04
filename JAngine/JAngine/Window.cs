using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    public sealed unsafe class Window
    {
        public struct ConstructorParameters
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public string Title { get; set; }

            public ConstructorParameters(int width, int height, string title)
            {
                Width = width;
                Height = height;
                Title = title;
            }
        }

        private static bool _glfwInitialized;
        private GlfwWindow* _window;
        
        public Window(int width, int height, string title) : this(new ConstructorParameters(width, height, title))
        {
        }

        public Window(ConstructorParameters parameters)
        {
            ConstructorParameters p = parameters;
            if (!_glfwInitialized)
            {
                //Do all the hint thingies here
                if (!GLFW.Init())
                {
                    throw new Exception("Glfw failed to initialize");
                }
                _glfwInitialized = true;
            }

            _window = GLFW.CreateWindow(p.Width, p.Height, p.Title, null, null);
            GLFW.MakeContextCurrent(_window);

            Mouse = new Mouse(_window);
            Keyboard = new Keyboard(_window);
        }
        
        public bool IsOpen
        {
            get => !GLFW.WindowShouldClose(_window);
        }

        public void Close()
        {
            GLFW.SetWindowShouldClose(_window, true);
        }

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(_window);
        }

        public void PollInput()
        {
            GLFW.PollEvents();
        }
        
        public Mouse Mouse { get; }
        public Keyboard Keyboard { get; }
    }
}