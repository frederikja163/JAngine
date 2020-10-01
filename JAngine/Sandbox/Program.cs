using System;
using JAngine;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Window window = new Window(800, 600, "Sandbox");
            window.Mouse.OnMouseButton += (s, e) =>
            {
                Console.Write("1");
                return false;
            };
            window.Mouse.OnMouseButton += (s, e) =>
            {
                Console.Write("2");
                return true;
            };
            window.Mouse.OnMouseButton += (s, e) =>
            {
                Console.Write("3");
                return true;
            };
            while (window.IsOpen)
            {
                window.PollInput();
            }
        }
    }
}