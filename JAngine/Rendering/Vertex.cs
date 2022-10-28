using OpenTK.Mathematics;

namespace JAngine.Rendering;

public readonly struct Vertex3D
{
    public readonly Vector3 Position;
    public readonly Vector2 TexCoord;
    public readonly Vector3 Normal;

    public Vertex3D(Vector3 position, Vector2 texCoord, Vector3 normal)
    {
        Position = position;
        TexCoord = texCoord;
        Normal = normal;
    }
}
