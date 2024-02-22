namespace JAngine.Mathematics;

/// <summary>
/// A vector with 2 float components.
/// </summary>
public readonly struct Vec2
{
    private readonly float _x;
    private readonly float _y;
    
    /// <summary>
    /// Constructs a Vec2 using an x and y component.
    /// </summary>
    /// <param name="x">The first component of the vector.</param>
    /// <param name="y">The second component of the vector.</param>
    public Vec2(float x, float y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>
    /// Constructs a Vec2 using a single value for both x and y.
    /// </summary>
    /// <param name="value">The value of all components in the vector.</param>
    public Vec2(float value)
    {
        _x = value;
        _y = value;
    }

    /// <summary>
    /// Returns the first value of the vector.
    /// </summary>
    public float X => _x;
    
    /// <summary>
    /// Returns the second value of the vector.
    /// </summary>
    public float Y => _y;
    
    /// <summary>
    /// Returns the first value of the vector.
    /// </summary>
    public float R => _x;
    
    /// <summary>
    /// Returns the second value of the vector.
    /// </summary>
    public float G => _y;

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The value to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator *(Vec2 left, float right)
    {
        return new Vec2(left.X * right, left.Y * right);
    }

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The value to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator *(float left, Vec2 right)
    {
        return right * left;
    }

    /// <summary>
    /// Divides a vector with a float.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The value to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator /(Vec2 left, float right)
    {
        return new Vec2(left.X / right, left.Y / right);
    }

    /// <summary>
    /// Divides a float with a vector.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator /(float left, Vec2 right)
    {
        return new Vec2(left / right.X, left / right.Y);
    }

    /// <summary>
    /// Divides a vector with a vector.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator /(Vec2 left, Vec2 right)
    {
        return new Vec2(left.X / right.X, left.Y / right.Y);
    }
    
    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>Returns the length of the vector, i.e. '√(X² + Y²)'.</returns>
    public float Length()
    {
        return (float)Math.Sqrt(_x * _x + _y * _y);
    }

    /// <summary>
    /// Calculates the unit vector in the same direction as the original vector.
    /// </summary>
    /// <returns>Returns a vector with the same direction as its parameter, v, but with length 1.</returns>
    public Vec2 Normalize()
    {
        return this * (1 / Length());
    }
    
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
    
    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="p0">Specifies the first of two points.</param>
    /// <param name="p1">Specifies the second of two points.</param>
    /// <returns>Returns the distance between the two points p0 and p1. i.e., 'length(p0 - p1)'.</returns>
    public static float Distance(Vec2 p0, Vec2 p1) // TODO: fix when overloading minus
    {
        return (float)Math.Sqrt(Math.Pow(p0.X - p1.X, 2) + Math.Pow(p0.Y - p1.Y, 2));
    }
}
