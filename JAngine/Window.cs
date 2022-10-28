using JAngine.Entities;
using JAngine.Reflection;
using JAngine.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace JAngine;
public sealed class Window
{
    private readonly GameWindow _window;
    private Level? _level = null;
    public event Action? OnInit;

    public Window()
    {
        _window = new GameWindow(new GameWindowSettings
        {
            IsMultiThreaded = false,
            RenderFrequency = 60,
            UpdateFrequency = 60,
        },
        new NativeWindowSettings
        {
            API = ContextAPI.OpenGL,
            APIVersion = new Version(4, 6),
            StartFocused = true,
            StartVisible = true,
            Size = new Vector2i(1280, 720),
            Title = "Bad Ice Cream game | re-created by FrederikJA",
            AutoLoadBindings = true,
            IsEventDriven = false,
            IsFullscreen = false,
            Profile = ContextProfile.Core,
            WindowState = WindowState.Normal,
        });

        _window.RenderFrame += RenderFrame;
        _window.Load += Init;

        Camera3D.Main = new Camera3D((float)_window.ClientSize.X / _window.ClientSize.Y, new Vector3(0, 30, -20));
        Assemblies.AddType<RenderingSystem>();
    }

    private void Init()
    {
        Renderer.Init();
        OnInit?.Invoke();
    }

    private void RenderFrame(FrameEventArgs obj)
    {
        _window.ProcessEvents();

        _level?.UpdateAllSystems((float)_window.RenderTime);

        Renderer.Clear();
        Renderer.RenderScene();

        _window.SwapBuffers();
    }

    public void SwapLevel(Level level)
    {
        _level = level;
    }

    public void Run()
    {
        _window.Run();
    }
}