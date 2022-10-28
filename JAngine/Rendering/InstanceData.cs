using OpenTK.Mathematics;

namespace JAngine.Rendering;

public readonly struct InstanceData
{
    public readonly Matrix4 Transform;

    public InstanceData(Matrix4 transform)
    {
        Transform = transform;
    }
}
