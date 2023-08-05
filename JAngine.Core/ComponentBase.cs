namespace JAngine.Core;

/// <summary>
/// The base of a single component stored inside entities.
/// </summary>
public abstract class ComponentBase
{
    private Entity? _entity;

    /// <summary>
    /// The parent entity containing this component.
    /// </summary>
    /// <exception cref="Exception">Thrown if no parent exists for this component.</exception>
    public Entity Entity
    {
        get => _entity ?? throw new Exception("Cannot get entity before an entity has been set.");
        set
        {
            if (_entity is not null)
            {
                throw new Exception("Can only add components to a single entity at a time.");
            }
            _entity = value;
        }
    }
}