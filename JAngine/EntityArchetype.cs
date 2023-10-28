using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.ComTypes;

namespace JAngine;

internal sealed class EntityArchetype
{
    private readonly Dictionary<Type, Dictionary<Guid, object>> _componentTypes = new();
    private readonly List<Entity> _entities = new List<Entity>();

    internal EntityArchetype(World world, SortedSet<Type> componentTypes)
    {
        World = world;
        ComponentTypes = componentTypes;
        foreach (Type componentType in componentTypes)
        {
            _componentTypes.Add(componentType, new Dictionary<Guid, object>());
        }
    }
    
    internal World World { get; private init; }
    internal SortedSet<Type> ComponentTypes { get; private init; }

    internal Entity AddEntity(Entity? existingEntity = null, IEnumerable? existingComponents = null)
    {
        if (existingEntity == default)
        {
            existingEntity = new Entity(this, Guid.NewGuid());
        }
        else
        {
            existingEntity.Archetype = this;
        }

        Dictionary<Type, object> existingComponentsByType =
            existingComponents?.OfType<object>().ToDictionary(o => o.GetType(), o => o, TypeComparer.Default) ??
            new Dictionary<Type, object>();
        foreach ((Type componentType, Dictionary<Guid, object> components) in _componentTypes)
        {
            if (!existingComponentsByType.TryGetValue(componentType, out object? component))
            {
                component = Activator.CreateInstance(componentType);
                if (component is null)
                {
                    RemoveEntity(existingEntity);
                    throw new Exception($"Component of type {componentType.FullName} failed to initialize.");
                }
                existingComponentsByType.Add(componentType, component);
            }

            while (!components.TryAdd(existingEntity.Id, component))
            {
                throw new Exception("Duplicate component types or entity guid.");
            }
        }
        _entities.Add(existingEntity);

        return existingEntity;
    }

    internal List<object> RemoveEntity(Entity entity)
    {
        List<object> removedComponents = new List<object>();
        foreach (Dictionary<Guid, object> components in _componentTypes.Values)
        {
            if (components.TryGetValue(entity.Id, out object? component))
            {
                components.Remove(entity.Id);
                removedComponents.Add(component);
            }
        }
        _entities.Remove(entity);
        return removedComponents;
    }

    internal bool TryGetComponent<T>(Entity entity, [NotNullWhen(true)] out T? component)
    {
        if(!_componentTypes.TryGetValue(typeof(T), out Dictionary<Guid, object>? components) ||
           !components.TryGetValue(entity.Id, out object? componentObj))
        {
            component = default;
            return false;
        }

        component = (T)componentObj;
        return true;
    }

    internal IEnumerable<T> GetAllComponentsOfType<T>()
    {
        if(!_componentTypes.TryGetValue(typeof(T), out Dictionary<Guid, object>? components))
        {
            yield break;
        }

        foreach (T component in components.Values.OfType<T>())
        {
            yield return component;
        }
    }

    internal IEnumerable<Entity> GetEntities()
    {
        foreach (Entity entity in _entities)
        {
            yield return entity;
        }
    }
}