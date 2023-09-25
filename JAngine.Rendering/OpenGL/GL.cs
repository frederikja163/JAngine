using System.Runtime.InteropServices;

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

    internal enum DrawElementsType : Bitfield
    {
        UnsignedByte = 0x1401,
        UnsignedShort = 0x1403,
        UnsignedInt = 0x1405,
    }

    internal enum PrimitiveType : Bitfield
    {
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        // More types exist here, but those should never be used.
    }

    internal enum BufferUsage : Bitfield
    {
        StreamDraw = 0x88E0,
        StreamRead = 0x88E1,
        StreamCopy = 0x88E2,
        StaticDraw = 0x88E4,
        StaticRead = 0x88E5,
        StaticCopy = 0x88E6,
        DynamicDraw = 0x88E8,
        DynamicRead = 0x88E9,
        DynamicCopy = 0x88EA,
    }

    internal enum ProgramProperty : Bitfield
    {
        DeleteStatus = 0x8B80,
        LinkStatus = 0x8B82,
        ValidateStatus = 0x8B83,
        InfoLogLength = 0x8B84,
        AttachedShaders = 0x8B85,
        ActiveAtomicCounterBuffers = 0x92D9,
        ActiveAttributes = 0x8B89,
        ActiveAttributeMaxLength = 0x8B8A,
        ActiveUniforms = 0x8B86,
        ActiveUniformMaxLength = 0x8B87,
        ProgramBinaryLength = 0x8741,
        ComputeWorkGroupSize = 0x8267,
        TransformFeedbackMode = 0x8C7F,
        TransformFeedbackVaryings = 0x8C83,
        TransformFeedbackVaryingMaxLength = 0x8C76,
        GeometryVerticesOut = 0x8916,
        GeometryInputType = 0x8917,
        GeometryOutputType = 0x8918,
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

    internal enum ShaderParameterName : Bitfield
    {
        ShaderType = 0x8B4F,
        DeleteStatus = 0x8B80,
        CompileStatus = 0x8B81,
        InfoLogLength = 0x8B84,
        ShaderSourceLength = 0x8B84,
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
    
    private static readonly delegate* unmanaged<float, float, float, float, void> ClearColorPtr =
        (delegate* unmanaged<float, float, float, float, void>)Glfw.GetProcAddress("glClearColor");
    private static readonly delegate* unmanaged<ClearBufferMask, void> ClearPtr =
        (delegate* unmanaged<ClearBufferMask, void>)Glfw.GetProcAddress("glClear");
    private static readonly delegate* unmanaged<PrimitiveType, SizeI, DrawElementsType, void*, SizeI, void> DrawElementsInstancedPtr =
        (delegate* unmanaged<PrimitiveType, SizeI, DrawElementsType, void*, SizeI, void>)Glfw.GetProcAddress("glDrawElementsInstanced");

    private static readonly delegate* unmanaged<Uint> CreateProgramPtr =
        (delegate* unmanaged<Uint>)Glfw.GetProcAddress("glCreateProgram");
    private static readonly delegate* unmanaged<Uint, void> DeleteProgramPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glDeleteProgram");
    private static readonly delegate* unmanaged<Uint, void> UseProgramPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glUseProgram");
    private static readonly delegate* unmanaged<Uint, Uint, void> AttachShaderPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glAttachShader");
    private static readonly delegate* unmanaged<Uint, Uint, void> DetachShaderPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glDetachShader");
    private static readonly delegate* unmanaged<Uint, void> LinkProgramPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glLinkProgram");
    private static readonly delegate* unmanaged<Uint, ProgramProperty, Int*, void> GetProgramivPtr =
        (delegate* unmanaged<Uint, ProgramProperty, Int*, void>)Glfw.GetProcAddress("glGetProgramiv");
    private static readonly delegate* unmanaged<Uint, SizeI, SizeI*, Char*, void> GetProgramInfoLogPtr =
        (delegate* unmanaged<Uint, SizeI, SizeI*, Char*, void>)Glfw.GetProcAddress("glGetProgramInfoLog");
    
    private static readonly delegate* unmanaged<ShaderType, Uint> CreateShaderPtr =
        (delegate* unmanaged<ShaderType, Uint>)Glfw.GetProcAddress("glCreateShader");
    private static readonly delegate* unmanaged<Uint, void> DeleteShaderPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glDeleteShader");
    private static readonly delegate* unmanaged<Uint, SizeI, Char**, Int*, void> ShaderSourcePtr =
        (delegate* unmanaged<Uint, SizeI, Char**, Int*, void>)Glfw.GetProcAddress("glShaderSource");
    private static readonly delegate* unmanaged<Uint, void> CompilerShaderPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glCompileShader");
    private static readonly delegate* unmanaged<Uint, ShaderParameterName, Int*, void> GetShaderivPtr =
        (delegate* unmanaged<Uint, ShaderParameterName, Int*, void>)Glfw.GetProcAddress("glGetShaderiv");
    private static readonly delegate* unmanaged<Uint, SizeI, SizeI*, Char*, void> GetShaderInfoLogPtr =
        (delegate* unmanaged<Uint, SizeI, SizeI*, Char*, void>)Glfw.GetProcAddress("glGetShaderInfoLog");
    
    private static readonly delegate* unmanaged<SizeI, Uint*, void> CreateBuffersPtr =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glCreateBuffers");
    private static readonly delegate* unmanaged<SizeI, Uint*, void> DeleteBuffersPtr =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glDeleteBuffers");
    private static readonly delegate* unmanaged<Uint, SizeIPtr, void*, BufferStorageMask, void> NamedBufferStoragePtr =
        (delegate* unmanaged<Uint, SizeIPtr, void*, BufferStorageMask, void>)Glfw.GetProcAddress("glNamedBufferStorage");
    private static readonly delegate* unmanaged<Uint, SizeIPtr, void*, BufferUsage, void> NamedBufferDataPtr =
        (delegate* unmanaged<Uint, SizeIPtr, void*, BufferUsage, void>)Glfw.GetProcAddress("glNamedBufferData");
    private static readonly delegate* unmanaged<Uint, IntPtr, SizeIPtr, void*, void> NamedBufferSubDataPtr =
        (delegate* unmanaged<Uint, IntPtr, SizeIPtr, void*, void>)Glfw.GetProcAddress("glNamedBufferSubData");
    
    private static readonly delegate* unmanaged<SizeI, Uint*, void> CreateVertexArraysPtr =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glCreateVertexArrays");
    private static readonly delegate* unmanaged<SizeI, Uint*, void> DeleteVertexArraysPtr =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glDeleteVertexArrays");
    private static readonly delegate* unmanaged<Uint, void> BindVertexArrayPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glBindVertexArray");
    private static readonly delegate* unmanaged<Uint, Uint, void> VertexArrayElementBufferPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glVertexArrayElementBuffer");
    private static readonly delegate* unmanaged<Uint, Uint, Uint, IntPtr, SizeI, void> VertexArrayVertexBufferPtr =
        (delegate* unmanaged<Uint, Uint, Uint, IntPtr, SizeI, void>)Glfw.GetProcAddress("glVertexArrayVertexBuffer");
    private static readonly delegate* unmanaged<Uint, Uint, Uint, void> VertexArrayAttribBindingPtr =
        (delegate* unmanaged<Uint, Uint, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribBinding");
    private static readonly delegate* unmanaged<Uint, Uint, void> EnableVertexArrayAttribPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glEnableVertexArrayAttrib");
    private static readonly delegate* unmanaged<Uint, Uint, void> DisableVertexArrayAttribPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glDisableVertexArrayAttrib");
    private static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Boolean, Uint, void> VertexArrayAttribFormatPtr =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Boolean, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribFormat");
    private static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void> VertexArrayAttribIFormatPtr =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribIFormat");
    private static readonly delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void> VertexArrayAttribLFormatPtr =
        (delegate* unmanaged<Uint, Uint, Int, VertexAttribType, Uint, void>)Glfw.GetProcAddress("glVertexArrayAttribLFormat");

    internal static void ClearColor(float r, float g, float b, float a)
    {
        ClearColorPtr(r, g, b, a);
    }

    internal static void Clear(ClearBufferMask clearMask)
    {
        ClearPtr(clearMask);
    }

    internal static void DrawElementsInstanced(PrimitiveType mode, SizeI count, DrawElementsType type, uint offset,
        int instanceCount)
    {
        DrawElementsInstancedPtr(mode, count, type, (void*)offset, instanceCount);
    }
    
    internal static uint CreateProgram()
    {
        return CreateProgramPtr();
    }

    internal static void DeleteProgram(uint program)
    {
        DeleteProgramPtr(program);
    }

    internal static void UseProgram(uint program)
    {
        UseProgramPtr(program);
    }
    
    internal static void AttachShader(uint program, uint shader)
    {
        AttachShaderPtr(program, shader);
    }

    internal static void DetachShader(uint program, uint shader)
    {
        DetachShaderPtr(program, shader);
    }

    internal static void LinkProgram(uint program)
    {
        LinkProgramPtr(program);
    }

    internal static void GetProgram(uint program, ProgramProperty pname, out int value)
    {
        int paramValue = 0;
        GetProgramivPtr(program, pname, &paramValue);
        value = paramValue;
    }
    
    internal static void GetProgramInfoLog(uint program, int maxLength, out string infoLog)
    {
        nint infoLogPtr =  Marshal.AllocCoTaskMem(maxLength);
        int length = 0;
        GetProgramInfoLogPtr(program, maxLength, &length, (byte*)infoLogPtr);
        infoLog = Marshal.PtrToStringAnsi(infoLogPtr, length);
        Marshal.FreeCoTaskMem(infoLogPtr);
    }

    internal static uint CreateShader(ShaderType shaderType)
    {
        return CreateShaderPtr(shaderType);
    }

    internal static void DeleteShader(uint shader)
    {
        DeleteShaderPtr(shader);
    }

    internal static void ShaderSource(uint shader, string shaderSource)
    {
        nint shaderSourcePtr = Marshal.StringToCoTaskMemAnsi(shaderSource);
        int length = shaderSource.Length;
        ShaderSourcePtr(shader, 1, (Char**)&shaderSourcePtr, &length);
        Marshal.FreeCoTaskMem(shaderSourcePtr);
    }

    internal static void CompileShader(uint shader)
    {
        CompilerShaderPtr(shader);
    }

    internal static void GetShader(uint shader, ShaderParameterName pname, out int value)
    {
        int paramValue = 0;
        GetShaderivPtr(shader, pname, &paramValue);
        value = paramValue;
    }

    internal static void GetShaderInfoLog(uint shader, int maxLength, out string infoLog)
    {
        nint infoLogPtr =  Marshal.AllocCoTaskMem(maxLength);
        int length = 0;
        GetShaderInfoLogPtr(shader, maxLength, &length, (byte*)infoLogPtr);
        infoLog = Marshal.PtrToStringAnsi(infoLogPtr, length);
        Marshal.FreeCoTaskMem(infoLogPtr);
    }
    
    internal static uint CreateBuffer()
    {
        uint buffer = 0;
        CreateBuffersPtr(1, &buffer);
        return buffer;
    }
    
    internal static void DeleteBuffer(uint buffer)
    {
        DeleteBuffersPtr(1, &buffer);
    }

    internal static void NamedBufferStorage<T>(uint buffer, ReadOnlySpan<T> data, BufferStorageMask flags)
        where T : unmanaged
    {
        fixed (T* dataPtr = &data.GetPinnableReference())
        {
            NamedBufferStoragePtr(buffer, data.Length * sizeof(T), dataPtr, flags);
        }
    }
    
    internal static void NamedBufferStorage(uint buffer, int length, BufferStorageMask flags)
    {
        NamedBufferStoragePtr(buffer, length, (void*)IntPtr.Zero, flags);
    }
    
    internal static void NamedBufferData<T>(uint buffer, ReadOnlySpan<T> data, BufferUsage flags)
        where T : unmanaged
    {
        fixed (T* dataPtr = &data.GetPinnableReference())
        {
            NamedBufferDataPtr(buffer, data.Length * sizeof(T), dataPtr, flags);
        }
    }
    
    internal static void NamedBufferData(uint buffer, int length, BufferUsage flags)
    {
        NamedBufferDataPtr(buffer, length, (void*)IntPtr.Zero, flags);
    }

    internal static void NamedBufferSubData<T>(uint buffer, nint offset, nint size, ReadOnlySpan<T> data)
        where T : unmanaged
    {
        fixed (T* dataPtr = &data.GetPinnableReference())
        {
            NamedBufferSubDataPtr(buffer, offset, size, dataPtr);
        }
    }

    internal static uint CreateVertexArray()
    {
        uint vertexArray = 0;
        CreateVertexArraysPtr(1, &vertexArray);
        return vertexArray;
    }

    internal static void DeleteVertexArray(uint vertexArray)
    {
        DeleteVertexArraysPtr(1, &vertexArray);
    }
    
    internal static void BindVertexArray(uint vao)
    {
        BindVertexArrayPtr(vao);
    }

    internal static void VertexArrayElementBuffer(uint vao, uint buffer)
    {
        VertexArrayElementBufferPtr(vao, buffer);
    }

    internal static void VertexArrayVertexBuffer(uint vao, uint bindingIndex, uint buffer, nint offset, int stride)
    {
        VertexArrayVertexBufferPtr(vao, bindingIndex, buffer, offset, stride);
    }

    internal static void VertexArrayAttribBinding(uint vao, uint attribIndex, uint bindingIndex)
    {
        VertexArrayAttribBindingPtr(vao, attribIndex, bindingIndex);
    }

    internal static void EnableVertexArrayAttrib(uint vao, uint index)
    {
        EnableVertexArrayAttribPtr(vao, index);
    }
    
    internal static void DisableVertexArrayAttrib(uint vao, uint index)
    {
        DisableVertexArrayAttribPtr(vao, index);
    }

    internal static void VertexArrayAttribFormat(uint vao, uint attribIndex, int size, VertexAttribType type,
        bool normalized, uint relativeOffset)
    {
        VertexArrayAttribFormatPtr(vao, attribIndex, size, type, normalized, relativeOffset);
    }
    
    internal static void VertexArrayAttribIFormat(uint vao, uint attribIndex, int size, VertexAttribType type, uint relativeOffset)
    {
        VertexArrayAttribIFormatPtr(vao, attribIndex, size, type, relativeOffset);
    }
    
    internal static void VertexArrayAttribLFormat(uint vao, uint attribIndex, int size, VertexAttribType type, uint relativeOffset)
    {
        VertexArrayAttribLFormatPtr(vao, attribIndex, size, type, relativeOffset);
    }
}