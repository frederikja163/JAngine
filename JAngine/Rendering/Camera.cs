using OpenTK.Mathematics;

namespace JAngine.Rendering;

public sealed class Camera3D
{
    public static Camera3D? Main { get; set; }

    public Matrix4 ViewMatrix { get; private set; }
    public Matrix4 ProjectionMatrix { get; private set; }

    public Camera3D(float aspectRatio, Vector3 position)
    {
        _aspectRatio = aspectRatio;
        _position = position;
        UpdateMatrices();
    }

    private float _fov = MathHelper.DegreesToRadians(90);
    public float Fov
    {
        get => _fov;
        set
        {
            _fov = value;
            UpdateMatrices();
        }
    }

    private float _aspectRatio;
    public float AspectRatio
    {
        get => _aspectRatio;
        set
        {
            _aspectRatio = value;
            UpdateMatrices();
        }
    }

    private float _nearPlane = 0.001f;
    public float NearPlane
    {
        get => _nearPlane;
        set
        {
            _nearPlane = value;
            UpdateMatrices();
        }
    }

    private float _farPlane = 1000f;
    public float FarPlane
    {
        get => _farPlane;
        set
        {
            _farPlane = value;
            UpdateMatrices();
        }
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            UpdateMatrices();
        }
    }

    public Vector3 Forward
    {
        get
        {
            return _target - Position;
        }
        set
        {
            _target = Position - value;
            UpdateMatrices();
        }
    }

    private Vector3 _target = Vector3.Zero;
    public Vector3 Target
    {
        get => _target;
        set
        {
            _target = value;
            UpdateMatrices();
        }
    }

    private Vector3 _up = Vector3.UnitY;
    public Vector3 Up
    {
        get => _up;
        set
        {
            _up = value;
            UpdateMatrices();
        }
    }

    private void UpdateMatrices()
    {
        ViewMatrix = Matrix4.LookAt(Position, Target, Up);
        ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Fov, AspectRatio, NearPlane, FarPlane);
    }
}
