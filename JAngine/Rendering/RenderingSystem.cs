using JAngine.Entities;
using JAngine.OpenGL;

namespace JAngine.Rendering;

internal sealed class RenderingSystem : ISystem
{
    private record Render(Material Material, Mesh Mesh);

    private readonly List<Entity> _entities = new List<Entity>();
    private readonly Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();
    private readonly Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
    private readonly Dictionary<string, Material> _materials = new Dictionary<string, Material>();
    private readonly Dictionary<RenderingComponent, Instance> _instances = new Dictionary<RenderingComponent, Instance>();

    public RenderingSystem(Level level)
    {
        foreach (var entity in level.Entities)
        {
            OnEntityAdded(entity);
        }
        level.Entities.OnEntityAdded += OnEntityAdded;
    }

    private void OnEntityAdded(Entity entity)
    {
        if (!entity.TryGetComponent(out RenderingComponent? rendering))
        {
            return;
        }
        _entities.Add(entity);
        if (!_meshes.TryGetValue(rendering.MeshPath, out Mesh? mesh))
        {
            mesh = new Mesh(rendering.MeshPath);
            _meshes.Add(rendering.MeshPath, mesh);
        }
        if (!_textures.TryGetValue(rendering.TexturePath, out Texture? texture))
        {
            texture = new Texture(rendering.TexturePath);
            _textures.Add(rendering.TexturePath, texture);
        }
        if (!_materials.TryGetValue(rendering.MaterialPath, out Material? material))
        {
            material = new Material(rendering.MaterialPath);
            _materials.Add(rendering.MaterialPath, material);
        }
        _instances.Add(rendering, _meshes[rendering.MeshPath].AddInstance());
        Renderer.AddToRender(mesh, material);
    }

    public void Update(float dt)
    {

    }
}
