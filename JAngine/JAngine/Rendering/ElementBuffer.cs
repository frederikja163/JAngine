using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class ElementBuffer : Buffer<uint>
    {
        protected override BufferTarget Target => BufferTarget.ElementArrayBuffer;

        public ElementBuffer(uint[] data) : base(data)
        {
        }
    }
}