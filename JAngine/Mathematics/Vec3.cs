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
    /// Constructs a Vec3 using x, y, and z components.
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
    /// Constructs a Vec3 using a single value for x, y, and z.
    /// </summary>
    /// <param name="value">The value of all components in the vector.</param>
    public Vec3(float value)
    {
        _x = value;
        _y = value;
        _z = value;
    }

    /// <inheritdoc cref="Vec2.X"/>
    public float X => _x;
    
    /// <inheritdoc cref="Vec2.Y"/>
    public float Y => _y;

    /// <summary>
    /// Returns the third component of the vector.
    /// </summary>
    public float Z => _z;
    
    /// <summary>
    /// Returns the first component of the vector.
    /// </summary>
    public float R => _x;
    
    /// <summary>
    /// Returns the second component of the vector.
    /// </summary>
    public float G => _y;
    
    /// <summary>
    /// Returns the third component of the vector.
    /// </summary>
    public float B => _z;

    
    
    
    
    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The vector to be added to.</param>
    /// <param name="right">The value to add.</param>
    /// <returns>A new vector with each component increased by the right value.</returns>
    public static Vec3 operator +(Vec3 left, float right)
    {
        return new Vec3(left.X + right, left.Y + right, left.Z + right);
    }

    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The value to add.</param>
    /// <param name="right">The vector to be added to.</param>
    /// <returns>A new vector with each component increased by the left value.</returns>
    public static Vec3 operator +(float left, Vec3 right)
    {
        return new Vec3(left + right.X, left + right.Y, left + right.Z);
    }

    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>A new vector resulting from component-wise addition.</returns>
    public static Vec3 operator +(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
    }

    /// <summary>
    /// Subtracts a float from all components of a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The value to subtract.</param>
    /// <returns>A new vector with each component decreased by the right value.</returns>
    public static Vec3 operator -(Vec3 left, float right)
    {
        return new Vec3(left.X - right, left.Y - right, left.Z - right);
    }

    /// <summary>
    /// Subtracts all components of a vector from a float.
    /// </summary>
    /// <param name="left">The value to subtract from.</param>
    /// <param name="right">The vector to subtract.</param>
    /// <returns>A new vector with each component of right subtracted from left.</returns>
    public static Vec3 operator -(float left, Vec3 right)
    {
        return new Vec3(left - right.X, left - right.Y, left - right.Z);
    }

    /// <summary>
    /// Subtracts the second vector from the first.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The vector to subtract.</param>
    /// <returns>A new vector resulting from component-wise subtraction.</returns>
    public static Vec3 operator -(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
    }

    /// <summary>
    /// Multiplies a vector by a float.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The value to multiply by.</param>
    /// <returns>A new vector with each component multiplied by the right value.</returns>
    public static Vec3 operator *(Vec3 left, float right)
    {
        return new Vec3(left.X * right, left.Y * right, left.Z * right);
    }

    /// <summary>
    /// Multiplies a vector by a float.
    /// </summary>
    /// <param name="left">The value to multiply by.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>A new vector with each component multiplied by the left value.</returns>
    public static Vec3 operator *(float left, Vec3 right)
    {
        return new Vec3(left * right.X, left * right.Y, left * right.Z);
    }

    /// <summary>
    /// Multiplies two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector to multiply.</param>
    /// <param name="right">The second vector to multiply.</param>
    /// <returns>A new vector resulting from component-wise multiplication.</returns>
    public static Vec3 operator *(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
    }

    /// <summary>
    /// Divides a vector by a float.
    /// </summary>
    /// <param name="left">The vector to divide.</param>
    /// <param name="right">The value to divide by.</param>
    /// <returns>A new vector with each component divided by the right value.</returns>
    public static Vec3 operator /(Vec3 left, float right)
    {
        return new Vec3(left.X / right, left.Y / right, left.Z / right);
    }

    /// <summary>
    /// Divides a float by all components of a vector.
    /// </summary>
    /// <param name="left">The value to divide.</param>
    /// <param name="right">The vector to divide by.</param>
    /// <returns>A new vector with left divided by each component of the right vector.</returns>
    public static Vec3 operator /(float left, Vec3 right)
    {
        return new Vec3(left / right.X, left / right.Y, left / right.Z);
    }

    /// <summary>
    /// Divides the first vector by the second vector component-wise.
    /// </summary>
    /// <param name="left">The vector to divide.</param>
    /// <param name="right">The vector to divide by.</param>
    /// <returns>A new vector resulting from component-wise division.</returns>
    public static Vec3 operator /(Vec3 left, Vec3 right)
    {
        return new Vec3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
    }

    /// <summary>
    /// Compares two vectors for equality.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns>True if the vectors have the same components; otherwise, false.</returns>
    public static bool operator ==(Vec3 left, Vec3 right)
    {
        return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
    }

    /// <summary>
    /// Compares two vectors for inequality.
    /// </summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns>True if the vectors do not have the same components; otherwise, false.</returns>
    public static bool operator !=(Vec3 left, Vec3 right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Returns the absolute value of the vector components.
    /// </summary>
    /// <returns>A new vector with the absolute value of each component.</returns>
    public Vec3 Abs()
    {
        return new Vec3(MathF.Abs(X), MathF.Abs(Y), MathF.Abs(Z));
    }

    /// <summary>
    /// Returns a vector with the arccosine of each component.
    /// </summary>
    /// <returns>A new vector with the arccosine of each component.</returns>
    public Vec3 Acos()
    {
        return new Vec3(MathF.Acos(X), MathF.Acos(Y), MathF.Acos(Z));
    }

    /// <summary>
    /// Returns a vector with the arcsine of each component.
    /// </summary>
    /// <returns>A new vector with the arcsine of each component.</returns>
    public Vec3 Asin()
    {
        return new Vec3(MathF.Asin(X), MathF.Asin(Y), MathF.Asin(Z));
    }

    /// <summary>
    /// Returns a vector with the arctangent of each component.
    /// </summary>
    /// <returns>A new vector with the arctangent of each component.</returns>
    public Vec3 Atan()
    {
        return new Vec3(MathF.Atan(X), MathF.Atan(Y), MathF.Atan(Z));
    }
    
    /// <summary>
    /// Returns a vector with the ceiling of each component.
    /// </summary>
    /// <returns>A new vector with the ceiling of each component.</returns>
    public Vec3 Ceil()
    {
        return new Vec3(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(Z));
    }

    /// <summary>
    /// Returns a vector with the cosine of each component.
    /// </summary>
    /// <returns>A new vector with the cosine of each component.</returns>
    public Vec3 Cos()
    {
        return new Vec3(MathF.Cos(X), MathF.Cos(Y), MathF.Cos(Z));
    }
    
    /// <summary>
    /// Converts the vector from radians to degrees.
    /// </summary>
    /// <returns>A new vector with each component converted from radians to degrees.</returns>
    public Vec3 Degrees()
    {
        return this * (180 / MathF.PI);
    }

    /// <summary>
    /// Returns a vector with the exponential (e^) of each component.
    /// </summary>
    /// <returns>A new vector with the exponential of each component.</returns>
    public Vec3 Exp()
    {
        return new Vec3(MathF.Exp(X), MathF.Exp(Y), MathF.Exp(Z));
    }
    
    /// <summary>
    /// Returns a vector with 2 raised to the power of each component.
    /// </summary>
    /// <returns>A new vector with 2 raised to the power of each component.</returns>
    public Vec3 Exp2()
    {
        return new Vec3(MathF.Pow(2, X), MathF.Pow(2, Y), MathF.Pow(2, Z));
    }

    /// <summary>
    /// Returns a vector with the floor of each component.
    /// </summary>
    /// <returns>A new vector with the floor of each component.</returns>
    public Vec3 Floor()
    {
        return new Vec3(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(Z));
    }

    /// <summary>
    /// Returns a vector with each component being the fractional part.
    /// </summary>
    /// <returns>A new vector with the fractional part of each component.</returns>
    public Vec3 Fract()
    {
        return this - Floor();
    }
    
    /// <summary>
    /// Returns the inverse of the square root of each component.
    /// </summary>
    /// <returns>A new vector with the inverse of the square root of each component.</returns>
    public Vec3 Inversesqrt()
    {
        return 1 / Sqrt();
    }

    /// <summary>
    /// Returns the length (magnitude) of the vector.
    /// </summary>
    /// <returns>The length of the vector.</returns>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y + Z * Z);
    }

    /// <summary>
    /// Returns a vector with the natural logarithm (base e) of each component.
    /// </summary>
    /// <returns>A new vector with the natural logarithm of each component.</returns>
    public Vec3 Log()
    {
        return new Vec3(MathF.Log(X), MathF.Log(Y), MathF.Log(Z));
    }

    /// <summary>
    /// Returns a vector normalized to unit length.
    /// </summary>
    /// <returns>A new vector of unit length in the same direction as the original vector.</returns>
    public Vec3 Normalize()
    {
        float length = Length();
        if (length > 0)
        {
            return this / length;
        }
        return new Vec3(0);
    }

    /// <summary>
    /// Converts the vector from degrees to radians.
    /// </summary>
    /// <returns>A new vector with each component converted from degrees to radians.</returns>
    public Vec3 Radians()
    {
        return this * (MathF.PI / 180);
    }

    /// <summary>
    /// Returns a vector with the sine of each component.
    /// </summary>
    /// <returns>A new vector with the sine of each component.</returns>
    public Vec3 Sin()
    {
        return new Vec3(MathF.Sin(X), MathF.Sin(Y), MathF.Sin(Z));
    }

    /// <summary>
    /// Returns a vector with the square root of each component.
    /// </summary>
    /// <returns>A new vector with the square root of each component.</returns>
    public Vec3 Sqrt()
    {
        return new Vec3(MathF.Sqrt(X), MathF.Sqrt(Y), MathF.Sqrt(Z));
    }

    /// <summary>
    /// Returns a vector with the tangent of each component.
    /// </summary>
    /// <returns>A new vector with the tangent of each component.</returns>
    public Vec3 Tan()
    {
        return new Vec3(MathF.Tan(X), MathF.Tan(Y), MathF.Tan(Z));
    }
    
    /// <summary>
    /// Constrains a value to lie between two further values.
    /// </summary>
    /// <param name="val">The value to constrain.</param>
    /// <param name="min">The lower end of the range into which to constrain val.</param>
    /// <param name="max">The upper end of the range into which to constrain val.</param>
    /// <returns>The value of val constrained to the range min to max.</returns>
    public static Vec3 Clamp(Vec3 val, float min, float max)
    {
        return new Vec3(MathF.Min(MathF.Max(val.X, min), max), MathF.Min(MathF.Max(val.Y, min), max), MathF.Min(MathF.Max(val.Z, min), max));
    }

    /// <summary>
    /// Constrains a value to lie between two further vector values.
    /// </summary>
    /// <param name="val">The value to constrain.</param>
    /// <param name="min">The vector representing the lower end of the range into which to constrain val.</param>
    /// <param name="max">The vector representing the upper end of the range into which to constrain val.</param>
    /// <returns>The vector of val constrained to the range min to max.</returns>
    public static Vec3 Clamp(Vec3 val, Vec3 min, Vec3 max)
    {
        return new Vec3(MathF.Min(MathF.Max(val.X, min.X), max.X), MathF.Min(MathF.Max(val.Y, min.Y), max.Y), MathF.Min(MathF.Max(val.Z, min.Z), max.Z));
    }
    
    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="p0">The first point.</param>
    /// <param name="p1">The second point.</param>
    /// <returns>The distance between points p0 and p1.</returns>
    public static float Distance(Vec3 p0, Vec3 p1)
    {
        Vec3 vector = p0 - p1;
        return vector.Length();
    }
    
    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The dot product of vectors left and right.</returns>
    public static float Dot(Vec3 left, Vec3 right)
    {
        return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
    }

    /// <summary>
    /// Orient a vector to point away from a surface as defined by its normal.
    /// </summary>
    /// <param name="n">The normal vector of the surface.</param>
    /// <param name="i">The incident vector.</param>
    /// <param name="nRef">The reference vector to determine the side of the surface.</param>
    /// <returns>The vector pointed away from the surface.</returns>
    public static Vec3 Faceforward(Vec3 n, Vec3 i, Vec3 nRef)
    {
        if (Dot(nRef, i) < 0)
            return n;
        else
            return -n;
    }

    /// <summary>
    /// Returns the greater of two values for each component.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The scalar value to compare with each component.</param>
    /// <returns>A vector with each component being the maximum of the original component and the scalar value.</returns>
    public static Vec3 Max(Vec3 left, float right)
    {
        return new Vec3(MathF.Max(left.X, right), MathF.Max(left.Y, right), MathF.Max(left.Z, right));
    }

    /// <summary>
    /// Returns the greater of two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with each component being the maximum of the corresponding components of the two vectors.</returns>
    public static Vec3 Max(Vec3 left, Vec3 right)
    {
        return new Vec3(MathF.Max(left.X, right.X), MathF.Max(left.Y, right.Y), MathF.Max(left.Z, right.Z));
    }
    
    /// <summary>
    /// Returns the lesser of two values for each component.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The scalar value to compare with each component.</param>
    /// <returns>A vector with each component being the minimum of the original component and the scalar value.</returns>
    public static Vec3 Min(Vec3 left, float right)
    {
        return new Vec3(MathF.Min(left.X, right), MathF.Min(left.Y, right), MathF.Min(left.Z, right));
    }

    /// <summary>
    /// Returns the lesser of two vectors component-wise.
    /// </summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>A vector with each component being the minimum of the corresponding components of the two vectors.</returns>
    public static Vec3 Min(Vec3 left, Vec3 right)
    {
        return new Vec3(MathF.Min(left.X, right.X), MathF.Min(left.Y, right.Y), MathF.Min(left.Z, right.Z));
    }

    /// <summary>
    /// Reflects a vector off a surface that has a specified normal.
    /// </summary>
    /// <param name="i">The incident vector.</param>
    /// <param name="n">The normal vector of the surface.</param>
    /// <returns>The reflection direction.</returns>
    public static Vec3 Reflect(Vec3 i, Vec3 n)
    {
        return i - 2 * Dot(n, i) * n;
    }

    /// <summary>
    /// Performs smooth Hermite interpolation between 0 and 1 when edge0 &lt; x &lt; edge1.
    /// </summary>
    /// <param name="edge0">The value of the lower edge of the Hermite function.</param>
    /// <param name="edge1">The value of the upper edge of the Hermite function.</param>
    /// <param name="x">The source value for interpolation.</param>
    /// <returns>The interpolated value.</returns>
    public static Vec3 Smoothstep(float edge0, float edge1, Vec3 x)
    {
        Vec3 t = Clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return t * t * (3 - 2 * t);
    }

    // Implementations for Equals, GetHashCode, and ToString methods omitted for brevity but should be included for completeness.
}
