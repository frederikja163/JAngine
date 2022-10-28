using OpenTK.Mathematics;

namespace JAngine.Rendering;

public sealed class Instance
{
    private readonly Mesh _mesh;
    private InstanceData _data;
    private readonly int _instanceId;


    internal Instance(Mesh mesh, InstanceData data, int instanceId)
    {
        _mesh = mesh;
        _data = data;
        _instanceId = instanceId;
        _position = data.Transform.ExtractTranslation();
        _rotation = data.Transform.ExtractRotation();
        _scale = data.Transform.ExtractScale();
    }

    private void UpdateTransform()
    {
        Matrix4 transform = Matrix4.CreateTranslation(_position) * Matrix4.CreateFromQuaternion(_rotation) * Matrix4.CreateScale(_scale);
        _data = new InstanceData(transform);
        Update();
    }

    private void Update()
    {
        _mesh.Update(_instanceId, _data);
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateTransform();
        }
    }
    private Quaternion _rotation;
    public Quaternion Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            UpdateTransform();
        }
    }
    private Vector3 _scale;
    public Vector3 Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            UpdateTransform();
        }
    }

    public Vector3 EulerAngles
    {
        get => _rotation.ToEulerAngles();
        set
        {
            Rotation = Quaternion.FromEulerAngles(value);
        }
    }

    public Vector4 AxisAngle
    {
        get => _rotation.ToAxisAngle();
        set => Rotation = Quaternion.FromAxisAngle(value.Xyz, value.W);
    }
}
