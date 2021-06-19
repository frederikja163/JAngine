using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public readonly struct TransformData : IVertex
    {
        public readonly Matrix4 Matrix;

        public TransformData(Matrix4 matrix)
        {
            Matrix = matrix;
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
    
    public class TransformInstance : Instance<TransformData>
    {
        public TransformInstance(ShapeDefinition<TransformData> shapeDefinition, float x, float y)
            : this(shapeDefinition, new Vector2(x, y), Vector2.One, 0)
        {
        }
        
        public TransformInstance(ShapeDefinition<TransformData> shapeDefinition, Vector2 position)
            : this(shapeDefinition, position, Vector2.One, 0)
        {
        }

        public TransformInstance(ShapeDefinition<TransformData> shapeDefinition, Vector2 position, Vector2 scale, float rotation = 0)
            : base(shapeDefinition, CreateData(rotation, position, scale))
        {
            _position = position;
            _scale = scale;
            _rotation = rotation;
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateMatrix();
            }
        }

        private Vector2 _scale;
        public Vector2 Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateMatrix();
            }
        }

        private float _rotation;
        public float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateMatrix();
            }
        }

        private void UpdateMatrix()
        {
            Data = CreateData(_rotation, _position, _scale);
        }
        
        private static TransformData CreateData(float rotation, Vector2 position, Vector2 scale)
        {
            return new TransformData(Matrix4.CreateScale(scale.X, scale.Y, 1) *
                                     Matrix4.CreateRotationZ(rotation) *
                                     Matrix4.CreateTranslation(position.X, position.Y, 0));
        }
    }
}