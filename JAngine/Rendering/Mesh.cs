using System.Globalization;
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

public class Instance<T>
    where T : unmanaged
{
    private readonly Buffer<T> _buffer;
    private readonly int _index;

    public Instance(Buffer<T> buffer, T data = default)
    {
        _buffer = buffer;
        _index = buffer.Count;
        buffer.SetSubData(_index, data);
    }

    public T Data
    {
        get
        {
            return _buffer[_index];
        }
        
        set
        {
            _buffer[_index] = value;
        }
    }
}

internal static class Mesh
{
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

public class Mesh<TVertex, TInstance> : IDisposable
    where TVertex : unmanaged
    where TInstance : unmanaged
{

    private readonly List<VertexArray> _vaos = new List<VertexArray>();

    public Mesh(Window window, string name, TVertex[] vertices, uint[] indices)
    {
        Window = window;
        Name = name;
        VertexBuffer = new Buffer<TVertex>(window, name + ".vbo", Gl.BufferUsage.StaticDraw, vertices);
        InstanceBuffer = new Buffer<TInstance>(window, name + ".ivbo", Gl.BufferUsage.StaticDraw, 0);
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
        vao.InstanceCount = InstanceBuffer.Count;
        
        void AddAttributes<T>(IBuffer<T> buffer, uint divisor)
            where T : unmanaged
        {
            BufferBinding binding = vao.BindBuffer(buffer);

            foreach (FieldInfo field in typeof(T).GetRuntimeFields())
            {
                if (!Mesh.CsTypesToAttributes.TryGetValue(field.FieldType, out (int attributeCount, int count, Type type) tuple))
                {
                    throw new Exception(
                        $"Field of type {field.FieldType.Name} is not supported on vertex or instance fields.");
                }

                ShaderAttributeAttribute? attribute = field.GetCustomAttributes<ShaderAttributeAttribute>().FirstOrDefault();
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
    }

    public Instance<TInstance> AddInstance(TInstance data = default)
    {
        foreach (VertexArray vao in _vaos)
        {
            vao.InstanceCount += 1;
        }
        return new Instance<TInstance>(InstanceBuffer, data);
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

