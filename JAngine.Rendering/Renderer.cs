using JAngine.Core;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

internal sealed class RenderThreadBinding : IDisposable
{
    private readonly Glfw.Window _context;
    private int _bindCount = 0;

    internal RenderThreadBinding(Glfw.Window context)
    {
        _context = context;
    }

    internal void Bind()
    {
        if (_bindCount == 0)
        {
            Glfw.MakeContextCurrent(_context);
            Renderer.CurrentBinding = this;
        }
        _bindCount += 1;
    }
        
    
    public void Dispose()
    {
        _bindCount -= 1;
        if (_bindCount == 0)
        {
            Glfw.MakeContextCurrent(Glfw.Window.Null);
        }
    }
}

public static unsafe class Renderer
{
    private static List<RenderThreadBinding> _unusedBindings = new List<RenderThreadBinding>();
    [ThreadStatic] internal static RenderThreadBinding? CurrentBinding;
    internal static Glfw.Window ShareContext { get; }
    
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
    }

    private static RenderThreadBinding GetRenderThreadBinding()
    {
        if (CurrentBinding is not null)
        {
            return CurrentBinding;
        }

        lock (_unusedBindings)
        {
            if (_unusedBindings.Any())
            {
                return _unusedBindings.First();
            }
        }
        
        Glfw.WindowHint(Glfw.Hint.Visible, false);
        Glfw.Window threadContext = Glfw.CreateWindow(0, 0, "__thread_context__", Glfw.Monitor.Null, ShareContext);
        return new RenderThreadBinding(threadContext);
    }
    
    /// <summary>
    /// Should ONLY be called on an OpenGL thread.
    /// </summary>
    public static void ClearColor(float r, float g, float b, float a) => Gl.ClearColor(r, g, b, a);
    
    /// <summary>
    /// Should ONLY be called on an OpenGL thread.
    /// </summary>
    internal static void Clear() => Gl.Clear(Gl.ClearBufferMask.ColorBuffer);

    internal static RenderThreadBinding EnsureRenderThread()
    {
        RenderThreadBinding binding = GetRenderThreadBinding();
        binding.Bind();
        return binding;
    }

    internal static void ReturnRenderThreadBinding(RenderThreadBinding binding)
    {
        lock (_unusedBindings)
        {
            _unusedBindings.Add(binding);
        }
    }
}