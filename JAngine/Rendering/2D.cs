using System.Numerics;
using JAngine.Extensions;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public readonly struct Vertex2D
{
    [ShaderAttribute("vPosition")]
    public readonly Vector2 Position;
    [ShaderAttribute("vTexCoord")]
    public readonly Vector2 TexCoord;

    public Vertex2D(Vector2 position, Vector2 texCoord)
    {
        Position = position;
        TexCoord = texCoord;
    }
    
    public Vertex2D(float x, float y)
    {
        Position = new Vector2(x, y);
        TexCoord = Position;
    }
}

public readonly struct Instance2D
{
    [ShaderAttribute("vColor")]
    public readonly Vector4 Color = Vector4.One;
    [ShaderAttribute("vTransform{0}")]
    public readonly Matrix4x4 Transformation = Matrix4x4.Identity;
    [ShaderAttribute("vTextureIndex")]
    public readonly int TextureIndex = 0;
    [ShaderAttribute("vTextureOffset")]
    public readonly Vector2 TextureOffset = Vector2.Zero;
    [ShaderAttribute("vTextureSize")]
    public readonly Vector2 TextureSize = Vector2.One;

    public Instance2D(Matrix4x4 transformation, Vector4 color, int textureIndex, Vector2 textureOffset, Vector2 textureSize)
    {
        Transformation = transformation;
        Color = color;
        TextureIndex = textureIndex;
        TextureOffset = textureOffset;
        TextureSize = textureSize;
    }
}

public sealed class Instance2DRef
{
    private readonly BufferDataReference<Instance2D> _dataRef;
    private bool _isSelfUpdating = false;
    private Vector4 _color;
    private Vector3 _position;
    private Vector3 _scale;
    private Quaternion _rotation;
    private int _textureIndex;
    private Vector2 _textureOffset;
    private Vector2 _textureSize;
    private Matrix4x4 _parentMatrix = Matrix4x4.Identity;

    private Instance2DRef(BufferDataReference<Instance2D> dataRef)
    {
        _dataRef = dataRef;
        _dataRef.OnDataUpdated += UpdateReference;
        UpdateReference(_dataRef);
    }

    public static implicit operator Instance2DRef(BufferDataReference<Instance2D> dataRef)
    {
        return new Instance2DRef(dataRef);
    }

    private void UpdateReference(BufferDataReference<Instance2D> reference)
    {
        if (_isSelfUpdating)
        {
            _isSelfUpdating = false;
            return;
        }
        _color = _dataRef.Data.Color;
        _textureIndex = _dataRef.Data.TextureIndex;
        _textureOffset = _dataRef.Data.TextureOffset;
        _textureSize = _dataRef.Data.TextureSize;
        _dataRef.Data.Transformation.TryDecomposeTRS(out _position, out _rotation, out _scale);
    }

    public Instance2D Data
    {
        get => _dataRef.Data;
        set => _dataRef.Data = value;
    }

    public Instance2D Instance2D
    {
        get => _dataRef.Data;
        set => _dataRef.Data = value;
    }

    public int TextureIndex
    {
        get => _textureIndex;
        set
        {
            _textureIndex = value;
            Update();
        }
    }

    public Vector2 TextureOffset
    {
        get => _textureOffset;
        set
        {
            _textureOffset = value;
            Update();
        }
    }

    public Vector2 TextureSize
    {
        get => _textureOffset;
        set
        {
            _textureSize = value;
            Update();
        }
    }

    public Vector4 Color
    {
        get => _color;
        set
        {
            _color = value;
            Update();
        }
    }

    public void SetColorNoUpdate(Vector4 value)
    {
        _color = value;
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            Update();
        }
    }

    public void SetPositionNoUpdate(Vector3 value)
    {
        _position = value;
    }

    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            Update();
        }
    }

    public void SetScaleNoUpdate(Vector3 value)
    {
        Scale = value;
    }

    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            Update();
        }
    }

    public void SetRotationNoUpdate(Quaternion value)
    {
        _rotation = value;
    }

    public Matrix4x4 ParentMatrix
    {
        get => _parentMatrix;
        set
        {
            _parentMatrix = value;
            Update();
        }
    }

    public void SetParentMatrixNoUpdate(Matrix4x4 value)
    {
        _parentMatrix = value;
    }

    public void SetAxisAngle(Vector3 axis, float angle)
    {
        Rotation = Quaternion.CreateFromAxisAngle(axis, angle);
    }

    public void RotateAxisAngle(Vector3 axis, float angle)
    {
        Rotation *= Quaternion.CreateFromAxisAngle(axis, angle);
    }
    
    public void Update()
    {
        _isSelfUpdating = true;
        _dataRef.Data = new Instance2D(
            Matrix4x4.CreateScale(_scale) *
            Matrix4x4.CreateFromQuaternion(_rotation) *
            Matrix4x4.CreateTranslation(_position)
            , _color, _textureIndex, _textureOffset, _textureSize
            );
    }
}
