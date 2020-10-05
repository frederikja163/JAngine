using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class ElementBufferObject : BufferObject<uint>
    {
        protected override BufferTarget Target => BufferTarget.ElementArrayBuffer;

        public ElementBufferObject(uint[] data) : base(data)
        {
        }
    }
}