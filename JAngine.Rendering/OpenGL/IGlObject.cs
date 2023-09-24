namespace JAngine.Rendering.OpenGL;

internal interface IGlObject
{
    /// <summary>
    /// Should ONLY be called on an OpenGL thread.
    /// </summary>
    void ApplyChanges();
}