namespace JAngine.Core;

/// <summary>
/// The base of a single system used to manipulate entities.
/// </summary>
public abstract class SystemBase
{
    private Level? _level;
    
    /// <summary>
    /// Gets the level containing this system.
    /// </summary>
    /// <exception cref="Exception">Thrown if this system hasn't been added to a level yet.</exception>
    public Level Level
    {
        get => _level ?? throw new Exception("Cannot get entity before an entity has been set.");
        set
        {
            if (_level is not null)
            {
                throw new Exception("Can only add components to a single entity at a time.");
            }
            _level = value;
        }
    }
}