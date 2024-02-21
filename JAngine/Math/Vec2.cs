namespace JAngine.Math;

/// <summary>
/// A vector with 2 float components.
/// </summary>
public readonly struct Vec2
{
    private readonly int _x;
    private readonly int _y;
    
    /// <summary>
    /// Constructs a Vec2 using an x and y component.
    /// </summary>
    /// <param name="x">The first component of the vector.</param>
    /// <param name="y">The second component of the vector.</param>
    public Vec2(int x, int y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>
    /// Constructs a Vec2 using a single value for both x and y.
    /// </summary>
    /// <param name="value">The value of all components in the vector.</param>
    public Vec2(int value)
    {
        _x = value;
        _y = value;
    }

    /// <summary>
    /// Returns the first value of the vector.
    /// </summary>
    public int X => _x;
    
    /// <summary>
    /// Returns the second value of the vector.
    /// </summary>
    public int Y => _y;
    
    /// <summary>
    /// Returns the first value of the vector.
    /// </summary>
    public int R => _x;
    
    /// <summary>
    /// Returns the second value of the vector.
    /// </summary>
    public int G => _y;

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">Specifies the first of two vectors.</param>
    /// <param name="right">Specifies the second of two vectors.</param>
    /// <returns>Returns the dot product of two vectors, left and right. i.e., 'left.X * right.X + left.Y * right.Y'.</returns>
    public static float Dot(Vec2 left, Vec2 right)
    {
        return left.X * right.X + left.Y * right.Y;
    }
    
    
}
