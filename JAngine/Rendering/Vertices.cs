using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public readonly struct Vertex : IVertex
    {
        public readonly Vector2 Position;
        public readonly Vector2 TextureCoordinate;
        public readonly Color4<Rgba> Color;
        
        public Vertex(Vector2 position, Vector2 textureCoordinate)
            : this(position, textureCoordinate, Color4.White)
        {
        }

        public Vertex(Vector2 position, Vector2 textureCoordinate, Color4<Rgba> color)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
            Color = color;
        }

        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(0, 2, VertexAttribType.Float),
            new VertexArray.Attribute(1, 2, VertexAttribType.Float),
            new VertexArray.Attribute(2, 4, VertexAttribType.Float),
        };

        public static VertexArray.Attribute[] Attributes => AttributesField;
    }
    
    public readonly struct PositionVertex : IVertex
    {
        public readonly Vector2 Position;

        public PositionVertex(Vector2 position)
        {
            Position = position;
        }

        public PositionVertex(float x, float y) : this(new Vector2(x, y))
        {
        }

        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(0, 2, VertexAttribType.Float),
        };

        public static VertexArray.Attribute[] Attributes => AttributesField;
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

        public static VertexArray.Attribute[] Attributes => AttributesField;
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

        public static VertexArray.Attribute[] Attributes => AttributesField;
    }
}