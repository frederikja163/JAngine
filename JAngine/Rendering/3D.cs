using System.Globalization;
using System.Numerics;

namespace JAngine.Rendering;

public readonly struct Vertex3D
{
    [ShaderAttribute("vPosition")]
    public readonly Vector3 Position;
    [ShaderAttribute("vTexCoord")]
    public readonly Vector2 TexCoord;
    [ShaderAttribute("vNormal")]
    public readonly Vector3 Normal;

    public Vertex3D(Vector3 position, Vector2 texCoord, Vector3 normal)
    {
        Position = position;
        TexCoord = texCoord;
        Normal = normal;
    }

    public Vertex3D(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);
    }
}


public readonly struct Instance3D
{
    [ShaderAttribute("Transform")]
    public readonly Matrix4x4 Transformation;
}

public sealed class Mesh3D : Mesh<Vertex3D, Instance3D>
{
    private record VertexIndex(int PosIndex, int TexCoordIndex, int NormalIndex)
    {
        public override int GetHashCode()
        {
            return PosIndex * 2 + TexCoordIndex * 3 + NormalIndex * 5;
        }
    }
    
    private static Mesh3D LoadObjFile(Window window, string path)
    {
        using Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new StreamReader(stream);
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> texCoords = new List<Vector2>();
        List<Vector3> positions = new List<Vector3>();
        List<Vertex3D> vertices = new List<Vertex3D>();
        // This dictionary is used to deduplicate vertices,
        // so if two vertices are the same they will only be stored once.
        Dictionary<VertexIndex, int> vertexIndices = new Dictionary<VertexIndex, int>();
        List<List<int>> faces = new List<List<int>>();
        string? line;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        while ((line = reader.ReadLine()) != null)
        {
            string[] tokens = line.Trim().Split(' ');
            switch (tokens[0])
            {
                case "vn":
                    normals.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    continue;
                case "vt":
                    texCoords.Add(new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2])));
                    continue;
                case "v":
                    positions.Add(new Vector3(float.Parse(tokens[1]), float.Parse(tokens[2]), float.Parse(tokens[3])));
                    continue;
                case "f":
                    List<int> face = new List<int>();
                    for (int i = 1; i < tokens.Length; i++)
                    {
                        // TODO: Support other types of vertices like [0-9]+/[0-9]+.
                        
                        int[] vertIndices = tokens[i]
                            .Split('/')
                            .Select(int.Parse)
                            .ToArray();
                        VertexIndex vertexIndex = new VertexIndex(vertIndices[0], vertIndices[1], vertIndices[2]);
                        if (!vertexIndices.TryGetValue(vertexIndex, out int index))
                        {
                            index = vertices.Count;
                            vertexIndices.Add(vertexIndex, index);
                            vertices.Add(new Vertex3D(positions[vertexIndex.PosIndex - 1],
                                texCoords[vertexIndex.TexCoordIndex - 1],
                                normals[vertexIndex.NormalIndex - 1]));
                        }
                        face.Add(index);
                    }
                    faces.Add(face);
                    continue;
                case "#":
                    // Ignore as this is used for comments.
                    continue;
                default:
                    // TODO: Support materials properly.
                    continue;
            }
        }
        uint[] indices = faces
            .SelectMany(f => f)
            .Select(i => (uint)i)
            .ToArray();
        return new Mesh3D(window, Path.GetFileNameWithoutExtension(path) + ".mesh", vertices.ToArray(), indices);
    }

    public Mesh3D(Window window, string name, Vertex3D[] vertices, uint[] indices) : base(window, name, vertices, indices)
    {
    }
}
