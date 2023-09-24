using System.Drawing;

namespace JAngine.Rendering.OpenGL;


internal sealed class VertexArray : IGlObject, IDisposable
{
    private record AttributeUpdateEvent (IGlObject Buffer, uint AttribIndex, int Stride, int Size, Gl.VertexAttribType Type) : IGlEvent;
    
    private readonly Buffer<uint> _ebo;
    private readonly Dictionary<IGlObject, uint> _attributeBuffers = new();
    
    public VertexArray(Window window, Buffer<uint> ebo)
    {
        _ebo = ebo;
        Window = window;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }

    internal void AddAttribute<T>(Buffer<T> buffer, uint attribIndex, int stride, int size, Gl.VertexAttribType type) where T : unmanaged
    {
        Window.QueueUpdate(this, new AttributeUpdateEvent(buffer, attribIndex, stride, size, type));
    }

    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;
    
    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateVertexArray();
                Gl.VertexArrayElementBuffer(Handle, _ebo.Handle);
                break;
            case AttributeUpdateEvent attrib:
                if (!_attributeBuffers.TryGetValue(attrib.Buffer, out uint bindingIndex))
                {
                    bindingIndex = (uint)_attributeBuffers.Count;
                    Gl.VertexArrayVertexBuffer(Handle, bindingIndex, attrib.Buffer.Handle, IntPtr.Zero, attrib.Stride);
                }
                Gl.EnableVertexArrayAttrib(Handle, attrib.AttribIndex);
                Gl.VertexArrayAttribBinding(Handle, attrib.AttribIndex, bindingIndex);
                Gl.VertexArrayAttribFormat(Handle, attrib.AttribIndex, attrib.Size, attrib.Type, false, 0);
                break;
            case DisposeEvent:
                Gl.DeleteVertexArray(Handle);
                Handle = 0;
                break;
        }
    }

    public void Dispose()
    {
        Window.QueueUpdate(this, DisposeEvent.Singleton);
    }
}