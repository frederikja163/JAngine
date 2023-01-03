namespace JAngine.Rendering.OpenGL;

/// <summary>
/// The base of all OpenGL Objects.
/// </summary>
/// <typeparam name="T">The Handle type of this object.</typeparam>
public abstract class ObjectBase<T>: IDisposable where T : struct
{
    internal T Handle { get; private set; }
    
    internal ObjectBase(Func<T> createMethod)
    {
        Game.Instance.QueueCommand(() => Handle = createMethod());
    }

    /// <summary>
    /// Dispose this Object from the GPUs memory.
    /// </summary>
    public abstract void Dispose();
}