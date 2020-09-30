using System;
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

    public enum Key
    {
        Unknown = -1,
        Space = 32,
        Apostrophe = 39,
        Comma = 44,
        Minus = 45,
        Period = 46,
        Slash = 47,
        Key0 = 48,
        Key1 = 49,
        Key2 = 50,
        Key3 = 51,
        Key4 = 52,
        Key5 = 53,
        Key6 = 54,
        Key7 = 55,
        Key8 = 56,
        Key9 = 57,
        Semicolon = 59,
        Equal = 61,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LeftBracket = 91, 
        BackSlash = 92, 
        RightBracket = 93, 
        Grave = 96, 
        World1 = 161,
        World2 = 162,
        Escape = 256,
        Enter = 257,
        Tab = 258,
        Backspace = 259,
        Insert = 260,
        Delete = 261,
        Right = 262,
        Left = 263,
        Up = 264,
        Down = 265,
        PageUp = 266,
        PageDown = 267,
        Home = 268,
        End = 269,
        CapsLock = 280,
        ScrollLock = 281,
        NumLock = 282,
        PrintScreen = 283,
        Pause = 284,
        F1 = 290,
        F2 = 291,
        F3 = 292,
        F4 = 293,
        F5 = 294,
        F6 = 295,
        F7 = 296,
        F8 = 297,
        F9 = 298,
        F10 = 299,
        F11 = 300,
        F12 = 301,
        F13 = 302,
        F14 = 303,
        F15 = 304,
        F16 = 305,
        F17 = 306,
        F18 = 307,
        F19 = 308,
        F20 = 309,
        F21 = 310,
        F22 = 311,
        F23 = 312,
        F24 = 313,
        F25 = 314,
        Keypad0 = 320,
        Keypad1 = 321,
        Keypad2 = 322,
        Keypad3 = 323,
        Keypad4 = 324,
        Keypad5 = 325,
        Keypad6 = 326,
        Keypad7 = 327,
        Keypad8 = 328,
        Keypad9 = 329,
        KeypadDecimal = 330,
        KeypadDivide = 331,
        KeypadMultiply = 332,
        KeypadSubtract = 333,
        KeypadAdd = 334,
        KeypadEnter = 335,
        KeypadEqual = 336,
        LeftShift = 340,
        LeftControl = 341,
        LeftAlt = 342,
        LeftSuper = 343,
        RightShift = 344,
        RightControl = 345,
        RightAlt = 346,
        RightSuper = 347,
        Men = 348
    }

    public enum KeyState
    {
        JustPressed = 0,
        Pressed = 1,
        JustReleased = 2,
        Released = 3
    }
    
    public enum MouseButtonState
    {
        JustPressed = 0,
        Pressed = 1,
        JustReleased = 2,
        Released = 3
    }
    
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

        public struct KeyEventArgs
        {
            public Key Key { get; }
            public int KeyCode { get; }
            public KeyState State { get; }

            internal KeyEventArgs(Key key, int keyCode, KeyState state)
            {
                Key = key;
                KeyCode = keyCode;
                State = state;
            }
        }

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

            GLFW.SetKeyCallback(_window, (window, key, code, action, mods) =>
            {
                
            });

            GLFW.SetMouseButtonCallback(_window, (window, button, action, mods) =>
            {

            });
        }
        
        public bool IsOpen
        {
            get => !GLFW.WindowShouldClose(_window);
        }

        public void Close()
        {
            GLFW.SetWindowShouldClose(_window, true);
        }

        public void PollInput()
        {
            GLFW.PollEvents();
        }

        public delegate bool MouseEvent(MouseButtonEventArgs e);
        public delegate bool KeyEvent(KeyEventArgs e);

        public Action<Vector2>? OnMouseMoved;
        public MouseEvent? OnMouseButton;
        public KeyEvent? OnKey;
    }
}