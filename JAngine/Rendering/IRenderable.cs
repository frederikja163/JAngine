using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public interface IRenderable
{
    internal void Render();
}

internal sealed class Renderable : IRenderable
{
    private readonly VertexArray _vao;
    private readonly Shader _shader;

    public Renderable(VertexArray vao, Shader shader)
    {
        _vao = vao;
        _shader = shader;
    }

    void IRenderable.Render()
    {
        _vao.Bind();
        _shader.Bind();
        Gl.DrawElementsInstanced(Gl.PrimitiveType.Triangles, 6, Gl.DrawElementsType.UnsignedInt, 0, 1);
    }
}
