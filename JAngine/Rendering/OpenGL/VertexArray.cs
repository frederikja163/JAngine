using System.Drawing;

namespace JAngine.Rendering.OpenGL;

public sealed class VertexArrayVertexBufferBinding : IGlEvent
{
    private readonly VertexArray _vao;
    private readonly List<VertexArray.Attribute> _attributes = new List<VertexArray.Attribute>();

    internal VertexArrayVertexBufferBinding(VertexArray vao, IGlObject buffer, uint index, int offset)
    {
        _vao = vao;
        Buffer = buffer;
        Index = index;
        Offset = offset;
    }

    public VertexArrayVertexBufferBinding AddAttribute(string attributeName, int count)
    {
        lock (_attributes)
        {
            _attributes.Add(new VertexArray.Attribute(attributeName, count, Gl.VertexAttribType.Float));
        }
        _vao.UpdateBinding(this);
        return this;
    }

    internal IEnumerable<VertexArray.Attribute> GetAttributes()
    {
        lock (_attributes)
        {
            foreach (VertexArray.Attribute attribute in _attributes)
            {
                yield return attribute;
            }
        }
    }
    internal int Offset { get; }
    internal IGlObject Buffer { get; }
    internal uint Index { get; }
}


public sealed class VertexArray : IGlObject, IDisposable
{
    internal sealed record Attribute(string Name, int Count, Gl.VertexAttribType Type);
    private record AttributeUpdateEvent (IGlObject Buffer, uint AttribIndex, int Stride, int Size, Gl.VertexAttribType Type) : IGlEvent;
    
    private readonly Window _window;
    private uint _handle;
    private readonly IBuffer<uint> _ebo;
    private readonly Shader _shader;
    private readonly Dictionary<IGlObject, VertexArrayVertexBufferBinding> _attributeBuffers = new();
    
    public VertexArray(Window window, Shader shader, IBuffer<uint> ebo)
    {
        _ebo = ebo;
        _shader = shader;
        _window = window;
        _window.QueueUpdate(this, CreateEvent.Singleton);
    }

    public VertexArrayVertexBufferBinding BindBuffer<T>(IBuffer<T> buffer, int offset = 0)
        where T : unmanaged
    {
        lock (_attributeBuffers)
        {
            if (!_attributeBuffers.TryGetValue(buffer, out VertexArrayVertexBufferBinding? binding))
            {
                binding = new VertexArrayVertexBufferBinding(this, buffer, (uint)_attributeBuffers.Count, offset);
                _attributeBuffers.Add(buffer, binding);
            }
            _window.QueueUpdateUnique(this, binding);

            return binding;
        }
    }

    internal void UpdateBinding(VertexArrayVertexBufferBinding binding)
    {
        _window.QueueUpdateUnique(this, binding);
    }
    
    public void AddAttribute<T>(IBuffer<T> buffer, uint attribIndex, int stride, int size)
        where T : unmanaged
    {
        AddAttribute(buffer, attribIndex, stride, size, Gl.VertexAttribType.Float);
    }

    private void AddAttribute<T>(IBuffer<T> fixedBuffer, uint attribIndex, int stride, int size, Gl.VertexAttribType type)
        where T : unmanaged
    {
        _window.QueueUpdate(this, new AttributeUpdateEvent(fixedBuffer, attribIndex, stride, size, type));
    }

    Window IGlObject.Window => _window;
    uint IGlObject.Handle => _handle;
    
    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                _handle = Gl.CreateVertexArray();
                Gl.VertexArrayElementBuffer(_handle, _ebo.Handle);
                break;
            case VertexArrayVertexBufferBinding binding:
                uint relativeOffset = 0;
                foreach (Attribute attribute in binding.GetAttributes())
                {
                    uint location = (uint)_shader.GetAttribute(attribute.Name).Location;
                    Gl.EnableVertexArrayAttrib(_handle, location);
                    Gl.VertexArrayAttribBinding(_handle, location, binding.Index);
                    Gl.VertexArrayAttribFormat(_handle, location, attribute.Count, attribute.Type, false, relativeOffset);
                    relativeOffset += (uint)attribute.Count * sizeof(float);
                }
                Gl.VertexArrayVertexBuffer(_handle, binding.Index, binding.Buffer.Handle, binding.Offset, (int)relativeOffset);
                break;
            case DisposeEvent:
                Gl.DeleteVertexArray(_handle);
                _handle = 0;
                break;
        }
    }

    internal void Bind()
    {
        Gl.BindVertexArray(_handle);
    }

    public void Dispose()
    {
        _window.QueueUpdate(this, DisposeEvent.Singleton);
    }
}
