using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vertex3D
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Vector2 TextureCoordinate;

        public Vertex3D(float x, float y, float z, float normX, float normY, float normZ, float u, float v) :
            this(new Vector3(x, y, z), new Vector3(normX, normY, normZ), new Vector2(u, v))
        { }

        public Vertex3D(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }


        private static VertexArray.Attribute[]? _attributeLayout;
        
        public static VertexArray.Attribute[] GetAttributeLayout
        {
            get
            {
                if (_attributeLayout == null)
                {
                    unsafe
                    {
                        _attributeLayout = new VertexArray.Attribute[]
                        {
                            (0, 3, VertexAttribType.Float, 0),
                            (1, 3, VertexAttribType.Float, sizeof(Vector3)),
                            (2, 2, VertexAttribType.Float, sizeof(Vector3) * 2)
                        };
                    }
                }
                return _attributeLayout;
            }
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ColoredVertex3D
    {
        public readonly Vector3 Position;
        public readonly Vector3 Normal;
        public readonly Color4 Color;

        public ColoredVertex3D(float x, float y, float z, float normX, float normY, float normZ, float r, float g, float b, float a) :
            this(new Vector3(x, y, z), new Vector3(normX, normY, normZ), new Color4(r, g, b, a))
        { }
        public ColoredVertex3D(Vector3 position, Vector3 normal, Color4 color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }


        private static VertexArray.Attribute[]? _attributeLayout;
        
        public static VertexArray.Attribute[] GetAttributeLayout
        {
            get
            {
                if (_attributeLayout == null)
                {
                    unsafe
                    {
                        _attributeLayout = new VertexArray.Attribute[]
                        {
                            (0, 3, VertexAttribType.Float, 0),
                            (1, 3, VertexAttribType.Float, sizeof(Vector3)),
                            (2, 4, VertexAttribType.Float, sizeof(Vector3) * 2)
                        };
                    }
                }
                return _attributeLayout;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vertex2D
    {
        public readonly Vector2 Position;
        public readonly Vector2 TextureCoordinate;

        public Vertex2D(float x, float y, float u, float v) :
            this(new Vector2(x, y), new Vector2(u, v))
        { }
        public Vertex2D(Vector2 position, Vector2 textureCoordinate)
        {
            Position = position;
            TextureCoordinate = textureCoordinate;
        }

        private static VertexArray.Attribute[]? _attributeLayout;
        
        public static VertexArray.Attribute[] GetAttributeLayout
        {
            get
            {
                if (_attributeLayout == null)
                {
                    unsafe
                    {
                        _attributeLayout = new VertexArray.Attribute[]
                        {
                            (0, 2, VertexAttribType.Float, 0),
                            (1, 2, VertexAttribType.Float, sizeof(Vector2))
                        };
                    }
                }
                return _attributeLayout;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ColoredVertex2D
    {
        public readonly Vector2 Position;
        public readonly Color4 Color;

        public ColoredVertex2D(float x, float y, float r, float g, float b, float a) :
            this(new Vector2(x, y), new Color4(r, g, b, a))
        { }
        public ColoredVertex2D(Vector2 position, Color4 color)
        {
            Position = position;
            Color = color;
        }

        private static VertexArray.Attribute[]? _attributeLayout;
        
        public static VertexArray.Attribute[] GetAttributeLayout
        {
            get
            {
                if (_attributeLayout == null)
                {
                    unsafe
                    {
                        _attributeLayout = new VertexArray.Attribute[]
                        {
                            (0, 2, VertexAttribType.Float, 0),
                            (1, 4, VertexAttribType.Float, sizeof(Vector2))
                        };
                    }
                }
                return _attributeLayout;
            }
        }
    }
}