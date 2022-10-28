using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JAngine.Entities;

public sealed class EntityCollection : IEnumerable<Entity>
{
    private readonly Dictionary<string, Entity> _prototypes;
    private readonly List<Entity> _entities;
    private readonly Dictionary<Type, List<Entity>> _entitiesByComponentTypes;

    public event Action<Entity>? OnEntityAdded;

    internal EntityCollection()
    {
        _prototypes = new Dictionary<string, Entity>();
        _entities = new List<Entity>();
        _entitiesByComponentTypes = new Dictionary<Type, List<Entity>>();
    }

    public bool TryGetPrototype(string name, [NotNullWhen(true)] out Entity? entity)
    {
        return _prototypes.TryGetValue(name, out entity);
    }

    public List<Entity> GetEntities<TComponent>()
        where TComponent : ComponentBase
    {
        Type type = typeof(TComponent);
        if (!_entitiesByComponentTypes.TryGetValue(type, out List<Entity>? entities))
        {
            entities = new List<Entity>();
            _entitiesByComponentTypes[type] = entities;
        }
        return entities;
    }

    public bool TryAddEntity(string name, Entity entity)
    {
        if (string.IsNullOrEmpty(name))
        {
            AddEntity(entity);
            return true;
        }

        if (_prototypes.TryAdd(name, entity))
        {
            AddEntityComponents(entity);
            return true;
        }
        return false;
    }

    public void AddEntity(Entity entity)
    {
        AddEntityComponents(entity);
        OnEntityAdded?.Invoke(entity);
        _entities.Add(entity);
    }

    private void AddEntityComponents(Entity entity)
    {
        foreach (var component in entity)
        {
            Type type = component.GetType();
            if (!_entitiesByComponentTypes.TryGetValue(type, out List<Entity>? entities))
            {
                entities = new List<Entity> { entity };
                _entitiesByComponentTypes[type] = entities;
            }
            else
            {
                entities.Add(entity);
            }
        }
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        return _entities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
