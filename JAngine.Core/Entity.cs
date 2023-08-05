using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace JAngine.Core;

/// <summary>
/// An entity in the world, holds components.
/// Create entities using <see cref="EntityPrototype"/>
/// </summary>
public sealed class Entity
{
    private Dictionary<Type, ComponentBase> _components;

    internal Entity(Level level)
    {
        Level = level;
    }

    /// <summary>
    /// Gets the level containing this entity.
    /// </summary>
    public Level Level { get; }

    /// <summary>
    /// Adds a component to this entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <returns>True if the component was successfully added. False if a component of the same type already exists on this entity.</returns>
    public bool TryAddComponent(ComponentBase component)
    {
        Type type = component.GetType();
        return _components.TryAdd(type, component);
    }

    /// <summary>
    /// Checks if a component of a specific type <see cref="T"/> exists on this entity.
    /// </summary>
    /// <typeparam name="T">The type of component to look for.</typeparam>
    /// <returns>If a component is found of type <see cref="T"/> the method will return true. Otherwise it will return false.</returns>
    public bool HasComponent<T>()
        where T : ComponentBase
    {
        return _components.TryGetValue(typeof(T), out _);
    }
    
    /// <summary>
    /// Checks if a component of a specific type <see cref="type"/> exists on this entity.
    /// </summary>
    /// <param name="type">The type of component to look for.</param>
    /// <returns>If a component is found of type <see cref="type"/> the method will return true. Otherwise it will return false.</returns>
    public bool HasComponent(Type type)
    {
        return _components.TryGetValue(type, out _);
    }

    /// <summary>
    /// Tries to get a component based on the type of said component.
    /// </summary>
    /// <param name="component">The component of type <see cref="T"/> if found. Otherwise it will be default.</param>
    /// <typeparam name="T">The type of component to look for.</typeparam>
    /// <returns>If a component is found of type <see cref="T"/> the method will return true. Otherwise it will return false.</returns>
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
        where T : ComponentBase
    {
        if (_components.TryGetValue(typeof(T), out ComponentBase? val) && val is T value)
        {
            component = value;
            return true;
        }

        component = default;
        return false;
    }
    
    /// <summary>
    /// Tries to get a component based on the type of said component.
    /// </summary>
    /// <param name="component">The component of type <see cref="type"/> if found. Otherwise it will be default.</param>
    /// <param name="type">The type of component to look for.</param>
    /// <returns>If a component is found of type <see cref="type"/> the method will return true. Otherwise it will return false.</returns>
    public bool TryGetComponent(Type type, [NotNullWhen(true)] out ComponentBase? component)
    {
        return _components.TryGetValue(type, out component);
    }
    
    /// <summary>
    /// Gets all components contained within this <see cref="Entity"/>.
    /// </summary>
    /// <returns>All components within this entity.</returns>
    public IEnumerator<ComponentBase> Components()
    {
        foreach ((Type _, ComponentBase component) in _components)
        {
            yield return component;
        }
    }
}