namespace JAngine.Rendering.OpenGL;
// https://raw.githubusercontent.com/KhronosGroup/OpenGL-Registry/main/xml/gl.xml
using Enum = System.UInt32;
using Boolean = System.Boolean;
using Bitfield = System.UInt32;
using Byte = System.SByte;
using UByte = System.Byte;
using Short = System.Int16;
using UShort = System.UInt16;
using Int = System.Int32;
using Uint = System.UInt32;
using ClampX = System.Int32;
using SizeI = System.Int32;
using Float = System.Single;
using ClampF = System.Single;
using Double = System.Double;
using ClampD = System.Double;
using Char = System.Byte;
using Half = System.UInt16;
using Fixed = System.Int32;
using IntPtr = System.IntPtr;
using SizeIPtr = System.IntPtr;
using Int64 = System.Int64;
using Uint64 = System.UInt64;
// Sync
// Context
// Event
// DebugProc

internal static unsafe class Gl
{
    [Flags]
    internal enum ClearBufferMask : Bitfield
    {
        ColorBuffer = 0x00004000,
        DepthBuffer = 0x00000100,
        StencilBuffer = 0x00000400,
    }

    internal enum BufferStorageMask : Bitfield
    {
        DynamicStorageBit = 0x100,
        MapReadBit = 0x0001,
        MapWriteBit = 0x0002,
        MapPersistentBit = 0x0040,
        MapCoherentBit = 0x0080,
        ClientStorageBit = 0x200,
    }

    internal enum ShaderType : Bitfield
    {
        ComputeShader = 0x91B9,
        VertexShader = 0x8B31,
        TessControlShader = 0x8E88,
        TessEvaluationShader = 0x8E87,
        GeometryShader = 0x8DD9,
        FragmentShader = 0x8B30,
    }

    internal enum VertexAttribType : Enum
    {
        Byte = 0x1400,
        Short = 0x1402,
        Int = 0x1404,
        Fixed = 0x140C,
        Float = 0x1406,
        HalfFloat = 0x140B,
        Double = 0x140A,
        UnsignedByte = 0x1401,
        UnsignedShort = 0x1403,
        UnsignedInt = 0x1405,
        // GL_INT_2_10_10_10_REV
        // GL_UNSIGNED_INT_2_10_10_10_REV
        // GL_UNSIGNED_INT_10F_11F_11F_REV
    }
    
    internal static readonly delegate* unmanaged<float, float, float, float, void> ClearColor =
        (delegate* unmanaged<float, float, float, float, void>)Glfw.GetProcAddress("glClearColor");
    internal static readonly delegate* unmanaged<ClearBufferMask, void> Clear =
        (delegate* unmanaged<ClearBufferMask, void>)Glfw.GetProcAddress("glClear");

    internal static readonly delegate* unmanaged<Uint> CreateProgram =
        (delegate* unmanaged<Uint>)Glfw.GetProcAddress("glCreateProgram");
    internal static readonly delegate* unmanaged<Uint, void> DeleteProgram =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glDeleteProgram");
    internal static readonly delegate* unmanaged<ShaderType, Int> CreateShader =
        (delegate* unmanaged<ShaderType, Int>)Glfw.GetProcAddress("glCreateShader");
    internal static readonly delegate* unmanaged<Uint, void> DeleteShader =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glDeleteShader");
    internal static readonly delegate* unmanaged<Int, SizeI, Char**, Int*> ShaderSource =
        (delegate* unmanaged<Int, SizeI, Char**, Int*>)Glfw.GetProcAddress("glShaderSource");
    internal static readonly delegate* unmanaged<Int, void> CompilerShader =
        (delegate* unmanaged<Int, void>)Glfw.GetProcAddress("glCompileShader");
    internal static readonly delegate* unmanaged<Uint, SizeI, SizeI*, Char*> GetShaderInfoLog =
        (delegate* unmanaged<Uint, SizeI, SizeI*, Char*>)Glfw.GetProcAddress("glGetShaderInfoLog");
    internal static readonly delegate* unmanaged<Uint, Uint, void> AttachShader =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glAttachShader");
    internal static readonly delegate* unmanaged<Uint, void> LinkProgram =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glLinkProgram");
    internal static readonly delegate* unmanaged<Uint, Uint, void> DetachShader =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glDetachShader");
    
    internal static readonly delegate* unmanaged<SizeI, Uint*, void> CreateBuffers =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glCreateBuffers");
    internal static readonly delegate* unmanaged<SizeI, Uint*, void> DeleteBuffers =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glDeleteBuffers");
    internal static readonly delegate* unmanaged<Uint, SizeIPtr, void*, Bitfield> BufferStorage =
        (delegate* unmanaged<Uint, SizeIPtr, void*, Bitfield>)Glfw.GetProcAddress("glBufferStorage");
    
    internal static readonly delegate* unmanaged<SizeI, Uint*, void> CreateVertexArrays =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glCreateVertexArrays");
    internal static readonly delegate* unmanaged<SizeI, Uint*, void> DeleteVertexArrays =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glDeleteVertexArrays");
    internal static readonly delegate* unmanaged<SizeI, Uint, void> VertexArrayElementBuffer =
        (delegate* unmanaged<SizeI, Uint, void>)Glfw.GetProcAddress("glVertexArrayElementBuffer");
    internal static readonly delegate* unmanaged<Uint, Uint, Uint, IntPtr, SizeI, void> VertexArrayVertexBuffer =
        (delegate* unmanaged<Uint, Uint, Uint, IntPtr, SizeI, void>)Glfw.GetProcAddress("glVertexArrayVertexBuffer");
    internal static readonly delegate* unmanaged<Uint, Uint, Uint, void> VertexArrayAttribBinding =
        (delegate* unmanaged<Uint, Uint, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribBinding");
    internal static readonly delegate* unmanaged<Uint, void> EnableVertexArrayAttrib =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glEnableVertexArrayAttrib");
    internal static readonly delegate* unmanaged<Uint, void> DisableVertexArrayAttrib =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glDisableVertexArrayAttrib");
    internal static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Boolean, Uint, void> VertexArrayAttribFormat =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Boolean, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribFormat");
    internal static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void> VertexArrayAttribIFormat =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribIFormat");
    internal static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void> VertexArrayAttribLFormat =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribLFormat");
}