using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;

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

    public struct PositionInstance : IInstance
    {
        public readonly Vector2 Position;

        public PositionInstance(Vector2 position)
        {
            Position = position;
        }

        public PositionInstance(float x, float y) : this(new Vector2(x, y))
        {
        }
        
        private static readonly VertexArray.Attribute[] AttributesField = new []
        {
            new VertexArray.Attribute(2, 2, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
}