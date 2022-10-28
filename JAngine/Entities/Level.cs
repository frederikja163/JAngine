using System.Reflection;
using JAngine.Reflection;

namespace JAngine.Entities;

public sealed class Level : IDisposable
{
    private ISystem[] _systems;
    public EntityCollection Entities { get; }


    internal Level()
    {
        Entities = new EntityCollection();
        Entities.OnEntityAdded += OnEntityAdded;
        _systems = Assemblies.GetAllTypes()
            .Where(t => t.IsAssignableTo(typeof(ISystem)))
            .Where(t => t.GetConstructor(BindingFlags.Public | BindingFlags.Instance, new Type[] { typeof(Level) }) is not null)
            .Select(t => Activator.CreateInstance(t, this))
            .Where(s => s is not null)
            .Cast<ISystem>()
            .ToArray();
    }

    private void OnEntityAdded(Entity entity)
    {
        entity.OnAddToLevel(this);
    }

    internal void UpdateAllSystems(float dt)
    {
        foreach (var system in _systems)
        {
            system.Update(dt);
        }
    }

    public void Dispose()
    {
        foreach (var system in _systems)
        {
            if (system is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
