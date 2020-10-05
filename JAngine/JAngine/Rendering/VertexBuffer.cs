using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class VertexBuffer : Buffer<float>
    {
        protected override BufferTarget Target => BufferTarget.ArrayBuffer;
        
        public VertexBuffer(float[] data) : base(data)
        {
        }
    }
}