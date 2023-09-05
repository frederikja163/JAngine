namespace JAngine.Rendering.OpenGL;

internal static unsafe class Gl
{
    [Flags]
    internal enum ClearBufferMask : int
    {
        ColorBuffer = 0x00004000,
        DepthBuffer = 0x00000100,
        StencilBuffer = 0x00000400,
    }
    
    internal static readonly delegate* unmanaged<float, float, float, float, void> ClearColor =
        (delegate* unmanaged<float, float, float, float, void>)Glfw.GetProcAddress("glClearColor");
    internal static readonly delegate* unmanaged<ClearBufferMask, void> Clear =
        (delegate* unmanaged<ClearBufferMask, void>)Glfw.GetProcAddress("glClear");
}