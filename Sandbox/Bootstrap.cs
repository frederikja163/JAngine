using JAngine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Desktop;

namespace Sandbox;

public class Bootstrap : IBootstrap
{
    public void EngineInitialized()
    {
        GameWindow window = new GameWindow(GameWindowSettings.Default, NativeWindowSettings.Default);
        window.Load += () => GL.ClearColor(1, 0, 1, 1);
        window.RenderFrame += args =>
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            window.SwapBuffers();
        };
        Engine.AddWindow(window);
    }
}