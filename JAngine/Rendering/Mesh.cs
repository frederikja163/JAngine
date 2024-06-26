using System.Numerics;
using System.Reflection;
using JAngine.Rendering.OpenGL;
namespace JAngine.Rendering;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class ShaderAttributeAttribute : Attribute
{
    public string NameInShader { get; }

    public ShaderAttributeAttribute(string nameInShader)
    {
        NameInShader = nameInShader;
    }
}

public sealed class Mesh: IDisposable
{
    private readonly Dictionary<Texture, int> _textures;
    private readonly List<VertexArray> _vaos;
    private readonly Dictionary<Type, IBuffer> _vertexBuffers;
    private readonly Dictionary<Type, IBuffer> _instanceBuffers;
    private readonly Buffer<uint> _ebo;
    
    public Mesh(Window window, string name)
    {
        Window = window;
        Name = name;
        _textures = new Dictionary<Texture, int>();
        _vaos = new List<VertexArray>();
        _ebo = new Buffer<uint>(window, $"{name}.buffer");
        _vertexBuffers = new Dictionary<Type, IBuffer>(TypeComparer.Default);
        _instanceBuffers = new Dictionary<Type, IBuffer>();
    }
    
    public Window Window { get; }
    public string Name { get; }

    public void AddIndices(ReadOnlySpan<uint> indices)
    {
        _ebo.Add(indices);
    }

    public void AddIndex(uint index)
    {
        _ebo.Add(index);
    }

    public IEnumerable<uint> GetIndices()
    {
        return _ebo.ToList();
    }

    public void ClearIndices()
    {
        Span<uint> s = stackalloc uint[_ebo.Count]; 
        s.Fill(0);
        _ebo.SetSubData(0, s);
    }

    private Buffer<T> GetVertexBuffer<T>() where T : unmanaged
    {
        if (!_vertexBuffers.TryGetValue(typeof(T), out IBuffer? buffer))
        {
            buffer = new Buffer<T>(Window, $"{Name}.{typeof(T).Name}.vbuffer");
            _vertexBuffers.Add(typeof(T), buffer);
        
            foreach (VertexArray vao in _vaos)
            {
                AddAttributes(vao, buffer, 0);
            }
        }

        Buffer<T> buf = (Buffer<T>)buffer;
        return buf;
    }

    public BufferDataReference<T> AddVertex<T>(T data)
        where T : unmanaged
    {
        Buffer<T> buf = GetVertexBuffer<T>();
        return new BufferDataReference<T>(buf, buf.Add(data));
    }
    
    public IEnumerable<BufferDataReference<T>> AddVertices<T>(ReadOnlySpan<T> datas)
        where T : unmanaged
    {
        Buffer<T> buf = GetVertexBuffer<T>();
        int index = buf.Add(datas);
        List<BufferDataReference<T>> references = new ();
        foreach (T d in datas)
        {
            references.Add(new BufferDataReference<T>(buf, index++));
        }

        return references;
    }

    public BufferDataReference<T> GetVertex<T>(int index)
        where T : unmanaged
    {
        Buffer<T> buf = GetVertexBuffer<T>();
        return new BufferDataReference<T>(buf, index);
    }
    
    public IEnumerable<BufferDataReference<T>> GetVertices<T>(int start = 0, int count = -1)
        where T : unmanaged
    {
        Buffer<T> buf = GetVertexBuffer<T>();
        if (count == -1)
        {
            count = buf.Count;
        }
        
        for (int i = start; i < start + count; i++)
        {
            yield return new BufferDataReference<T>(buf, i);
        }
    }
    
    private Buffer<T> GetInstanceBuffer<T>() where T : unmanaged
    {
        if (!_instanceBuffers.TryGetValue(typeof(T), out IBuffer? buffer))
        {
            buffer = new Buffer<T>(Window, $"{Name}.{typeof(T).Name}.ibuffer");
            _instanceBuffers.Add(typeof(T), buffer);
        
            foreach (VertexArray vao in _vaos)
            {
                AddAttributes(vao, buffer, 1);
            }
        }

        Buffer<T> buf = (Buffer<T>)buffer;
        return buf;
    }

    private void UpdateInstanceCount()
    {
        int instanceCount = _instanceBuffers.Max(b => b.Value.Count);
        foreach (VertexArray vao in _vaos)
        {
            vao.InstanceCount = instanceCount;
        }
    }
    
    public BufferDataReference<T> AddInstance<T>(T data)
        where T : unmanaged
    {
        Buffer<T> buf = GetInstanceBuffer<T>();
        int index = buf.Add(data);
        UpdateInstanceCount();
        return new BufferDataReference<T>(buf, index);
    }

    public IEnumerable<BufferDataReference<T>> AddInstances<T>(ReadOnlySpan<T> datas)
        where T : unmanaged
    {
        Buffer<T> buf = GetInstanceBuffer<T>();
        int index = buf.Add(datas);
        List<BufferDataReference<T>> references = new ();
        foreach (T d in datas)
        {
            references.Add(new BufferDataReference<T>(buf, index++));
        }
        UpdateInstanceCount();

        return references;
    }

    public BufferDataReference<T> GetInstance<T>(int index)
        where T : unmanaged
    {
        Buffer<T> buf = GetInstanceBuffer<T>();
        return new BufferDataReference<T>(buf, index);
    }
    
    public IEnumerable<BufferDataReference<T>> GetInstances<T>(int start = 0, int count = -1)
        where T : unmanaged
    {
        Buffer<T> buf = GetInstanceBuffer<T>();
        if (count == -1)
        {
            count = buf.Count;
        }
        
        for (int i = start; i < start + count; i++)
        {
            yield return new BufferDataReference<T>(buf, i);
        }
    }

    public int AddTexture(Texture texture)
    {
        if (!_textures.TryGetValue(texture, out int index))
        {
            index = _textures.Count;
            _textures.Add(texture, index);
        }

        foreach (VertexArray vao in _vaos)
        {
            vao.SetTexture(index, texture);
        }

        return index;
    }
    
    public void BindToShader(Shader shader)
    {
        VertexArray vao = new VertexArray(Window, Name + ".vao", shader, _ebo);
        _vaos.Add(vao);

        foreach ((Texture texture, int index) in _textures)
        {
            vao.SetTexture(index, texture);
        }

        foreach (IBuffer buffer in _vertexBuffers.Values)
        {
            AddAttributes(vao, buffer, 0);
        }
        foreach (IBuffer buffer in _instanceBuffers.Values)
        {
            AddAttributes(vao, buffer, 1);
        }
    }

    private static void AddAttributes(VertexArray vao, IBuffer buffer, int divisor)
    {
        BufferBinding binding = vao.BindBuffer(buffer);

        foreach (FieldInfo field in buffer.UnderlyingType.GetRuntimeFields())
        {
            if (!CsTypesToAttributes.TryGetValue(field.FieldType,
                    out (int attributeCount, int count, Type type) tuple))
            {
                throw new Exception(
                    $"Field of type {field.FieldType.Name} is not supported on vertex or instance fields.");
            }

            ShaderAttributeAttribute? attribute = field.GetCustomAttribute<ShaderAttributeAttribute>();
            for (int i = 0; i < tuple.attributeCount; i++)
            {
                if (attribute is not null)
                {
                    binding.AddAttribute(string.Format(attribute.NameInShader, i), tuple.count, divisor);
                }
                else
                {
                    binding.AddAttribute(field.Name, tuple.count, divisor);
                }
            }
        }
    }

    public void Dispose()
    {
        _ebo.Dispose();
        foreach (IBuffer buffer in _vertexBuffers.Values)
        {
            buffer.Dispose();
        }
        foreach (IBuffer buffer in _instanceBuffers.Values)
        {
            buffer.Dispose();
        }
        foreach (VertexArray vao in _vaos)
        {
            vao.Dispose();
        }
    }
    
    // TODO: Convert to function with switch.
    internal static readonly IReadOnlyDictionary<Type, (int attribCount, int count, Type type)> CsTypesToAttributes =
        new Dictionary<Type, (int attribCount, int count, Type type)>()
        {
            {typeof(int), (1, 1, typeof(int))},
            {typeof(uint), (1, 1, typeof(uint))},
            {typeof(float), (1, 1, typeof(float))},
            {typeof(double), (1, 1, typeof(double))},
            
            {typeof(Vector2), (1, 2, typeof(float))},
            {typeof(Vector3), (1, 3, typeof(float))},
            {typeof(Vector4), (1, 4, typeof(float))},
            
            {typeof(Matrix4x4), (4, 4, typeof(float))},
            {typeof(Matrix3x2), (3, 2, typeof(float))},
        };
}

