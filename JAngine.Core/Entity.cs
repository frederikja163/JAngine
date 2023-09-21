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

    public void AddComponent<T>(T value)
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes, TypeComparer.Default);
        newComponentTypes.Add(typeof(T));
        SetArchetype(newComponentTypes, value);
    }
    
    public void AddComponent<T>()
    {
        SortedSet<Type> newComponentTypes = new SortedSet<Type>(Archetype.ComponentTypes, TypeComparer.Default);
        newComponentTypes.Add(typeof(T));
        SetArchetype(newComponentTypes);
    }

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