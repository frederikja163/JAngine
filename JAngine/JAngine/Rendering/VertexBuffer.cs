using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class VertexBuffer<T> : Buffer<T>
        where T : unmanaged
    {
        protected override BufferTarget Target => BufferTarget.ArrayBuffer;
        
        public VertexBuffer(params T[] data) : base(data)
        {
        }
    }
}