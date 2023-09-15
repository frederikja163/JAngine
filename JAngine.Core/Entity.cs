using System.Diagnostics.CodeAnalysis;

namespace JAngine.Core;

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

    public World World => Archetype.World;
    
    public bool TryGetComponent<T>([NotNullWhen(true)] out T? component)
    {
        return Archetype.TryGetComponent(this, out component);
    }

    public void AddComponent<T>()
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes);
        newComponentTypes.Add(typeof(T));
        SetArchetype(newComponentTypes);
    }

    public void RemoveComponent<T>()
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes);
        newComponentTypes.Remove(typeof(T));
        SetArchetype(newComponentTypes);
    }

    private void SetArchetype(SortedSet<Type> newComponentTypes)
    {
        Archetype.RemoveEntity(this);
        Archetype = World.GetOrCreateArchetype(newComponentTypes);
        Archetype.AddEntity(this);
    }
}