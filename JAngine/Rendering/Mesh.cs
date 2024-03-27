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
    private readonly List<VertexArray> _vaos;
    private readonly Dictionary<Type, (IBuffer buffer, int divisor)> _buffers;
    private readonly IBuffer<uint> _ebo;
    
    public Mesh(Window window, string name, IBuffer<uint> indexBuffer)
    {
        Window = window;
        Name = name;
        _vaos = new List<VertexArray>();
        _ebo = indexBuffer;
        _buffers = new Dictionary<Type, (IBuffer buffer, int divisor)>(TypeComparer.Default);
    }
    
    public Window Window { get; }
    public string Name { get; }

    public void AddAttribute<T>(int divisor = 0, params T[] data) where T : unmanaged
    {
        if (_buffers.TryGetValue(typeof(T), out (IBuffer buffer, int divisor) tuple))
        {
            throw new Exception($"Mesh {Name} already contains an attribute for {typeof(T).FullName}");
        }
        tuple.buffer = new Buffer<T>(Window, $"{Name}.buffer");
        tuple.divisor = divisor;
        _buffers.Add(typeof(T), tuple);
        
        foreach (VertexArray vao in _vaos)
        {
            AddAttributes(vao, tuple.buffer, tuple.divisor);
        }
    }
    
    public void BindToShader(Shader shader)
    {
        VertexArray vao = new VertexArray(Window, Name + ".vao", shader, _ebo);
        _vaos.Add(vao);

        foreach ((IBuffer buffer, int divisor) in _buffers.Values)
        {
            AddAttributes(vao, buffer, divisor);
        }
    }

    private static void AddAttributes(VertexArray vao, IBuffer buffer, int divisor)
    {
        BufferBinding binding = vao.BindBuffer(buffer);

        foreach (FieldInfo field in buffer.UnderlyingType.GetRuntimeFields())
        {
            if (!Mesh.CsTypesToAttributes.TryGetValue(field.FieldType,
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
        foreach ((IBuffer buffer, _) in _buffers.Values)
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

