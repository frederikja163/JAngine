using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.OpenGL;

public sealed class VertexArray : IDisposable
{
    private readonly Dictionary<Buffer, uint> _vertexBuffers = new Dictionary<Buffer, uint>();
    private Buffer<uint> _indexBuffer;
    public VertexArrayHandle Handle { get; private init; }
    public VertexArray(Buffer<uint> indexBuffer)
    {
        Handle = GL.CreateVertexArray();
        _indexBuffer = indexBuffer;
        GL.VertexArrayElementBuffer(Handle, indexBuffer.Handle);
    }

    public void ReplaceElementBuffer(Buffer<uint> indexBuffer)
    {
        _indexBuffer = indexBuffer;
        GL.VertexArrayElementBuffer(Handle, indexBuffer.Handle);
    }

    public void SetVertexAttributeBuffer(
        Buffer buffer,
        uint attributeIndex,
        int bufferOffset = 0,
        int stride = sizeof(float),
        int count = 1,
        VertexAttribType type = VertexAttribType.Float,
        uint elementOffset = 0,
        uint divisor = 0)
    {
        if (!_vertexBuffers.TryGetValue(buffer, out uint bindingIndex))
        {
            bindingIndex = (uint)_vertexBuffers.Count;
            _vertexBuffers.Add(buffer, bindingIndex);
            GL.VertexArrayVertexBuffer(Handle, bindingIndex, buffer.Handle, (IntPtr)bufferOffset, stride);
        }
        GL.VertexArrayAttribBinding(Handle, attributeIndex, bindingIndex);
        GL.VertexArrayAttribFormat(Handle, attributeIndex, count, type, true, elementOffset);
        GL.EnableVertexArrayAttrib(Handle, attributeIndex);
        GL.VertexArrayBindingDivisor(Handle, bindingIndex, divisor);
    }

    public void ReplaceVertexAttributeBuffer(Buffer oldBuffer, Buffer newBuffer, int newStride, int newOffset = 0)
    {
        uint bindingIndex = _vertexBuffers[oldBuffer];
        _vertexBuffers.Remove(oldBuffer);
        _vertexBuffers.Add(newBuffer, bindingIndex);
        GL.VertexArrayVertexBuffer(Handle, bindingIndex, newBuffer.Handle, (IntPtr)newOffset, newStride);
    }

    public void Bind()
    {
        GL.BindVertexArray(Handle);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(Handle);
    }
}
