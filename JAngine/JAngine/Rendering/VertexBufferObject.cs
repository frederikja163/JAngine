using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public sealed class VertexBufferObject : BufferObject<float>
    {
        protected override BufferTarget Target => BufferTarget.ArrayBuffer;
        
        public VertexBufferObject(float[] data, VertexLayout layout) : base(data)
        {
            Bind();
            int offSet = 0;
            for (int i = 0; i < layout.Count; i++)
            {
                var attr = layout[i];
                GL.VertexAttribPointer(i, attr.Count, attr.Type, false, layout.Stride, offSet);
                GL.EnableVertexAttribArray(i);
                offSet += attr.Count * attr.Size;
            }
            Unbind();
        }
    }
}