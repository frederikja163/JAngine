using System.Xml;

namespace JAngine.Core;

/// <summary>
/// A container of entities and systems. Will regularly use the systems to update the levels.
/// </summary>
public sealed class Level
{
    private readonly List<Entity> _entities = new();

    public Level(XmlNode levelNode)
    {
        
    }

    /// <summary>
    /// Create an entity and add it to this level.
    /// </summary>
    /// <param name="prototype">The prototype to use for creating this entity.</param>
    /// <returns>The newly created entity.</returns>
    public Entity CreateEntity(EntityPrototype prototype)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> containing all entities of this level.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all entities in this level.</returns>
    public IEnumerable<Entity> Entities()
    {
        foreach (Entity entity in _entities)
        {
            yield return entity;
        }
    }
}