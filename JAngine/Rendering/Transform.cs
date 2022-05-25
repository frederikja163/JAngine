using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace JAngine.Rendering
{
    public struct Transform : IVertex
    {
        private class Data
        {
            public ShapeDefinition<Transform> Shape { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Scale { get; set; }
            public float Rotation { get; set; }

            public Data(ShapeDefinition<Transform> shape, Vector2 position, Vector2 scale, float rotation)
            {
                Shape = shape;
                Position = position;
                Scale = scale;
                Rotation = rotation;
            }
        }
        private static readonly Dictionary<int, Data> _data = new ();

        private Data GetData()
        {
            return _data[_index];
        }

        private Matrix4 _matrix;
        private int _index;

        public Transform(ShapeDefinition<Transform> shapeDefinition, float x, float y)
            : this(shapeDefinition, new Vector2(x, y), Vector2.One, 0)
        {
        }

        public Transform(ShapeDefinition<Transform> shapeDefinition, Vector2 position)
            : this(shapeDefinition, position, Vector2.One, 0)
        {
        }

        public Transform(ShapeDefinition<Transform> shapeDefinition, Vector2 position, Vector2 scale, float rotation = 0)
        {
            Data data = new(shapeDefinition, position, scale, rotation);
            _matrix = CreateMatrix(data);

            _index = 0;
            _index = shapeDefinition.Add(this);
            _data[_index] = data;
        }

        public Vector2 Position
        {
            get => GetData().Position;
            set
            {
                Data data = GetData();
                data.Position = value;
                UpdateMatrix(data);
            }
        }

        public Vector2 Scale
        {
            get => GetData().Scale;
            set
            {
                Data data = GetData();
                data.Scale = value;
                UpdateMatrix(data);
            }
        }

        public float Rotation
        {
            get => GetData().Rotation;
            set
            {
                Data data = GetData();
                data.Rotation = value;
                UpdateMatrix(data);
            }
        }

        private void UpdateMatrix(Data data)
        {
            _matrix = CreateMatrix(data);
            data.Shape.Update(_index, this);
        }

        private static Matrix4 CreateMatrix(Data data)
        {
            return Matrix4.CreateScale(data.Scale.X, data.Scale.Y, 1) *
                Matrix4.CreateRotationZ(data.Rotation) *
                Matrix4.CreateTranslation(data.Position.X, data.Position.Y, 0);
        }

        private static readonly VertexArray.Attribute[] AttributesField = new []
        {
            new VertexArray.Attribute(2, 4, VertexAttribType.Float),
            new VertexArray.Attribute(3, 4, VertexAttribType.Float),
            new VertexArray.Attribute(4, 4, VertexAttribType.Float),
            new VertexArray.Attribute(5, 4, VertexAttribType.Float),
            new VertexArray.Attribute(6, 1, VertexAttribType.Int),
        };
            
        public static VertexArray.Attribute[] Attributes => AttributesField;
    }
    
    //public class TransformInstance
    //{
    //    public TransformInstance(ShapeDefinition<Transform> shapeDefinition, float x, float y)
    //        : this(shapeDefinition, new Vector2(x, y), Vector2.One, 0)
    //    {
    //    }
        
    //    public TransformInstance(ShapeDefinition<Transform> shapeDefinition, Vector2 position)
    //        : this(shapeDefinition, position, Vector2.One, 0)
    //    {
    //    }

    //    public TransformInstance(ShapeDefinition<Transform> shapeDefinition, Vector2 position, Vector2 scale, float rotation = 0)
    //    {
    //        _position = position;
    //        _scale = scale;
    //        _rotation = rotation;
    //    }

    //    private Vector2 _position;
    //    public Vector2 Position
    //    {
    //        get => _position;
    //        set
    //        {
    //            _position = value;
    //            UpdateMatrix();
    //        }
    //    }

    //    private Vector2 _scale;
    //    public Vector2 Scale
    //    {
    //        get => _scale;
    //        set
    //        {
    //            _scale = value;
    //            UpdateMatrix();
    //        }
    //    }

    //    private float _rotation;
    //    public float Rotation
    //    {
    //        get => _rotation;
    //        set
    //        {
    //            _rotation = value;
    //            UpdateMatrix();
    //        }
    //    }

    //    private void UpdateMatrix()
    //    {
    //         CreateData(_rotation, _position, _scale);
    //    }
        
    //    private static Transform CreateData(float rotation, Vector2 position, Vector2 scale)
    //    {
    //        return new Transform(Matrix4.CreateScale(scale.X, scale.Y, 1) *
    //                                 Matrix4.CreateRotationZ(rotation) *
    //                                 Matrix4.CreateTranslation(position.X, position.Y, 0));
    //    }
    //}
}