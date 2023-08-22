using System.Numerics;
using JAngine.Core;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public static unsafe class Renderer
{
    public static readonly Glfw.Window ShareContext;
    static Renderer()
    {
        Glfw.GetVersion(out int major, out int minor, out int rev);
        Log.Info($"Glfw version {major}.{minor}.{rev}");
        if (!Glfw.Init())
        {
            throw new Exception("Failed to initialize Glfw");
        }
        
        Glfw.WindowHint(Glfw.Hint.ContextVersionMajor, 4);
        Glfw.WindowHint(Glfw.Hint.ContextVersionMinor, 6);
        Glfw.WindowHint(Glfw.Hint.OpenglProfile, Glfw.OpenGL.CoreProfile);
        Glfw.WindowHint(Glfw.Hint.Visible, false);
        ShareContext = Glfw.CreateWindow(0, 0, "__share_context__", Glfw.Monitor.Null, Glfw.Window.Null);
        Glfw.WindowHint(Glfw.Hint.Visible, true);
    }
    
    public static void ClearColor(float r, float g, float b, float a) => Gl.ClearColor(r, g, b, a);

    internal static void Clear() => Gl.Clear(Gl.ClearBufferMask.ColorBuffer);
}