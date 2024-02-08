using System.Globalization;
using System.Numerics;
using System.Reflection;
using JAngine.Rendering.OpenGL;
namespace JAngine.Rendering;

public readonly struct Vertex3D
{
    public readonly Vector3 vPosition;
    public readonly Vector2 TexCoord;
    public readonly Vector3 Normal;

    public Vertex3D(Vector3 position, Vector2 texCoord, Vector3 normal)
    {
        vPosition = position;
        TexCoord = texCoord;
        Normal = normal;
    }

    public Vertex3D(float x, float y, float z)
    {
        vPosition = new Vector3(x, y, z);
    }
}

public readonly struct Instance3D
{
}

public class Mesh<TVertex, TInstance> : IDisposable
    where TVertex : unmanaged
    where TInstance : unmanaged
{
    private static IReadOnlyDictionary<Type, (int count, Type type)> _csTypesToAttributes =
        new Dictionary<Type, (int count, Type type)>()
        {
            {typeof(int), (1, typeof(int))},
            {typeof(uint), (1, typeof(uint))},
            {typeof(float), (1, typeof(float))},
            {typeof(Vector2), (2, typeof(float))},
            {typeof(Vector3), (3, typeof(float))},
            {typeof(Vector4), (4, typeof(float))},
            {typeof(double), (1, typeof(double))}
        };

    private readonly List<VertexArray> _vaos = new List<VertexArray>();

    public Mesh(Window window, string name, TVertex[] vertices, uint[] indices)
    {
        Window = window;
        Name = name;
        VertexBuffer = new Buffer<TVertex>(window, name + ".vbo", Gl.BufferUsage.StaticDraw, vertices);
        InstanceBuffer = new Buffer<TInstance>(window, name + ".ivbo", Gl.BufferUsage.StaticDraw, 1);
        ElementBuffer = new Buffer<uint>(window, name + ".ibo", Gl.BufferUsage.StaticDraw, indices);
    }
    
    public Window Window { get; }
    public string Name { get; }
    public Buffer<TVertex> VertexBuffer { get; }
    public Buffer<TInstance> InstanceBuffer { get; }
    public Buffer<uint> ElementBuffer { get; }

    public void BindToShader(Shader shader)
    {
        VertexArray vao = new VertexArray(Window, Name + ".vao", shader, ElementBuffer);
        _vaos.Add(vao);
        AddAttributes(VertexBuffer, 0);
        AddAttributes(InstanceBuffer, 1);
        
        void AddAttributes<T>(IBuffer<T> buffer, uint divisor)
            where T : unmanaged
        {
            BufferBinding binding = vao.BindBuffer(buffer);

            foreach (FieldInfo field in typeof(T).GetRuntimeFields())
            {
                if (!_csTypesToAttributes.TryGetValue(field.FieldType, out (int count, Type type) tuple))
                {
                    throw new Exception(
                        $"Field of type {field.FieldType.Name} is not supported on vertex or instance fields.");
                }
                binding.AddAttribute(field.Name, tuple.count, divisor);
            }
        }
    }
    
    public void Dispose()
    {
        ElementBuffer.Dispose();
        InstanceBuffer.Dispose();
        VertexBuffer.Dispose();
        foreach (VertexArray vao in _vaos)
        {
            vao.Dispose();
        }
    }
}

public sealed class Mesh : Mesh<Vertex3D, Instance3D>
{
    private record VertexIndex(int PosIndex, int TexCoordIndex, int NormalIndex)
    {
        public override int GetHashCode()
        {
            return PosIndex * 2 + TexCoordIndex * 3 + NormalIndex * 5;
        }
    }
    
    private static Mesh LoadObjFile(Window window, string path)
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
        return new Mesh(window, Path.GetFileNameWithoutExtension(path) + ".mesh", vertices.ToArray(), indices);
    }

    public Mesh(Window window, string name, Vertex3D[] vertices, uint[] indices) : base(window, name, vertices, indices)
    {
    }
}
