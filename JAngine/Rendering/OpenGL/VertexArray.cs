using System.Drawing;

namespace JAngine.Rendering.OpenGL;


public sealed class VertexArray : IGlObject, IDisposable
{
    private record AttributeUpdateEvent (IGlObject Buffer, uint AttribIndex, int Stride, int Size, Gl.VertexAttribType Type) : IGlEvent;
    
    private readonly Window _window;
    private uint _handle;
    private readonly IBuffer<uint> _ebo;
    private readonly Dictionary<IGlObject, uint> _attributeBuffers = new();
    
    public VertexArray(Window window, IBuffer<uint> ebo)
    {
        _ebo = ebo;
        _window = window;
        _window.QueueUpdate(this, CreateEvent.Singleton);
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
            case AttributeUpdateEvent attrib:
                if (!_attributeBuffers.TryGetValue(attrib.Buffer, out uint bindingIndex))
                {
                    bindingIndex = (uint)_attributeBuffers.Count;
                    _attributeBuffers.Add(attrib.Buffer, bindingIndex);
                    Gl.VertexArrayVertexBuffer(_handle, bindingIndex, attrib.Buffer.Handle, IntPtr.Zero, attrib.Stride);
                }
                Gl.EnableVertexArrayAttrib(_handle, attrib.AttribIndex);
                Gl.VertexArrayAttribBinding(_handle, attrib.AttribIndex, bindingIndex);
                Gl.VertexArrayAttribFormat(_handle, attrib.AttribIndex, attrib.Size, attrib.Type, false, 0);
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
