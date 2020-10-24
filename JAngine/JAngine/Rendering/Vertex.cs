using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct Vertex3D
    {
        private readonly Vector3 _position;
        private readonly Vector3 _normal;
        private readonly Vector2 _textureCoordinate;

        public Vertex3D(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            _position = position;
            _normal = normal;
            _textureCoordinate = textureCoordinate;
        }

        public Vector3 Position => _position;

        public Vector3 Normal => _normal;

        public Vector2 TextureCoordinate => _textureCoordinate;

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
    public readonly ref struct Vertex2D
    {
        private readonly Vector2 _position;
        private readonly Vector2 _textureCoordinate;

        public Vertex2D(Vector2 position, Vector2 textureCoordinate)
        {
            _position = position;
            _textureCoordinate = textureCoordinate;
        }

        public Vector2 Position => _position;

        public Vector2 TextureCoordinate => _textureCoordinate;

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
}