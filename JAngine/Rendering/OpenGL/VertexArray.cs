namespace JAngine.Rendering.OpenGL;

public sealed class VertexArray : IGlObject, IDisposable
{
    private readonly Window _window;
    private uint _handle;
    private readonly IBuffer<uint> _ebo;
    private readonly Dictionary<IGlObject, BufferBinding> _attributeBuffers = new();
    
    public VertexArray(Window window, string name, Shader shader, IBuffer<uint> ebo)
    {
        _window = window;
        Name = name;
        _ebo = ebo;
        Shader = shader;
        _window.QueueUpdate(this, CreateEvent.Singleton);
        _window.AttachVao(this);
    }

    public string Name { get; }
    public Shader Shader { get; }
    Window IGlObject.Window => _window;
    uint IGlObject.Handle => _handle;
    internal int PointCount => _ebo.Count;
    public int InstanceCount { get; set; }

    public BufferBinding BindBuffer<T>(IBuffer<T> buffer, int offset = 0)
        where T : unmanaged
    {
        lock (_attributeBuffers)
        {
            if (!_attributeBuffers.TryGetValue(buffer, out BufferBinding? binding))
            {
                binding = new BufferBinding(buffer, (uint)_attributeBuffers.Count, offset);
                binding.OnChange += () => UpdateBinding(binding);
                _attributeBuffers.Add(buffer, binding);
            }
            UpdateBinding(binding);

            return binding;
        }
    }

    private void UpdateBinding(BufferBinding binding)
    {
        _window.QueueUpdateUnique(this, binding);
    }
    
    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                _handle = Gl.CreateVertexArray();
                Gl.ObjectLabel(Gl.ObjectIdentifier.VertexArray, _handle, Name);
                Gl.VertexArrayElementBuffer(_handle, _ebo.Handle);
                break;
            case BufferBinding binding:
                uint relativeOffset = 0;
                foreach (BufferBinding.Attribute attribute in binding.GetAttributes())
                {
                    if (!Shader.TryGetAttribute(attribute.Name, out Shader.Attribute? shaderAttrib))
                    {
                        relativeOffset += (uint)attribute.Count * sizeof(float);
                        continue;
                    }

                    uint location = (uint)shaderAttrib.Location;
                    Gl.EnableVertexArrayAttrib(_handle, location);
                    Gl.VertexArrayAttribBinding(_handle, location, binding.Index);
                    Gl.VertexArrayBindingDivisor(_handle, binding.Index, attribute.Divisor);
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
        _window.DetachVao(this);
    }
}
