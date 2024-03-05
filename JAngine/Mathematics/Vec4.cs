// All methods are based on OpenGL (https://registry.khronos.org/OpenGL-Refpages/gl4/)

namespace JAngine.Mathematics;

/// <summary>
/// A vector with 4 float components.
/// </summary>
public readonly struct Vec4
{
    private readonly float _x;
    private readonly float _y;
    private readonly float _z;
    private readonly float _w;

    /// <summary>
    /// Constructs a Vec4 using an x, y, z, and w component.
    /// </summary>
    /// <param name="x">The first component of the vector.</param>
    /// <param name="y">The second component of the vector.</param>
    /// <param name="z">The third component of the vector.</param>
    /// <param name="w">The fourth component of the vector.</param>
    public Vec4(float x, float y, float z, float w)
    {
        _x = x;
        _y = y;
        _z = z;
        _w = w;
    }

    /// <summary>
    /// Constructs a Vec4 using a single value for all components.
    /// </summary>
    /// <param name="value">The value of all components in the vector.</param>
    public Vec4(float value)
    {
        _x = value;
        _y = value;
        _z = value;
        _w = value;
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

    /// <summary>
    /// Returns the fourth component of the vector.
    /// </summary>
    public float W => _w;

    /// <inheritdoc cref="X"/>
    public float R => _x;

    /// <inheritdoc cref="Y"/>
    public float G => _y;

    /// <inheritdoc cref="Z"/>
    public float B => _y;

    /// <inheritdoc cref="W"/>
    public float A => _y;

    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The vector to be added to.</param>
    /// <param name="right">The value to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator +(Vec4 left, float right)
    {
        return new Vec4(left.X + right, left.Y + right, left.Z + right, left.W + right);
    }

    /// <summary>
    /// Adds a float to all components of a vector.
    /// </summary>
    /// <param name="left">The value to add.</param>
    /// <param name="right">The vector to be added to.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator +(float left, Vec4 right)
    {
        return right + left;
    }

    /// <summary>
    /// Adds 2 vectors.
    /// </summary>
    /// <param name="left">The vector to add.</param>
    /// <param name="right">The vector to add.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator +(Vec4 left, Vec4 right)
    {
        return new Vec4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
    }

    /// <summary>
    /// Subtracts a float from all components of a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The value to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator -(Vec4 left, float right)
    {
        return new Vec4(left.X - right, left.Y - right, left.Z - right, left.W - right);
    }

    /// <summary>
    /// Subtracts a vector from a float.
    /// </summary>
    /// <param name="left">The value to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator -(float left, Vec4 right)
    {
        return new Vec4(left - right.X, left - right.Y, left - right.Z, left - right.W);
    }

    /// <summary>
    /// Subtracts a vector from a vector.
    /// </summary>
    /// <param name="left">The vector to subtract from.</param>
    /// <param name="right">The vector to be subtracted.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator -(Vec4 left, Vec4 right)
    {
        return new Vec4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
    }

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The value to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator *(Vec4 left, float right)
    {
        return new Vec4(left.X * right, left.Y * right, left.Z * right, left.W * right);
    }

    /// <summary>
    /// Multiplies a vector with a float.
    /// </summary>
    /// <param name="left">The value to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator *(float left, Vec4 right)
    {
        return right * left;
    }

    /// <summary>
    /// Multiplies two vectors.
    /// </summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The vector to multiply.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator *(Vec4 left, Vec4 right)
    {
        return new Vec4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
    }

    /// <summary>
    /// Divides a vector with a float.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The value to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator /(Vec4 left, float right)
    {
        return new Vec4(left.X / right, left.Y / right, left.Z / right, left.W / right);
    }

    /// <summary>
    /// Divides a float with a vector.
    /// </summary>
    /// <param name="left">The value to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator /(float left, Vec4 right)
    {
        return new Vec4(left / right.X, left / right.Y, left / right.Z, left / right.W);
    }

    /// <summary>
    /// Divides a vector with a vector.
    /// </summary>
    /// <param name="left">The vector to be divided.</param>
    /// <param name="right">The vector to divide with.</param>
    /// <returns>Returns a new vector.</returns>
    public static Vec4 operator /(Vec4 left, Vec4 right)
    {
        return new Vec4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
    }

    /// <summary>
    /// Compares 2 vectors for equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator ==(Vec4 left, Vec4 right)
    {
        return left.X == right.X && left.Y == right.Y && left.Z == right.Z && left.W == right.W;
    }

    /// <summary>
    /// Compares 2 vectors for non-equal components.
    /// </summary>
    /// <param name="left">The vector to compare.</param>
    /// <param name="right">The vector to compare.</param>
    /// <returns>Returns a boolean.</returns>
    public static bool operator !=(Vec4 left, Vec4 right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Returns the absolute value of the vector.
    /// </summary>
    /// <returns>Returns the absolute value of the vector.</returns>
    public Vec4 Abs()
    {
        return new Vec4(MathF.Abs(X), MathF.Abs(Y), MathF.Abs(Z), MathF.Abs(W));
    }

    /// <summary>
    /// Returns the arccosine of the value.
    /// </summary>
    /// <returns>Returns the angle whose trigonometric cosine is value. The range of values returned by acos is [0,π]. The result is undefined if |value|>1.</returns>
    public Vec4 Acos()
    {
        return new Vec4(MathF.Acos(X), MathF.Acos(Y), MathF.Acos(Z), MathF.Acos(W));
    }

    /// <summary>
    /// Returns the arcsine of the value.
    /// </summary>
    /// <returns>Returns the angle whose trigonometric sine is value. The range of values returned by Asin is [−π/2,π/2]. The result is undefined if |value|≺1.</returns>
    public Vec4 Asin()
    {
        return new Vec4(MathF.Asin(X), MathF.Asin(Y), MathF.Asin(Z), MathF.Asin(W));
    }

    /// <summary>
    /// Returns the arc-tangent of the value.
    /// </summary>
    /// <returns>Returns the angle whose tangent is the value. The value returned is in the range [−π/2,π/2].</returns>
    public Vec4 Atan()
    {
        return new Vec4(MathF.Atan(X), MathF.Atan(Y), MathF.Atan(Z), MathF.Atan(W));
    }

    /// <summary>
    /// Finds the nearest integer that is greater than or equal to the value.
    /// </summary>
    /// <returns>Returns a value equal to the nearest integer that is greater than or equal to the value.</returns>
    public Vec4 Ceil()
    {
        return new Vec4(MathF.Ceiling(X), MathF.Ceiling(Y), MathF.Ceiling(Z), MathF.Ceiling(W));
    }

    /// <summary>
    /// Returns the cosine of the value.
    /// </summary>
    /// <returns>Returns the trigonometric cosine of the value.</returns>
    public Vec4 Cos()
    {
        return new Vec4(MathF.Cos(X), MathF.Cos(Y), MathF.Cos(Z), MathF.Cos(W));
    }

    /// <summary>
    /// Converts a quantity in radians to degrees.
    /// </summary>
    /// <returns>Returns a quantity specified in radians converted into degrees. The return value is (180×value)/π.</returns>
    public Vec4 Degrees()
    {
        return (this * 180) / MathF.PI;
    }

    /// <summary>
    /// Returns the natural exponentiation of the value.
    /// </summary>
    /// <returns>Returns the natural exponentiation of the value. i.e., e^this.</returns>
    public Vec4 Exp()
    {
        return Pow(new Vec4(MathF.E), this);
    }

    /// <summary>
    /// Returns 2 raised to the power of the value.
    /// </summary>
    /// <returns>Returns 2 raised to the power of the value. i.e., 2^this.</returns>
    public Vec4 Exp2()
    {
        return Pow(new Vec4(2), this);
    }

    /// <summary>
    /// Finds the nearest integer less than or equal to the value.
    /// </summary>
    /// <returns>Returns a value equal to the nearest integer that is less than or equal to the value.</returns>
    public Vec4 Floor()
    {
        return new Vec4(MathF.Floor(X), MathF.Floor(Y), MathF.Floor(Z), MathF.Floor(W));
    }

    /// <summary>
    /// Computes the fractional part of the value.
    /// </summary>
    /// <returns>Returns the fractional part of the value. This is calculated as 'this - Floor()'.</returns>
    public Vec4 Fract()
    {
        return this - Floor();
    }

    /// <summary>
    /// Returns the inverse of the square root of the value.
    /// </summary>
    /// <returns>Returns the inverse of the square root of the value; i.e. 1 / Sqrt(). The result is undefined if ≤0.</returns>
    public Vec4 Inversesqrt()
    {
        return 1 / Sqrt();
    }

    /// <summary>
    /// Calculates the length of the vector.
    /// </summary>
    /// <returns>Returns the length of the vector, i.e. '√(X² + Y² + ...)'.</returns>
    public float Length()
    {
        return MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
    }

    /// <summary>
    /// Returns the natural logarithm of the value.
    /// </summary>
    /// <returns>Returns the natural logarithm of the value, i.e. the value y which satisfies value=e^y. The result is undefined if ≤0.</returns>
    public Vec4 Log()
    {
        return new Vec4(MathF.Log(X), MathF.Log(Y), MathF.Log(Z), MathF.Log(W));
    }

    /// <summary>
    /// Returns the base 2 logarithm of the value.
    /// </summary>
    /// <returns>Returns the base 2 logarithm of the value, i.e. the value y which satisfies value=2^y. The result is undefined if ≤0.</returns>
    public Vec4 Log2()
    {
        return new Vec4(MathF.Log2(X), MathF.Log2(Y), MathF.Log2(Z), MathF.Log2(W));
    }

    /// <summary>
    /// Calculates the unit vector in the same direction as the original vector.
    /// </summary>
    /// <returns>Returns a vector with the same direction as the original vector, but with length 1.</returns>
    public Vec4 Normalize()
    {
        return this / Length();
    }

    /// <summary>
    /// Converts a quantity in degrees to radians.
    /// </summary>
    /// <returns>Returns a quantity specified in degrees converted into radians. The return value is (π×value)/180.</returns>
    public Vec4 Radians()
    {
        return (this * MathF.PI) / 180;
    }

    /// <summary>
    /// Extracts the sign of the value.
    /// </summary>
    /// <returns>Returns -1.0 if the value is less than 0.0, 0.0 if the value is equal to 0.0, and +1.0 if the value is greater than 0.</returns>
    public Vec4 Sign()
    {
        return new Vec4(MathF.Sign(X), MathF.Sign(Y), MathF.Sign(Z), MathF.Sign(W));
    }

    /// <summary>
    /// Returns the sine of the value.
    /// </summary>
    /// <returns>Returns the trigonometric sine of the value.</returns>
    public Vec4 Sin()
    {
        return new Vec4(MathF.Sin(X), MathF.Sin(Y), MathF.Sin(Z), MathF.Sin(W));
    }

    /// <summary>
    /// Returns the square root of the value.
    /// </summary>
    /// <returns>Returns the square root of the value. The result is undefined if ≺0.</returns>
    public Vec4 Sqrt()
    {
        return new Vec4(MathF.Sqrt(X), MathF.Sqrt(Y), MathF.Sqrt(Z), MathF.Sqrt(W));
    }

    /// <summary>
    /// Returns the tangent of the value.
    /// </summary>
    /// <returns>Returns the trigonometric tangent of the value.</returns>
    public Vec4 Tan()
    {
        return new Vec4(MathF.Tan(X), MathF.Tan(Y), MathF.Tan(Z), MathF.Tan(W));
    }

    /// <summary>
    /// Returns the arc-tangent of the parameters.
    /// </summary>
    /// <param name="left">Specify the numerator of the fraction whose arctangent to return.</param>
    /// <param name="right">Specify the denominator of the fraction whose arctangent to return.</param>
    /// <returns>Returns the angle whose trigonometric arctangent is left/right. The signs of left and right are used to determine the quadrant that the angle lies in. The value returned by Atan is in the range [−π,π]. The result is undefined if right=0.</returns>
    public static Vec4 Atan(Vec4 left, Vec4 right)
    {
        return new Vec4(MathF.Atan(left.X / right.X), MathF.Atan(left.Y / right.Y), MathF.Atan(left.Z / right.Z),
            MathF.Atan(left.W / right.W));
    }

    /// <summary>
    /// Constrains a value to lie between two further values.
    /// </summary>
    /// <param name="val">Specify the value to constrain.</param>
    /// <param name="min">Specify the lower end of the range into which to constrain val.</param>
    /// <param name="max">Specify the upper end of the range into which to constrain val.</param>
    /// <returns>Returns the value of val constrained to the range min to max. The returned value is computed as Min(Max(val, min), max).</returns>
    public static Vec4 Clamp(Vec4 val, float min, float max)
    {
        return new Vec4(MathF.Min(MathF.Max(val.X, min), max),
            MathF.Min(MathF.Max(val.Y, min), max),
            MathF.Min(MathF.Max(val.Z, min), max),
            MathF.Min(MathF.Max(val.W, min), max));
    }

    /// <inheritdoc cref="Clamp(JAngine.Mathematics.Vec4,float,float)"/>
    public static Vec4 Clamp(Vec4 val, Vec4 min, Vec4 max)
    {
        return new Vec4(MathF.Min(MathF.Max(val.X, min.X), max.X),
            MathF.Min(MathF.Max(val.Y, min.Y), max.Y),
            MathF.Min(MathF.Max(val.Z, min.Z), max.Z),
            MathF.Min(MathF.Max(val.W, min.W), max.W));
    }

    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="p0">Specifies the first of two points.</param>
    /// <param name="p1">Specifies the second of two points.</param>
    /// <returns>Returns the distance between the two points p0 and p1. i.e., 'Length(p0 - p1)'.</returns>
    public static float Distance(Vec4 p0, Vec4 p1)
    {
        Vec4 vector = p0 - p1;

        return vector.Length();
    }

    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="left">Specifies the first of two vectors.</param>
    /// <param name="right">Specifies the second of two vectors.</param>
    /// <returns>Returns the dot product of two vectors, left and right. i.e., 'left.X * right.X + left.Y * right.Y, + ...'.</returns>
    public static float Dot(Vec4 left, Vec4 right)
    {
        return left.X * right.X +
               left.Y * right.Y +
               left.Z * right.Z +
               left.W * right.W;
    }

    /// <summary>
    /// Return a vector pointing in the same direction as another.
    /// </summary>
    /// <param name="n">Specifies the vector to orient.</param>
    /// <param name="i">Specifies the incident vector.</param>
    /// <param name="nRef">Specifies the reference vector.</param>
    /// <returns>Returns a vector that points away from a surface as defined by its normal. If Dot(nRef, i) ≺ 0 faceforward returns N, otherwise it returns -N.</returns>
    public static Vec4 Faceforward(Vec4 n, Vec4 i, Vec4 nRef)
    {
        if (Vec4.Dot(i, nRef) < 0)
            return n;

        return n * -1;
    }

    /// <summary>
    /// Returns the greater of two values.
    /// </summary>
    /// <param name="left">Specify the first value to compare.</param>
    /// <param name="right">Specify the second value to compare.</param>
    /// <returns>Returns the maximum of the two parameters. It returns right if right is greater than left, otherwise it returns left.</returns>
    public static Vec4 Max(Vec4 left, float right)
    {
        return new Vec4(MathF.Max(left.X, right),
            MathF.Max(left.Y, right),
            MathF.Max(left.Z, right),
            MathF.Max(left.W, right));
    }

    ///<inheritdoc cref="Max(JAngine.Mathematics.Vec4,float)"/>
    public static Vec4 Max(Vec4 left, Vec4 right)
    {
        return new Vec4(MathF.Max(left.X, right.X),
            MathF.Max(left.Y, right.Y),
            MathF.Max(left.Z, right.Z),
            MathF.Max(left.W, right.W));
    }

    /// <summary>
    /// Returns the lesser of two values.
    /// </summary>
    /// <param name="left">Specify the first value to compare.</param>
    /// <param name="right">Specify the second value to compare.</param>
    /// <returns>Returns the minimum of the two parameters. It returns right if right is less than left, otherwise it returns left.</returns>
    public static Vec4 Min(Vec4 left, float right)
    {
        return new Vec4(MathF.Min(left.X, right),
            MathF.Min(left.Y, right),
            MathF.Min(left.Z, right),
            MathF.Min(left.W, right));
    }

    /// <inheritdoc cref="Min(JAngine.Mathematics.Vec4,float)"/>
    public static Vec4 Min(Vec4 left, Vec4 right)
    {
        return new Vec4(MathF.Min(left.X, right.X),
            MathF.Min(left.Y, right.Y),
            MathF.Min(left.Z, right.Z),
            MathF.Min(left.W, right.W));
    }

    /// <summary>
    /// Returns the component of the first parameter raised to the power of the second.
    /// </summary>
    /// <param name="left">Specify the value to raise to the power of right.</param>
    /// <param name="right">Specify the power to which to raise left.</param>
    /// <returns>Returns the value of left raised to the right power, i.e. left^right. The result is undefined if left≺0 or if left=0 and right≤0.</returns>
    public static Vec4 Pow(Vec4 left, Vec4 right)
    {
        return new Vec4(MathF.Pow(left.X, right.X),
            MathF.Pow(left.Y, right.Y),
            MathF.Pow(left.Z, right.Z),
            MathF.Pow(left.W, right.W));
    }

    /// <summary>
    /// Calculates the reflection direction for an incident vector.
    /// </summary>
    /// <param name="i">Specifies the incident vector.</param>
    /// <param name="n">Specifies the normal vector.</param>
    /// <returns>For a given incident vector I and surface normal N reflect returns the reflection direction calculated as 'i - 2 * Dot(n, i) * n'.</returns>
    public static Vec4 Reflect(Vec4 i, Vec4 n)
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
    public static Vec4 Smoothstep(float edge0, float edge1, Vec4 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }

    /// <inheritdoc cref="Smoothstep(float,float,JAngine.Mathematics.Vec4)"/>
    public static Vec4 Smoothstep(Vec4 edge0, Vec4 edge1, Vec4 src)
    {
        var t = Clamp((src - edge0) / (edge1 - edge0), 0, 1);
        return t * t * (3 - 2 * t);
    }
}
