using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public interface IVertex
    {
        public VertexArray.Attribute[] Attributes { get; }
    }
    
    public readonly struct Vertex : IVertex
    {
        public readonly Vector2 Position;

        public Vertex(Vector2 position)
        {
            Position = position;
        }

        public Vertex(float x, float y) : this(new Vector2(x, y))
        {
        }

        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(0, 2, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
    
    public readonly struct ColorVertex : IVertex
    {
        public readonly Vector2 Position;
        public readonly Color4<Rgba> Color;

        public ColorVertex(Vector2 position, Color4<Rgba> color)
        {
            Position = position;
            Color = color;
        }

        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(0, 2, VertexAttribType.Float),
            new VertexArray.Attribute(1, 4, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
    
    public readonly struct TextureVertex : IVertex
    {
        public readonly Vector2 Position;
        public readonly Vector2 TextureCoordinate;

        public TextureVertex(Vector2 position, Vector2 textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }
        
        public TextureVertex(float x, float y)
        {
            Position = new Vector2(x, y);
            TextureCoordinate = Position;
        }

        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(0, 2, VertexAttribType.Float),
            new VertexArray.Attribute(1, 2, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
}