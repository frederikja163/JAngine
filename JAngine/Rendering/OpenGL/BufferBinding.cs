namespace JAngine.Rendering.OpenGL;

public sealed class BufferBinding : IGlEvent
{
    // TODO: Consider merging this type with Shader.Attribute
    internal sealed record Attribute(string Name, int Count, Gl.VertexAttribType Type, uint Divisor);
    private readonly List<Attribute> _attributes = new List<Attribute>();

    internal BufferBinding(IGlObject buffer, uint index, int offset)
    {
        Buffer = buffer;
        Index = index;
        Offset = offset;
    }

    private Action? _onAction;
    internal event Action? OnChange
    {
        add
        {
            _onAction += value;
        }
        remove
        {
            _onAction -= value;
        }
    }

    public BufferBinding AddAttribute(string attributeName, int count, uint divisor = 0)
    {
        lock (_attributes)
        {
            _attributes.Add(new Attribute(attributeName, count, Gl.VertexAttribType.Float, divisor));
        }
        _onAction?.Invoke();
        return this;
    }

    internal IEnumerable<Attribute> GetAttributes()
    {
        lock (_attributes)
        {
            foreach (Attribute attribute in _attributes)
            {
                yield return attribute;
            }
        }
    }
    internal int Offset { get; }
    internal IGlObject Buffer { get; }
    internal uint Index { get; }
}
