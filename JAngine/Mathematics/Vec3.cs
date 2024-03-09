// All methods are based on OpenGL (https://registry.khronos.org/OpenGL-Refpages/gl4/)

namespace JAngine.Mathematics;

/// <summary>
/// A vector with 3 float components.
/// </summary>
public readonly struct Vec3
{
    private readonly float _x;
    private readonly float _y;
    private readonly float _z;

    /// <summary>
    /// Constructs a Vec3 using an x, y, and z component.
    /// </summary>
    /// <param name="x">The first component of the vector.</param>
    /// <param name="y">The second component of the vector.</param>
    /// <param name="z">The third component of the vector.</param>
    public Vec3(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    /// <summary>
    /// Constructs a Vec3 using a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components in the vector.</param>
    public Vec3(float value)
    {
        _x = value;
        _y = value;
        _z = value;
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
    /// Returns the third component of the vector.
    /// </summary>
    public float Z => _z;

    /// <inheritdoc cref="X"/>
    public float R => _x;

    /// <inheritdoc cref="Y"/>
    public float G => _y;

    /// <inheritdoc cref="Z"/>
    public float B => _z;

    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The vector to be added to.</param>
    /// <param name="right">The value to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator +(Vec3 left, float right)
    {
        return new Vec3(left.X + right, left.Y + right, left.Z + right);
    }

    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The value to add.</param>
    /// <param name="right">The vector to be added to.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator +(float left, Vec3 right)
    {
        return right + left;
    }

    /// <summary>
    /// Adds 2 vectors.
    /// </summary>
    /// <param name="left">The vector to add.</param>
    /// <param name="right">The vector to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator +(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Subtracts a float from all components of a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The value to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator -(Vec3 left, float right)
    {
        return new Vec3(left.X - right, left.Y - right, left.Z - right);
    }

    /// <summary>
    /// Subtracts a vector from a float.
    /// </summary>
    /// <param name="left">The value to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator -(float left, Vec3 right)
    {
        return new Vec3(left - right.X, left - right.Y, left - right.Z);
    }

    /// <summary>
    /// Subtracts a vector from a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator -(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The value to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator *(Vec3 left, float right)
    {
        return new Vec3(left.X * right, left.Y * right, left.Z * right);
    }

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The value to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator *(float left, Vec3 right)
    {
        return right * left;
    }

    /// <summary>
    /// Multiplies two vectors.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator *(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Divides a vector with a float.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The value to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator /(Vec3 left, float right)
    {
        return new Vec3(left.X / right, left.Y / right, left.Z / right);
    }

    /// <summary>
    /// Divides a float with a vector.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator /(float left, Vec3 right)
    {
        return new Vec3(left / right.X, left / right.Y, left / right.Z);
    }

    /// <summary>
    /// Divides a vector with a vector.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec3 operator /(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Compares 2 vectors for equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator ==(Vec3 left, Vec3 right)
    {
        return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    }

    /// <summary>
    /// Compares 2 vectors for non-equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator !=(Vec3 left, Vec3 right)
    {
        return !(left == right);
    }

    /// <inheritdoc cref="Vec4.Abs"/>
    public Vec3 Abs()
    {
        return new Vec3(MathF.Abs(X), MathF.Abs(Y), MathF.Abs(Z));
    }

    /// <inheritdoc cref="Vec4.Acos"/>
    public Vec3 Acos()
    {
        return new Vec3(MathF.Acos(X), MathF.Acos(Y), MathF.Acos(Z));
    }

    /// <inheritdoc cref="Vec4.Asin"/>
    public Vec3 Asin()
    {
        return new Vec3(MathF.Asin(X), MathF.Asin(Y), MathF.Asin(Z));
    }

    /// <inheritdoc cref="Vec4.Atan()"/>
    public Vec3 Atan()
    {
        return new Vec3(MathF.Atan(X), MathF.Atan(Y), MathF.Atan(Z));
    }

    /// <inheritdoc cref="Vec4.Ceil"/>
    public Vec3 Ceil()
    {
        return new Vec3(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(Z));
    }

    /// <inheritdoc cref="Vec4.Cos"/>
    public Vec3 Cos()
    {
        return new Vec3(MathF.Cos(X), MathF.Cos(Y), MathF.Cos(Z));
    }

    /// <inheritdoc cref="Vec4.Degrees"/>
    public Vec3 Degrees()
    {
        return (this * 180) / MathF.PI;
    }

    /// <inheritdoc cref="Vec4.Exp"/>
    public Vec3 Exp()
    {
        return Pow(new Vec3(MathF.E), this);
    }

    /// <inheritdoc cref="Vec4.Exp2"/>
    public Vec3 Exp2()
    {
        return Pow(new Vec3(2), this);
    }

    /// <inheritdoc cref="Vec4.Floor"/>
    public Vec3 Floor()
    {
        return new Vec3(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(Z));
    }

    /// <inheritdoc cref="Vec4.Fract"/>
    public Vec3 Fract()
    {
        return this - Floor();
    }

    /// <inheritdoc cref="Vec4.Inversesqrt"/>
    public Vec3 Inversesqrt()
    {
        return 1 / Sqrt();
    }

    /// <inheritdoc cref="Vec4.Length"/>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <inheritdoc cref="Vec4.Log"/>
    public Vec3 Log()
    {
        return new Vec3(MathF.Log(X), MathF.Log(Y), MathF.Log(Z));
    }

    /// <inheritdoc cref="Vec4.Log2"/>
    public Vec3 Log2()
    {
        return new Vec3(MathF.Log2(X), MathF.Log2(Y), MathF.Log2(Z));
    }

    /// <inheritdoc cref="Vec4.Normalize"/>
    public Vec3 Normalize()
    {
        return this / Length();
    }

    /// <inheritdoc cref="Vec4.Radians"/>
    public Vec3 Radians()
    {
        return (this * MathF.PI) / 180;
    }

    /// <inheritdoc cref="Vec4.Sign"/>
    public Vec3 Sign()
    {
        return new Vec3(MathF.Sign(X), MathF.Sign(Y), MathF.Sign(Z));
    }

    /// <inheritdoc cref="Vec4.Sin"/>
    public Vec3 Sin()
    {
        return new Vec3(MathF.Sin(X), MathF.Sin(Y), MathF.Sin(Z));
    }

    /// <inheritdoc cref="Vec4.Sqrt"/>
    public Vec3 Sqrt()
    {
        return new Vec3(MathF.Sqrt(X), MathF.Sqrt(Y), MathF.Sqrt(Z));
    }

    /// <inheritdoc cref="Vec4.Tan"/>
    public Vec3 Tan()
    {
        return new Vec3(MathF.Tan(X), MathF.Tan(Y), MathF.Tan(Z));
    }

    /// <inheritdoc cref="Vec4.Atan(Vec4, Vec4)"/>
    public static Vec3 Atan(Vec3 left, Vec3 right)
    {
        return new Vec3(
            MathF.Atan(left.X / right.X),
            MathF.Atan(left.Y / right.Y),
            MathF.Atan(left.Z / right.Z));
    }

    /// <inheritdoc cref="Vec4.Clamp(Vec4, float, float)"/>
    public static Vec3 Clamp(Vec3 val, float min, float max)
    {
        return new Vec3(
            MathF.Min(MathF.Max(val.X, min), max), 
            MathF.Min(MathF.Max(val.Y, min), max),
            MathF.Min(MathF.Max(val.Z, min), max));
    }

    /// <inheritdoc cref="Clamp(JAngine.Mathematics.Vec3,float,float)"/>
    public static Vec3 Clamp(Vec3 val, Vec3 min, Vec3 max)
    {
        return new Vec3(
            MathF.Min(MathF.Max(val.X, min.X), max.X), 
            MathF.Min(MathF.Max(val.Y, min.Y), max.Y),
            MathF.Min(MathF.Max(val.Z, min.Z), max.Z));
    }

    /// <inheritdoc cref="Vec4.Distance"/>
    public static float Distance(Vec3 p0, Vec3 p1)
    {
        Vec3 vector = p0 - p1;

        return vector.Length();
    }

    /// <inheritdoc cref="Vec4.Dot"/>
    public static float Dot(Vec3 left, Vec3 right)
    {
        return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
    }

    /// <inheritdoc cref="Vec4.Faceforward"/>
    public static Vec3 Faceforward(Vec3 n, Vec3 i, Vec3 nRef)
    {
        if (Vec3.Dot(i, nRef) < 0)
            return n;

        return n * -1;
    }

    /// <inheritdoc cref="Vec4.Max(Vec4, float)"/>
    public static Vec3 Max(Vec3 left, float right)
    {
        return new Vec3(
            MathF.Max(left.X, right), 
            MathF.Max(left.Y, right),
            MathF.Max(left.Z, right));
    }

    /// <inheritdoc cref="Vec4.Max(Vec4, Vec4)"/>
    public static Vec3 Max(Vec3 left, Vec3 right)
    {
        return new Vec3(
            MathF.Max(left.X, right.X), 
            MathF.Max(left.Y, right.Y),
            MathF.Max(left.Z, right.Z));
    }

    /// <inheritdoc cref="Vec4.Min(Vec4, float)"/>
    public static Vec3 Min(Vec3 left, float right)
    {
        return new Vec3(
            MathF.Min(left.X, right), 
            MathF.Min(left.Y, right),
            MathF.Min(left.Z, right));
    }

    /// <inheritdoc cref="Vec4.Min(Vec4, Vec4)"/>
    public static Vec3 Min(Vec3 left, Vec3 right)
    {
        return new Vec3(
            MathF.Min(left.X, right.X), 
            MathF.Min(left.Y, right.Y),
            MathF.Min(left.Z, right.Z));
    }

    /// <inheritdoc cref="Vec4.Pow"/>
    public static Vec3 Pow(Vec3 left, Vec3 right)
    {
        return new Vec3(
            MathF.Pow(left.X, right.X), 
            MathF.Pow(left.Y, right.Y),
            MathF.Pow(left.Z, right.Z));
    }

    /// <inheritdoc cref="Vec4.Reflect"/>
    public static Vec3 Reflect(Vec3 i, Vec3 n)
    {
        n.Normalize();

        return i - 2 * Dot(i, n) * n;
    }

    /// <inheritdoc cref="Vec4.Smoothstep(float, float, Vec4)"/>
    public static Vec3 Smoothstep(float edge0, float edge1, Vec3 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }

    /// <inheritdoc cref="Vec4.Smoothstep(Vec4, Vec4, Vec4)"/>
    public static Vec3 Smoothstep(Vec3 edge0, Vec3 edge1, Vec3 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
}
