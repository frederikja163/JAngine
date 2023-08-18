using System.Numerics;
using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

public static unsafe class Renderer
{
    public static void ClearColor(float r, float g, float b, float a) => Gl.ClearColor(r, g, b, a);

    internal static void Clear() => Gl.Clear(Gl.ClearBufferMask.ColorBuffer);
}