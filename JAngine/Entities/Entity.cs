using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JAngine.Entities;


public sealed class Entity : IEnumerable<ComponentBase>
{
    private readonly Dictionary<Type, ComponentBase> _components;
    private Level? _level;

    public Entity(params ComponentBase[] components)
    {
        _components = components.ToDictionary(c => c.GetType(), c => c);
    }

    internal void OnAddToLevel(Level level)
    {
        _level = level;
    }

    internal bool TryAddComponent(ComponentBase component)
    {
        Type type = component.Type;
        if (_components.ContainsKey(type))
        {
            return false;
        }
        _components.Add(type, component);
        return true;
    }

    internal bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
    {
        if (_components.TryGetValue(typeof(T), out ComponentBase? compBase))
        {
            if (compBase is T compType)
            {
                component = compType;
                return true;
            }
        }
        component = default;
        return false;
    }

    public bool TryGetComponent(Type type, [NotNullWhen(true)] out ComponentBase? component)
    {
        if (_components.TryGetValue(type, out var comp))
        {
            component = comp;
            return true;
        }
        component = null;
        return false;
    }

    public Entity CreateClone(PropertyCollection? overrideProperties = null)
    {
        ComponentBase[] components = _components
            .Select(kvp => kvp.Value.Clone(overrideProperties))
            .ToArray();

        Entity entity = new Entity(components);
        _level?.Entities.AddEntity(entity);
        return entity;
    }

    public IEnumerator<ComponentBase> GetEnumerator()
    {
        return _components
            .Select(kvp => kvp.Value)
            .Where(c => c is not null)
            .Cast<ComponentBase>()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}