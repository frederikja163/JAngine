using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public interface IRenderable : IDisposable
{
    internal Window Window { get; }
    internal VertexArray Vao { get; }
    internal Shader Shader { get; }
}

public sealed class Renderable : IRenderable
{
    private readonly Window _window;
    
    public Renderable(Window window, VertexArray vao, Shader shader)
    {
        _window = window;
        Vao = vao;
        Shader = shader;
        _window.AddRenderable(this);
    }

    public void Dispose()
    {
        _window.RemoveRenderable(this);
    }

    Window IRenderable.Window => _window;
    public VertexArray Vao { get; set; }
    public Shader Shader { get; set; }
}
