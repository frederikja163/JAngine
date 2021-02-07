﻿using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    public sealed unsafe class Window : IDisposable
    {
        private static readonly List<Window> Windows = new List<Window>();
        private readonly GlfwWindow* _handle;
        private Vector2i _size;
        private readonly Mouse _mouse;
        private readonly Keyboard _keyboard;

        public Window(int width, int height, string title)
        {
            if (Windows.Count == 0)
            {
                if (!GLFW.Init())
                {
                    throw new Exception("Glfw failed to initialize!");
                }
            }
            Windows.Add(this);
            
            _handle = GLFW.CreateWindow(width, height, title, null, null);
            if (_handle == null)
            {
                throw new Exception("Window failed to create");
            }
            _mouse = new Mouse(_handle);
            _keyboard = new Keyboard(_handle);
            _size = new Vector2i(width, height);
        }

        public bool IsRunning => !GLFW.WindowShouldClose(_handle);

        public Mouse Mouse => _mouse;

        public Keyboard Keyboard => _keyboard;

        public int Width
        {
            get => _size.X;
        }
        
        public int Height
        {
            get => _size.Y;
        }

        public Vector2i Size
        {
            get => _size;
            set
            {
                _size = value;
                GLFW.SetWindowSize(_handle, _size.X, _size.Y);
            }
        }

        public void MakeCurrent()
        {
            GLFW.MakeContextCurrent(_handle);
        }

        public void SwapBuffers()
        {
            GLFW.SwapBuffers(_handle);
        }

        public void PollEvents()
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
            Windows.Remove(this);
            if (Windows.Count == 0)
            {
                GLFW.Terminate();
            }
        }
    }
}