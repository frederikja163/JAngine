using OpenTK.Graphics.OpenGL4;

namespace JAngine
{
    public class VertexBufferObject : BufferObject<float>
    {
        protected override BufferTarget Target => BufferTarget.ArrayBuffer;
        
        public VertexBufferObject(float[] data) : base(data)
        {
        }

        public void SetAttributes()
        {
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
        }
    }
}