using System.Collections;

namespace JAngine;

/// <summary>
/// A world containing entities, can also compute changes to said entities using systems.
/// </summary>
public sealed class World
{
    private static ICollection<ISystem> _systems = Assemblies.CreateInstances<ISystem>().ToList();
    public static event Action<World>? WorldCreated;

    private readonly Dictionary<SortedSet<Type>, EntityArchetype> _archetypes = new();

    /// <summary>
    /// Creates a new world.
    /// </summary>
    public World()
    {
        WorldCreated?.Invoke(this);
    }
    
    internal EntityArchetype GetOrCreateArchetype(SortedSet<Type> types)
    {
        if (!_archetypes.TryGetValue(types, out EntityArchetype? archetype))
        {
            archetype = new EntityArchetype(this, types);
            _archetypes.Add(types, archetype);
        }

        return archetype;
    }

    private IEnumerable<EntityArchetype> GetAllMatchingArchetypes(SortedSet<Type> types)
    {
        foreach ((SortedSet<Type> componentTypes, EntityArchetype archetype) in _archetypes)
        {
            if (!componentTypes.IsSupersetOf(types))
            {
                continue;
            }

            yield return archetype;
        }
    }
    
    /// <summary>
    /// Creates a new entity and saves it in this world.
    /// </summary>
    /// <param name="components">The components of the newly created entity.</param>
    /// <returns>The newly created entity.</returns>
    public Entity CreateEntity(IEnumerable components)
    {
        return CreateEntity(components.OfType<object>().ToArray());
    }
    
    /// <inheritdoc cref="CreateEntity(System.Collections.IEnumerable)"/>
    public Entity CreateEntity(params object[] components)
    {
        SortedSet<Type> componentTypes = new SortedSet<Type>(components.Select(c => c.GetType()), TypeComparer.Default);
        EntityArchetype archetype = GetOrCreateArchetype(componentTypes);
        return archetype.AddEntity(null, components);
    }

    /// <inheritdoc cref="CreateEntity(System.Collections.IEnumerable)"/>
    /// <param name="component1">The value of the first component if any.</param>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    public Entity CreateEntity<T1>(T1? component1 = default)
    {
        return CreateEntity(new object[] { component1 });
    }
    
    /// <inheritdoc cref="CreateEntity(System.Collections.IEnumerable)"/>
    /// <param name="component1">The value of the first component if any.</param>
    /// <param name="component2">The value of the second component if any.</param>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    public Entity CreateEntity<T1, T2>(T1? component1 = default, T2? component2 = default)
    {
        return CreateEntity(new object[] { component1, component2 });
    }
    
    /// <inheritdoc cref="CreateEntity(System.Collections.IEnumerable)"/>
    /// <param name="component1">The value of the first component if any.</param>
    /// <param name="component2">The value of the second component if any.</param>
    /// <param name="component3">The value of the third component if any.</param>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    public Entity CreateEntity<T1, T2, T3>(T1? component1 = default, T2? component2 = default, T3? component3 = default)
    {
        return CreateEntity(new object[] { component1, component2, component3 });
    }

    /// <inheritdoc cref="CreateEntity(System.Collections.IEnumerable)"/>
    /// <param name="component1">The value of the first component if any.</param>
    /// <param name="component2">The value of the second component if any.</param>
    /// <param name="component3">The value of the third component if any.</param>
    /// <param name="component4">The value of the fourth component if any.</param>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    public Entity CreateEntity<T1, T2, T3, T4>(T1? component1 = default, T2? component2 = default, T3? component3 = default, T4? component4 = default)
    {
        return CreateEntity(new object[] { component1, component2, component3, component4 });
    }

    /// <summary>
    /// Gets the components of all entities that contains the components specified.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <returns>All components with the specified types.</returns>
    public IEnumerable<T1> GetComponents<T1>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1) })
            .SelectMany(e => e.GetAllComponentsOfType<T1>());
    }

    /// <summary>
    /// Gets the components of all entities that contains the components specified.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <returns>All components with the specified types.</returns>
    public IEnumerable<(T1, T2)> GetComponents<T1, T2>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2) })
            .SelectMany(e => e.GetAllComponentsOfType<T1>()
                .Zip(e.GetAllComponentsOfType<T2>(), (c1, c2) => (c1, c2)));
    }

    /// <summary>
    /// Gets the components of all entities that contains the components specified.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <returns>All components with the specified types.</returns>
    public IEnumerable<(T1, T2, T3)> GetComponents<T1, T2, T3>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2), typeof(T3) })
            .SelectMany(e => e.GetAllComponentsOfType<T1>()
                .Zip(e.GetAllComponentsOfType<T2>(), (c1, c2) => (c1, c2))
                .Zip(e.GetAllComponentsOfType<T3>(), (pair, c3) => (pair.c1, pair.c2, c3)));
    }

    /// <summary>
    /// Gets the components of all entities that contains the components specified.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    /// <returns>All components with the specified types.</returns>
    public IEnumerable<(T1, T2, T3, T4)> GetComponents<T1, T2, T3, T4>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2), typeof(T3) })
            .SelectMany(e => e.GetAllComponentsOfType<T1>()
                .Zip(e.GetAllComponentsOfType<T2>(), (c1, c2) => (c1, c2))
                .Zip(e.GetAllComponentsOfType<T3>(), (pair, c3) => (pair.c1, pair.c2, c3))
                .Zip(e.GetAllComponentsOfType<T4>(), (pair, c4) => (pair.c1, pair.c2, pair.c3, c4)));
    }

    /// <summary>
    /// Gets all entities of this world.
    /// </summary>
    /// <returns>All entities existing within this world.</returns>
    public IEnumerable<Entity> GetEntities()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default))
            .SelectMany(e => e.GetEntities());
    }
    
    /// <summary>
    /// Gets the entities with the specified component types.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <returns>All entities with the specified component types.</returns>
    public IEnumerable<Entity> GetEntities<T1>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1) })
            .SelectMany(e => e.GetEntities());
    }

    /// <summary>
    /// Gets the entities with the specified component types.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <returns>All entities with the specified component types.</returns>
    public IEnumerable<Entity> GetEntities<T1, T2>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2) })
            .SelectMany(e => e.GetEntities());
    }
    
    /// <summary>
    /// Gets the entities with the specified component types.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <returns>All entities with the specified component types.</returns>
    public IEnumerable<Entity> GetEntities<T1, T2, T3>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2), typeof(T3) })
            .SelectMany(e => e.GetEntities());
    }
    
    /// <summary>
    /// Gets the entities with the specified component types.
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    /// <returns>All entities with the specified component types.</returns>
    public IEnumerable<Entity> GetEntities<T1, T2, T3, T4>()
    {
        return GetAllMatchingArchetypes(new SortedSet<Type>(TypeComparer.Default) { typeof(T1), typeof(T2), typeof(T3), typeof(T4) })
            .SelectMany(e => e.GetEntities());
    }
}