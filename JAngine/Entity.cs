using System.Diagnostics.CodeAnalysis;

namespace JAngine;

/// <summary>
/// An entity in the world containing different components. To create an entity see <see cref="World.CreateEntity(System.Collections.Generic.IEnumerable{object})"/>
/// </summary>
public sealed class Entity
{
    internal Entity(EntityArchetype archetype, Guid id)
    {
        Archetype = archetype;
        Id = id;
    }
    
    internal Guid Id { get; set; }

    internal EntityArchetype Archetype { get; set; }

    /// <summary>
    /// Gets the world this entity is part of.
    /// </summary>
    public World World => Archetype.World;
    
    /// <summary>
    /// Tries to get a component from this entity.
    /// </summary>
    /// <param name="component">The found component if any. Will only be null if return-value is false.</param>
    /// <typeparam name="T">The type of component to look for.</typeparam>
    /// <returns>True if a component was found; false if this type of component does not exist on this entity.</returns>
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
    {
        return Archetype.TryGetComponent(this, out component);
    }

    /// <inheritdoc cref="AddComponent{T}()"/>
    /// <param name="value">The value of the new component.</param>
    public void AddComponent<T>(T value)
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes, TypeComparer.Default);
        newComponentTypes.Add(typeof(T));
        SetArchetype(newComponentTypes, value);
    }
    
    /// <summary>
    /// Add a component to this entity, moving its archetype.
    /// </summary>
    /// <typeparam name="T">The type of the new component.</typeparam>
    public void AddComponent<T>()
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes, TypeComparer.Default);
        newComponentTypes.Add(typeof(T));
        SetArchetype(newComponentTypes);
    }

    /// <summary>
    /// Remove a component from this entity, moving its archetype.
    /// </summary>
    /// <typeparam name="T">The type of the new component.</typepaAddram>
    public void RemoveComponent<T>()
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes, TypeComparer.Default);
        newComponentTypes.Remove(typeof(T));
        SetArchetype(newComponentTypes);
    }

    private void SetArchetype(SortedSet<Type> newComponentTypes, object? value = null)
    {
        List<object> componentValues = Archetype.RemoveEntity(this);
        if (value is not null)
        {
            componentValues.Add(value);
        }
        Archetype = World.GetOrCreateArchetype(newComponentTypes);
        Archetype.AddEntity(this, componentValues);
    }
}