using JAngine.Entities;

namespace JAngine.Rendering;

public sealed class RenderingComponent : ComponentBase
{
    public RenderingComponent(PropertyCollection properties) : base(properties)
    {
    }

    public string MeshPath { get; set; } = string.Empty;
    public string MaterialPath { get; set; } = string.Empty;
    public string TexturePath { get; set; } = string.Empty;
}
