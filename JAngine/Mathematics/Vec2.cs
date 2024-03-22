// All methods are based on OpenGL (https://registry.khronos.org/OpenGL-Refpages/gl4/)

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
    /// Returns the first component of the vector.
    /// </summary>
    public float X => _x;
    
    /// <summary>
    /// Returns the second component of the vector.
    /// </summary>
    public float Y => _y;
    
    /// <summary>
    /// Returns the first component of the vector.
    /// </summary>
    public float R => _x;
    
    /// <summary>
    /// Returns the second component of the vector.
    /// </summary>
    public float G => _y;

    /// <summary>
    /// Adds a float to both components of a vector.
    /// </summary>
    /// <param name="left">The vector to be added to.</param>
    /// <param name="right">The value to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator +(Vec2 left, float right)
    {
        return new Vec2(left.X + right, left.Y + right);
    }

    /// <summary>
    /// Adds a float to both components of a vector.
    /// </summary>
    /// <param name="left">The value to add.</param>
    /// <param name="right">The vector to be added to.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator +(float left, Vec2 right)
    {
        return right + left;
    }

    /// <summary>
    /// Adds 2 vectors.
    /// </summary>
    /// <param name="left">The vector to add.</param>
    /// <param name="right">The vector to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator +(Vec2 left, Vec2 right)
    {
        return new Vec2(left.X + right.X, left.Y + right.Y);
    }
    
    /// <summary>
    /// Subtracts a float from both components of a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The value to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator -(Vec2 left, float right)
    {
        return new Vec2(left.X - right, left.Y - right);
    }

    /// <summary>
    /// Subtracts a vector from a float.
    /// </summary>
    /// <param name="left">The value to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator -(float left, Vec2 right)
    {
        return new Vec2(left - right.X, left - right.Y);
    }

    /// <summary>
    /// Subtracts a vector from a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator -(Vec2 left, Vec2 right)
    {
        return new Vec2(left.X - right.X, left.Y - right.Y);
    }
    
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
    /// Multiplies two vectors.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec2 operator *(Vec2 left, Vec2 right)
    {
        return new Vec2(left.X * right.X, left.Y * right.Y);
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
    /// Compares 2 vectors for equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator ==(Vec2 left, Vec2 right)
    {
        return (left.X == right.X) && (left.Y == right.Y);
    }
    
    /// <summary>
    /// Compares 2 vectors for non-equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator !=(Vec2 left, Vec2 right)
    {
        return !(left == right);
    }

    /// <inheritdoc cref="Vec4.Abs"/>
    public Vec2 Abs()
    {
        return new Vec2(MathF.Abs(this.X), MathF.Abs(this.Y));
    }

    /// <inheritdoc cref="Vec4.Acos"/>
    public Vec2 Acos()
    {
        return new Vec2(MathF.Acos(X), MathF.Acos(Y));
    }

    /// <inheritdoc cref="Vec4.Asin"/>
    public Vec2 Asin()
    {
        return new Vec2(MathF.Asin(X), MathF.Asin(Y));
    }
    
    /// <inheritdoc cref="Vec4.Atan()"/>
    public Vec2 Atan()
    {
        return new Vec2(MathF.Atan(X), MathF.Atan(Y));
    }

    /// <inheritdoc cref="Vec4.Ceil"/>
    public Vec2 Ceil()
    {
        return new Vec2(MathF.Ceiling(this.X), MathF.Ceiling(this.Y));
    }

    /// <inheritdoc cref="Vec4.Cos"/>
    public Vec2 Cos()
    {
        return new Vec2(MathF.Cos(X), MathF.Cos(Y));
    }

    /// <inheritdoc cref="Vec4.Degrees"/>
    public Vec2 Degrees()
    {
        return (this * 180) / MathF.PI;
    }

    /// <inheritdoc cref="Vec4.Exp"/>
    public Vec2 Exp()
    {
        return Pow(new Vec2(MathF.E), this);
    }

    /// <inheritdoc cref="Vec4.Exp2"/>
    public Vec2 Exp2()
    {
        return Pow(new Vec2(2), this);
    }

    /// <inheritdoc cref="Vec4.Floor"/>
    public Vec2 Floor()
    {
        return new Vec2(MathF.Floor(this.X), MathF.Floor(this.Y));
    }

    /// <inheritdoc cref="Vec4.Fract"/>
    public Vec2 Fract()
    {
        return this - Floor();
    }

    /// <inheritdoc cref="Vec4.Inversesqrt"/>
    public Vec2 Inversesqrt()
    {
        return 1 / Sqrt();
    }
    
    /// <inheritdoc cref="Vec4.Length"/>
    public float Length()
    {
        return MathF.Sqrt(_x * _x + _y * _y);
    }

    /// <inheritdoc cref="Vec4.Log"/>
    public Vec2 Log()
    {
        return new Vec2(MathF.Log(X), MathF.Log(Y));
    }

    /// <inheritdoc cref="Vec4.Log2"/>
    public Vec2 Log2()
    {
        return new Vec2(MathF.Log2(X), MathF.Log2(Y));
    }

    /// <inheritdoc cref="Vec4.Normalize"/>
    public Vec2 Normalize()
    {
        return this / Length();
    }

    /// <inheritdoc cref="Vec4.Radians"/>
    public Vec2 Radians()
    {
        return (this * MathF.PI) / 180;
    }

    /// <inheritdoc cref="Vec4.Sign"/>
    public Vec2 Sign()
    {
        return new Vec2(MathF.Sign(X), MathF.Sign(Y));
    }

    /// <inheritdoc cref="Vec4.Sin"/>
    public Vec2 Sin()
    {
        return new Vec2(MathF.Sin(X), MathF.Sin(Y));
    }

    /// <inheritdoc cref="Vec4.Sqrt"/>
    public Vec2 Sqrt()
    {
        return new Vec2(MathF.Sqrt(X), MathF.Sqrt(Y));
    }

    /// <inheritdoc cref="Vec4.Tan"/>
    public Vec2 Tan()
    {
        return new Vec2(MathF.Tan(X), MathF.Tan(Y));
    }

    /// <inheritdoc cref="Vec4.Atan(Vec4, Vec4)"/>
    public static Vec2 Atan(Vec2 left, Vec2 right)
    {
        return new Vec2(MathF.Atan(left.X / right.X), MathF.Atan(left.Y / right.Y));
    }

    /// <inheritdoc cref="Vec4.Clamp(Vec4, float, float)"/>
    public static Vec2 Clamp(Vec2 val, float min, float max)
    {
        return new Vec2(MathF.Min(MathF.Max(val.X, min), max), MathF.Min(MathF.Max(val.Y, min), max));
    }
    
    /// <inheritdoc cref="Clamp(JAngine.Mathematics.Vec2,float,float)"/>
    public static Vec2 Clamp(Vec2 val, Vec2 min, Vec2 max)
    {
        return new Vec2(MathF.Min(MathF.Max(val.X, min.X), max.X), MathF.Min(MathF.Max(val.Y, min.Y), max.Y));
    }
    
    /// <inheritdoc cref="Vec4.Distance"/>
    public static float Distance(Vec2 p0, Vec2 p1)
    {
        Vec2 vector = p0 - p1;

        return vector.Length();
    }
    
    /// <inheritdoc cref="Vec4.Dot"/>
    public static float Dot(Vec2 left, Vec2 right)
    {
        return left.X * right.X + left.Y * right.Y;
    }

    /// <inheritdoc cref="Vec4.Faceforward"/>
    public static Vec2 Faceforward(Vec2 n, Vec2 i, Vec2 nRef)
    {
        if (Vec2.Dot(i, nRef) < 0)
            return n;

        return n * -1;
    }

    /// <inheritdoc cref="Vec4.Max(Vec4, float)"/>
    public static Vec2 Max(Vec2 left, float right)
    {
        return new Vec2(MathF.Max(left.X, right), MathF.Max(left.Y, right));
    }

    /// <inheritdoc cref="Vec4.Max(Vec4, Vec4)"/>
    public static Vec2 Max(Vec2 left, Vec2 right)
    {
        return new Vec2(MathF.Max(left.X, right.X), MathF.Max(left.Y, right.Y));
    }
    
    /// <inheritdoc cref="Vec4.Min(Vec4, float)"/>
    public static Vec2 Min(Vec2 left, float right)
    {
        return new Vec2(MathF.Min(left.X, right), MathF.Min(left.Y, right));
    }
    
    /// <inheritdoc cref="Vec4.Min(Vec4, Vec4)"/>
    public static Vec2 Min(Vec2 left, Vec2 right)
    {
        return new Vec2(MathF.Min(left.X, right.X), MathF.Min(left.Y, right.Y));
    }

    /// <inheritdoc cref="Vec4.Pow"/>
    public static Vec2 Pow(Vec2 left, Vec2 right)
    {
        return new Vec2(MathF.Pow(left.X, right.X), MathF.Pow(left.Y, right.Y));
    }

    /// <inheritdoc cref="Vec4.Reflect"/>
    public static Vec2 Reflect(Vec2 i, Vec2 n)
    {
        n.Normalize();
        
        return i - 2 * Dot(i, n) * n;
    }

    /// <inheritdoc cref="Vec4.Smoothstep(float, float, Vec4)"/>
    public static Vec2 Smoothstep(float edge0, float edge1, Vec2 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
    
    /// <inheritdoc cref="Vec4.Smoothstep(Vec4, Vec4, Vec4)"/>
    public static Vec2 Smoothstep(Vec2 edge0, Vec2 edge1, Vec2 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
}
