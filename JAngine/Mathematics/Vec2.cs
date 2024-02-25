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

    /// <summary>
    /// Returns the absolute value of the vector.
    /// </summary>
    /// <returns>Returns the absolute value of the vector.</returns>
    public Vec2 Abs()
    {
        return new Vec2(Math.Abs(this.X), Math.Abs(this.Y));
    }

    /// <summary>
    /// Finds the nearest integer that is greater than or equal to the value.
    /// </summary>
    /// <returns>Returns a value equal to the nearest integer that is greater than or equal to the value.</returns>
    public Vec2 Ceil()
    {
        return new Vec2((float)Math.Ceiling(this.X), (float)Math.Ceiling(this.Y));
    }

    /// <summary>
    /// Returns the natural exponentiation of the value.
    /// </summary>
    /// <returns>Returns the natural exponentiation of the value. i.e., e^this.</returns>
    public Vec2 Exp()
    {
        return Pow(new Vec2((float)Math.E), this);
    }

    /// <summary>
    /// Returns 2 raised to the power of the value.
    /// </summary>
    /// <returns>Returns 2 raised to the power of the value. i.e., 2^this.</returns>
    public Vec2 Exp2()
    {
        return Pow(new Vec2(2), this);
    }

    /// <summary>
    /// Finds the nearest integer less than or equal to the value.
    /// </summary>
    /// <returns>Returns a value equal to the nearest integer that is less than or equal to the value.</returns>
    public Vec2 Floor()
    {
        return new Vec2((float)Math.Floor(this.X), (float)Math.Floor(this.Y));
    }

    /// <summary>
    /// Computes the fractional part of the value.
    /// </summary>
    /// <returns>Returns the fractional part of the value. This is calculated as 'this - Floor()'.</returns>
    public Vec2 Fract()
    {
        return this - Floor();
    }

    /// <summary>
    /// Returns the inverse of the square root of the value.
    /// </summary>
    /// <returns>Returns the inverse of the square root of the value; i.e. 1 / Sqrt(). The result is undefined if ≤0.</returns>
    public Vec2 Inversesqrt()
    {
        return 1 / Sqrt();
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
        return this / Length();
    }

    /// <summary>
    /// Extracts the sign of the value.
    /// </summary>
    /// <returns>Returns -1.0 if the value is less than 0.0, 0.0 if the value is equal to 0.0, and +1.0 if the value is greater than 0.</returns>
    public Vec2 Sign()
    {
        return new Vec2(Math.Sign(X), Math.Sign(Y));
    }

    /// <summary>
    /// Returns the square root of the value.
    /// </summary>
    /// <returns>Returns the square root of the value. The result is undefined if ≺0.</returns>
    public Vec2 Sqrt()
    {
        return new Vec2((float)Math.Sqrt(X), (float)Math.Sqrt(Y));
    }

    /// <summary>
    /// Constrains a value to lie between two further values.
    /// </summary>
    /// <param name="val">Specify the value to constrain.</param>
    /// <param name="min">Specify the lower end of the range into which to constrain val.</param>
    /// <param name="max">Specify the upper end of the range into which to constrain val.</param>
    /// <returns>Returns the value of val constrained to the range min to max. The returned value is computed as Min(Max(val, min), max).</returns>
    public static Vec2 Clamp(Vec2 val, float min, float max)
    {
        return new Vec2(Math.Min(Math.Max(val.X, min), max), Math.Min(Math.Max(val.Y, min), max));
    }
    
    /// <inheritdoc cref="Clamp(JAngine.Mathematics.Vec2,float,float)"/>
    public static Vec2 Clamp(Vec2 val, Vec2 min, Vec2 max)
    {
        return new Vec2(Math.Min(Math.Max(val.X, min.X), max.X), Math.Min(Math.Max(val.Y, min.Y), max.Y));
    }
    
    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="p0">Specifies the first of two points.</param>
    /// <param name="p1">Specifies the second of two points.</param>
    /// <returns>Returns the distance between the two points p0 and p1. i.e., 'Length(p0 - p1)'.</returns>
    public static float Distance(Vec2 p0, Vec2 p1)
    {
        Vec2 vector = p0 - p1;

        return vector.Length();
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
    /// Return a vector pointing in the same direction as another.
    /// </summary>
    /// <param name="n">Specifies the vector to orient.</param>
    /// <param name="i">Specifies the incident vector.</param>
    /// <param name="nRef">Specifies the reference vector.</param>
    /// <returns>Returns a vector that points away from a surface as defined by its normal. If Dot(nRef, i) ≺ 0 faceforward returns N, otherwise it returns -N.</returns>
    public static Vec2 Faceforward(Vec2 n, Vec2 i, Vec2 nRef)
    {
        if (Vec2.Dot(i, nRef) < 0)
            return n;

        return n * -1;
    }

    /// <summary>
    /// Returns the greater of two values.
    /// </summary>
    /// <param name="left">Specify the first value to compare.</param>
    /// <param name="right">Specify the second value to compare.</param>
    /// <returns>Returns the maximum of the two parameters. It returns right if right is greater than left, otherwise it returns left.</returns>
    public static Vec2 Max(Vec2 left, float right)
    {
        return new Vec2(Math.Max(left.X, right), Math.Max(left.Y, right));
    }

    ///<inheritdoc cref="Max(JAngine.Mathematics.Vec2,float)"/>
    public static Vec2 Max(Vec2 left, Vec2 right)
    {
        return new Vec2(Math.Max(left.X, right.X), Math.Max(left.Y, right.Y));
    }
    
    /// <summary>
    /// Returns the lesser of two values.
    /// </summary>
    /// <param name="left">Specify the first value to compare.</param>
    /// <param name="right">Specify the second value to compare.</param>
    /// <returns>Returns the minimum of the two parameters. It returns right if right is less than left, otherwise it returns left.</returns>
    public static Vec2 Min(Vec2 left, float right)
    {
        return new Vec2(Math.Min(left.X, right), Math.Min(left.Y, right));
    }
    
    /// <inheritdoc cref="Min(JAngine.Mathematics.Vec2,float)"/>
    public static Vec2 Min(Vec2 left, Vec2 right)
    {
        return new Vec2(Math.Min(left.X, right.X), Math.Min(left.Y, right.Y));
    }

    /// <summary>
    /// Returns the component of the first parameter raised to the power of the second.
    /// </summary>
    /// <param name="left">Specify the value to raise to the power of right.</param>
    /// <param name="right">Specify the power to which to raise left.</param>
    /// <returns>Returns the value of left raised to the right power, i.e. left^right. The result is undefined if left≺0 or if left=0 and right≤0.</returns>
    public static Vec2 Pow(Vec2 left, Vec2 right)
    {
        return new Vec2((float)Math.Pow(left.X, right.X), (float)Math.Pow(left.Y, right.Y));
    }

    /// <summary>
    /// Calculates the reflection direction for an incident vector.
    /// </summary>
    /// <param name="i">Specifies the incident vector.</param>
    /// <param name="n">Specifies the normal vector.</param>
    /// <returns>For a given incident vector I and surface normal N reflect returns the reflection direction calculated as 'i - 2 * Dot(n, i) * n'.</returns>
    public static Vec2 Reflect(Vec2 i, Vec2 n)
    {
        n.Normalize();
        
        return i - 2 * Dot(i, n) * n;
    }

    /// <summary>
    /// Performs Hermite interpolation between two values.
    /// </summary>
    /// <param name="edge0">Specifies the value of the lower edge of the Hermite function.</param>
    /// <param name="edge1">Specifies the value of the upper edge of the Hermite function.</param>
    /// <param name="src">Specifies the source value for interpolation.</param>
    /// <returns>Performs smooth Hermite interpolation between 0 and 1 when edge0 ≺ src ≺ edge1. This is useful in cases where a threshold function with a smooth transition is desired.</returns>
    public static Vec2 Smoothstep(float edge0, float edge1, Vec2 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
    
    /// <inheritdoc cref="Smoothstep(float,float,JAngine.Mathematics.Vec2)"/>
    public static Vec2 Smoothstep(Vec2 edge0, Vec2 edge1, Vec2 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
}
