namespace JAngine.ECS;

/// <summary>
/// Shared methods, data and events for a collection of entities.
/// </summary>
public sealed class EntityCollection
{
    internal EntityCollection(SortedSet<Type> componentTypes)
    {
        
    }

    /// <summary>
    /// An entity matching this event type was created (added to the world).
    /// </summary>
    public event Action<Entity>? OnEntityCreated;
    /// <summary>
    /// An entity matching this event type was destroyed (removed from the world).
    /// </summary>
    internal event Action<Entity>? OnEntityDestroyed;
    
    /// <summary>
    /// An entity has been modified to match this event type (components were added).
    /// </summary>
    public event Action<Entity>? OnEntityAdded;
    /// <summary>
    /// An entity has been modified but still matches this event type (components were removed or added).
    /// </summary>
    public event Action<Entity>? OnEntityModified;
    /// <summary>
    /// An entity has been modified to not match this event type (components were removed).
    /// </summary>
    public event Action<Entity>? OnEntityRemoved;

    internal void InvokeEntityCreated(Entity entity)
    {
        OnEntityCreated?.Invoke(entity);
    }
    internal void InvokeEntityDestroyed(Entity entity)
    {
        OnEntityDestroyed?.Invoke(entity);
    }
    
    internal void InvokeEntityAdded(Entity entity)
    {
        OnEntityAdded?.Invoke(entity);
    }
    internal void InvokeEntityModified(Entity entity)
    {
        OnEntityModified?.Invoke(entity);
    }
    internal void InvokeEntityRemoved(Entity entity)
    {
        OnEntityRemoved?.Invoke(entity);
    }
}
