using System;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 3,
        Button1 = 4,
        Button2 = 5,
        Button3 = 6,
        Button4 = 7,
    }
    
    public enum MouseButtonState
    {
        JustPressed = 1,
        Pressed = 2,
        JustReleased = 0,
        Released = 3
    }

    public unsafe class Mouse
    {
        public struct MouseButtonEventArgs
        {
            public MouseButton Button { get; }
            public MouseButtonState State { get; }
            public Vector2 MousePosition { get; }

            internal MouseButtonEventArgs(MouseButton button, MouseButtonState state, Vector2 mousePosition)
            {
                Button = button;
                State = state;
                MousePosition = mousePosition;
            }
        }

        private Dictionary<MouseButton, MouseButtonState> _buttonStates;
        
        internal Mouse(GlfwWindow* handle)
        {
            GLFW.GetCursorPos(handle, out var x, out var y);
            Position = new Vector2((float)x, (float)y);
            
            _buttonStates = new Dictionary<MouseButton, MouseButtonState>();
            
            GLFW.SetMouseButtonCallback(handle, (window, buttonRaw, actionRaw, mods) =>
            {
                var button = (MouseButton) buttonRaw;
                var state = (MouseButtonState) actionRaw;
                var args = new MouseButtonEventArgs(button, state, Position);
                
                if (!_buttonStates.TryAdd(button, state))
                {
                    _buttonStates[button] = state;
                }
                
                var delegates = OnMouseButton?.GetInvocationList();
                if (delegates == null) return;
                foreach (var d in delegates)
                {
                    var e = (MouseButtonEvent) d;
                    if (e.Invoke(this, args))
                    {
                        return;
                    }
                }
            });

            GLFW.SetCursorPosCallback(handle, (window, x, y) =>
            {
                Position = new Vector2((float)x, (float)y);
                var delegates = OnMouseMoved?.GetInvocationList();
                if (delegates == null) return;
                foreach (var d in delegates)
                {
                    var e = (MouseMoveEvent) d;
                    if (e.Invoke(this, Position))
                    {
                        return;
                    }
                }
            });
        }

        public MouseButtonState this[MouseButton b] =>
            _buttonStates.TryGetValue(b, out var s) ? s : MouseButtonState.Released; 

        public delegate bool MouseButtonEvent(Mouse sender, MouseButtonEventArgs e);
        public delegate bool MouseMoveEvent(Mouse sender, Vector2 position);
        
        public MouseMoveEvent? OnMouseMoved;
        public MouseButtonEvent? OnMouseButton;
        
        public Vector2 Position { get; private set; }
    }
}