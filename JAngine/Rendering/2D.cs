using System.Numerics;

namespace JAngine.Rendering;

public readonly struct Vertex2D
{
    [ShaderAttribute("vPosition")]
    public readonly Vector2 Position;
    [ShaderAttribute("vTexCoord")]
    public readonly Vector2 TexCoord;

    public Vertex2D(Vector2 position, Vector2 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
    }
    
    public Vertex2D(float x, float y)
    {
        Position = new Vector2(x, y);
    }
}

public readonly struct Instance2D
{
    [ShaderAttribute("vColor")]
    public readonly Vector4 Color = Vector4.One;
    [ShaderAttribute("vTransform{0}")]
    public readonly Matrix4x4 Transformation;

    public Instance2D(Matrix4x4 transformation)
    {
        Transformation = transformation;
    }
}

public sealed class Mesh2D : Mesh<Vertex2D, Instance2D> {
    public Mesh2D(Window window, string name, Vertex2D[] vertices, uint[] indices)
        : base(window, name, vertices, indices)
    {
    }
}
