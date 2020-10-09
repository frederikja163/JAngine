using System;
using JAngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    public sealed unsafe class Window : IDisposable
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

        private static int _totalWindows = 0;
        private static bool _isOpenglLoaded = false;
        private GlfwWindow* _handle;
        
        public Window(int width, int height, string title) : this(new ConstructorParameters(width, height, title))
        {
        }

        public Window(ConstructorParameters parameters)
        {
            ConstructorParameters p = parameters;
            if (_totalWindows == 0)
            {
                //Do all the hint thingies here
                if (!GLFW.Init())
                {
                    throw new Exception("Glfw failed to initialize");
                }
            }

            _handle = GLFW.CreateWindow(p.Width, p.Height, p.Title, null, null);
            GLFW.MakeContextCurrent(_handle);
            GLFW.SwapInterval(0);
            _totalWindows++;

            if (!_isOpenglLoaded)
            {
                GL.LoadBindings(new GLFWBindingsContext());
                GL.CullFace(CullFaceMode.Front);
            }

            Mouse = new Mouse(_handle);
            Keyboard = new Keyboard(_handle);
        }
        
        public bool IsOpen
        {
            get => !GLFW.WindowShouldClose(_handle);
        }

        public void Close()
        {
            GLFW.SetWindowShouldClose(_handle, true);
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(_handle);
        }

        public void PollInput()
        {
            Mouse.PrePoll();
            Keyboard.PrePoll();
            GLFW.PollEvents();
            Mouse.PostPoll();
            Keyboard.PostPoll();
        }

        public void Dispose()
        {
            GLFW.DestroyWindow(_handle);
            _totalWindows--;
            if (_totalWindows == 0)
            {
                GLFW.Terminate();
            }
        }
        
        public Mouse Mouse { get; }
        public Keyboard Keyboard { get; }
    }
}