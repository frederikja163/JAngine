using System.Globalization;
using JAngine.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering;

public unsafe sealed class Mesh : IDisposable
{
    private VertexArray _vertexArray;
    private readonly Buffer<Vertex3D> _vertexBuffer;
    private readonly Buffer<uint> _indexBuffer;
    private Buffer<InstanceData> _instanceBuffer;
    public int InstanceCount { get; private set; }
    private List<(int, InstanceData)> _instancesToUpdate = new List<(int, InstanceData)>();

    private record VertexIndex(int PosIndex, int TexCoordIndex, int NormalIndex)
    {
        public override int GetHashCode()
        {
            return PosIndex << 20 + TexCoordIndex << 10 + NormalIndex;
        }
    }

    public Mesh(string path, int instanceCount = 1)
    {
        using Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new StreamReader(stream);

        List<Vector3> normals = new List<Vector3>();
        List<Vector2> texcoords = new List<Vector2>();
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
                    texcoords.Add(new Vector2(float.Parse(tokens[1]), float.Parse(tokens[2])));
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
                                texcoords[vertexIndex.TexCoordIndex - 1],
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

        _indexBuffer = new Buffer<uint>(indices);
        _vertexBuffer = new Buffer<Vertex3D>(vertices.ToArray());
        _vertexArray = new VertexArray(_indexBuffer);
        _vertexArray.SetVertexAttributeBuffer(_vertexBuffer, 0, stride: sizeof(Vertex3D), count: 3, elementOffset: sizeof(float) * 0);
        _vertexArray.SetVertexAttributeBuffer(_vertexBuffer, 1, stride: sizeof(Vertex3D), count: 2, elementOffset: sizeof(float) * 3);
        _vertexArray.SetVertexAttributeBuffer(_vertexBuffer, 2, stride: sizeof(Vertex3D), count: 3, elementOffset: sizeof(float) * 6);
        _instanceBuffer = new Buffer<InstanceData>(0);
        _vertexArray.SetVertexAttributeBuffer(_instanceBuffer, 3, stride: sizeof(InstanceData), count: 4, elementOffset: sizeof(float) * 0, divisor: 1);
        _vertexArray.SetVertexAttributeBuffer(_instanceBuffer, 4, stride: sizeof(InstanceData), count: 4, elementOffset: sizeof(float) * 4, divisor: 1);
        _vertexArray.SetVertexAttributeBuffer(_instanceBuffer, 5, stride: sizeof(InstanceData), count: 4, elementOffset: sizeof(float) * 8, divisor: 1);
        _vertexArray.SetVertexAttributeBuffer(_instanceBuffer, 6, stride: sizeof(InstanceData), count: 4, elementOffset: sizeof(float) * 12, divisor: 1);

    }

    public int IndexCount => _indexBuffer.Count;

    public Instance AddInstance() => AddInstance(new InstanceData(Matrix4.Identity));

    public Instance AddInstance(InstanceData data)
    {
        int instanceId = InstanceCount;
        InstanceCount += 1;
        // Delete old instance buffer if it doesn't have enough capacity.
        if (_instanceBuffer.Count < InstanceCount)
        {
            // Update all old instances before any other changes to said instance is applied.
            _instancesToUpdate.InsertRange(0, _instanceBuffer.Select((d, i) => (i, d)));
            _instanceBuffer.Dispose();
            Buffer<InstanceData> oldBuffer = _instanceBuffer;
            _instanceBuffer = new Buffer<InstanceData>(InstanceCount * 2);

            _vertexArray.ReplaceVertexAttributeBuffer(oldBuffer, _instanceBuffer, sizeof(InstanceData));
        }
        Update(instanceId, data);
        return new Instance(this, data, instanceId);
    }

    internal void Update(int instanceId, InstanceData data)
    {
        _instancesToUpdate.Add((instanceId, data));
    }

    public void Bind()
    {
        // Update instance buffer before binding.
        if (_instancesToUpdate.Count != 0)
        {
            int rangeStart = 0;
            int lastInstanceId = 0;
            List<InstanceData> range = new List<InstanceData>();
            foreach (var (instanceId, data) in _instancesToUpdate.OrderBy((tuple) => tuple.Item1))
            {
                if (range.Count + rangeStart == instanceId)
                {
                    range.Add(data);
                }
                else if (lastInstanceId == instanceId)
                {
                    range[range.Count - 1] = data;
                }
                else
                {
                    _instanceBuffer.SetRange(rangeStart, range.ToArray());
                    rangeStart = instanceId;
                    range.Clear();
                    range.Add(data);
                }
            }
            _instanceBuffer.SetRange(rangeStart, range.ToArray());
        }

        _vertexArray.Bind();
    }

    public void Dispose()
    {
        _vertexArray.Dispose();
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _instanceBuffer.Dispose();
    }
}
