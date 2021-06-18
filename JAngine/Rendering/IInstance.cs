using System.ComponentModel;
using System.Runtime.InteropServices;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;

namespace JAngine.Rendering
{
    public interface IInstance
    {

        public VertexArray.Attribute[] Attributes { get; }
    }

    public struct BaseInstance : IInstance
    {
        private static readonly VertexArray.Attribute[] AttributesField = new VertexArray.Attribute[0];

        public VertexArray.Attribute[] Attributes => AttributesField;
    }

    public struct TransformInstance : IInstance
    {
        public readonly Matrix4 Matrix;

        public TransformInstance(Vector2 position)
        {
            Matrix = Matrix4.CreateTranslation(position.X, position.Y, 0);
        }

        public TransformInstance(float x, float y)
        {
            Matrix = Matrix4.CreateTranslation(x, y, 0) * Matrix4.CreateScale(100);
        }

        public TransformInstance(Matrix4 matrix)
        {
            Matrix = matrix;
        }
        
        public TransformInstance(Transform transform)
        {
            Matrix = transform.GetMatrix();
        }

        private static readonly VertexArray.Attribute[] AttributesField = new []
        {
            new VertexArray.Attribute(2, 4, VertexAttribType.Float),
            new VertexArray.Attribute(3, 4, VertexAttribType.Float),
            new VertexArray.Attribute(4, 4, VertexAttribType.Float),
            new VertexArray.Attribute(5, 4, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
}