﻿
using OpenTK.Windowing.GraphicsLibraryFramework;
using GlfwWindow = OpenTK.Windowing.GraphicsLibraryFramework.Window;

namespace JAngine
{
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
        JustPressed = 1,
        Pressed = 2,
        JustReleased = 0,
        Released = 3
    }

    public sealed unsafe class Keyboard
    {
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

        internal Keyboard(GlfwWindow* handle)
        {
            GLFW.SetKeyCallback(handle, (window, keyRaw, code, action, mods) =>
            {
                var key = (Key) keyRaw;
                var state = (KeyState) action;
                var args = new KeyEventArgs(key, code, state);
                var delegates = OnKey.GetInvocationList();
                foreach (var d in delegates)
                {
                    var e = (KeyEvent) d;
                    if (e.Invoke(this, args))
                    {
                        return;
                    }
                }
            });
        }
        
        public delegate bool KeyEvent(Keyboard sender, KeyEventArgs e);

        public KeyEvent? OnKey;
    }
}