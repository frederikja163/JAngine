using System.Buffers;
using System.Runtime.InteropServices;

namespace JAngine.Rendering.OpenGL;
// https://raw.githubusercontent.com/KhronosGroup/OpenGL-Registry/main/xml/gl.xml
using Enum = System.UInt32;
using Boolean = System.Boolean;
using Bitfield = System.UInt32;
using Int = System.Int32;
using Uint = System.UInt32;
using SizeI = System.Int32;
using Char = System.Byte;
using IntPtr = System.IntPtr;
using SizeIPtr = System.IntPtr;

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

    internal enum EnableCap : Bitfield
    {
        Blend = 0x0BE2,
        ClipDistance0 = 0x3000,
        ClipDistance1 = 0x3001,
        ClipDistance2 = 0x3002,
        ClipDistance3 = 0x3003,
        ClipDistance4 = 0x3004,
        ClipDistance5 = 0x3005,
        ClipDistance6 = 0x3006,
        ClipDistance7 = 0x3007,
        ColorLogicOp = 0x0BF2,
        CullFace = 0x0B44,
        DebugOutput = 0x92E0,
        DebugOutputSynchronous = 0x8242,
        DepthClamp = 0x864F,
        DepthTest = 0x0B71,
        Dither = 0x0BD0,
        FramebufferSrgb = 0x8DB9,
        LineSmooth = 0x0B20,
        Multisample = 0x809D,
        PolygonOffsetFill = 0x8037,
        PolygonOffsetLine = 0x2A02,
        PolygonOffsetPoint = 0x2A01,
        PolygonSmooth = 0x0B41,
        PrimitiveRestart = 0x8F9D,
        PrimitiveRestartFixedIndex = 0x8D69,
        RasterizerDiscard = 0x8C89,
        SampleAlphaToCoverage = 0x809E,
        SampleAlphaToOne = 0x809F,
        SampleCoverage = 0x80A0,
        SampleShading = 0x8C36,
        SampleMask = 0x8E51,
        ScissorTest = 0x0C11,
        StencilTest = 0x0B90,
        TextureCubeMapSeamless = 0x884F,
        ProgramPointSize = 0x8642,
    }

    internal enum BlendFactor : Enum
    {
        Zero = 0,
        One = 1,
        SrcColor = 0x0300,
        OneMinusSrcColor = 0x0301,
        DstColor = 0x0306,
        OneMinusDstColor = 0x0307,
        SrcAlpha = 0x0302,
        OneMinusSrcAlpha = 0x0303,
        DstAlpha = 0x0304,
        OneMinusDstAlpha = 0x0305,
        ConstantColor = 0x8001,
        OneMinusConstantColor = 0x8002,
        ConstantAlpha = 0x8003,
        OneMinusConstantAlpha = 0x8004,
        SrcAlphaSaturate = 0x0308,
        Src1Color = 0x88F9,
        OneMinusSrc1Color = 0x88FA,
        Src1Alpha = 0x8589,
        OneMinusSrc1Alpha = 0x88FB,
    }
    
    internal enum PrimitiveType : Bitfield
    {
        Triangles = 0x0004,
        TriangleStrip = 0x0005,
        TriangleFan = 0x0006,
        // More types exist here, but those should never be used.
    }
    
    internal enum DrawElementsType : Bitfield
    {
        UnsignedByte = 0x1401,
        UnsignedShort = 0x1403,
        UnsignedInt = 0x1405,
    }

    internal enum ObjectIdentifier : Bitfield
    {
        Buffer = 0x82E0,
        Shader = 0x82E1,
        Program = 0x82E2,
        VertexArray = 0x8074,
        Query = 0x82E3,
        ProgramPipeline = 0x82E4,
        TransformFeedback = 0x8E22,
        Sampler = 0x82E6,
        Texture = 0x1702,
        RenderBuffer = 0x8D41,
        FrameBuffer = 0x8D40,
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

    internal enum AttributeType : Bitfield
    {
        Float = 0x1406,
        FloatVec2 = 0x8B50,
        FloatVec3 = 0x8B51,
        FloatVec4 = 0x8B52,
        FloatMat2 = 0x8B5A,
        FloatMat3 = 0x8B5B,
        FloatMat4 = 0x8B5C,
        FloatMat2X3 = 0x8B65,
        FloatMat2X4 = 0x8B66,
        FloatMat3X2 = 0x8B67,
        FloatMat3X4 = 0x8B68,
        FloatMat4X2 = 0x8B69,
        FloatMat4X3 = 0x8B6A,
        Int = 0x1404,
        IntVec2 = 0x8B53,
        IntVec3 = 0x8B54,
        IntVec4 = 0x8B55,
        UInt = 0x1405,
        UIntVec2 = 0x8DC6,
        UIntVec3 = 0x8DC7,
        UIntVec4 = 0x8DC8,
        Double = 0x140A,
        DoubleVec2 = 0x8FFC,
        DoubleVec3 = 0x8FFD,
        DoubleVec4 = 0x8FFE,
        DoubleMat2 = 0x8F46,
        DoubleMat3 = 0x8F47,
        DoubleMat4 = 0x8F48,
        DoubleMat2X3 = 0x8F49,
        DoubleMat2X4 = 0x8F4A,
        DoubleMat3X2 = 0x8F4B,
        DoubleMat3X4 = 0x8F4C,
        DoubleMat4X2 = 0x8F4D,
        DoubleMat4X3 = 0x8F4E,
    }
    
    internal enum UniformType : Bitfield
    {
        Float = 0x1406,
        FloatVec2 = 0x8B50,
        FloatVec3 = 0x8B51,
        FloatVec4 = 0x8B52,
        FloatMat2 = 0x8B5A,
        FloatMat3 = 0x8B5B,
        FloatMat4 = 0x8B5C,
        FloatMat2X3 = 0x8B65,
        FloatMat2X4 = 0x8B66,
        FloatMat3X2 = 0x8B67,
        FloatMat3X4 = 0x8B68,
        FloatMat4X2 = 0x8B69,
        FloatMat4X3 = 0x8B6A,
        Int = 0x1404,
        IntVec2 = 0x8B53,
        IntVec3 = 0x8B54,
        IntVec4 = 0x8B55,
        UInt = 0x1405,
        UIntVec2 = 0x8DC6,
        UIntVec3 = 0x8DC7,
        UIntVec4 = 0x8DC8,
        Double = 0x140A,
        DoubleVec2 = 0x8FFC,
        DoubleVec3 = 0x8FFD,
        DoubleVec4 = 0x8FFE,
        DoubleMat2 = 0x8F46,
        DoubleMat3 = 0x8F47,
        DoubleMat4 = 0x8F48,
        DoubleMat2X3 = 0x8F49,
        DoubleMat2X4 = 0x8F4A,
        DoubleMat3X2 = 0x8F4B,
        DoubleMat3X4 = 0x8F4C,
        DoubleMat4X2 = 0x8F4D,
        DoubleMat4X3 = 0x8F4E,
        Bool = 0x8B56,
        BoolVec2 = 0x8B57,
        BoolVec3 = 0x8B58,
        BoolVec4 = 0x8B59,
        Sampler1D = 0x8B5D,
        Sampler2D = 0x8B5E,
        Sampler3D = 0x8B5F,
        SamplerCube = 0x8B60,
        Sampler1DShadow = 0x8B61,
        Sampler2DShadow = 0x8B62,
        Sampler2DMultisample = 0x9108,
        Sampler2DMultisampleArray = 0x910B,
        SamplerCubeShadow = 0x8DC5,
        SamplerBuffer = 0x9001,
        Sampler2DRect = 0x8B63,
        Sampler2DRectShadow = 0x8B64,
        IntSampler1D = 0x8DC9,
        IntSampler1DArray = 0x8DCE,
        IntSampler2DArray = 0x8DCF,
        IntSampler2DMultiSample = 0x9109,
        IntSampler2DMultisampleArray = 0x910C,
        IntSamplerBuffer = 0x8DD0,
        IntSampler2DRect = 0x8DCD,
        UIntSampler1D = 0x8DD1,
        UIntSampler2D = 0x8DD2,
        UIntSampler3D = 0x8DD3,
        UIntSamplerCube = 0x8DD4,
        UIntSampler1DArray = 0x8DD6,
        UIntSampler2DArray = 0x8DD7,
        UIntSampler2DMultisample = 0x910A,
        UIntSampler2DMultisampleArray = 0x910D,
        UIntSamplerBuffer = 0x8DD8,
        UIntSampler2DRect = 0x8DD5,
        Image1D = 0x904C,
        Image2D = 0x904D,
        Image3D = 0x904E,
        Image2DRect = 0x904F,
        ImageCube = 0x9050,
        ImageBuffer = 0x9051,
        Image1DArray = 0x9052,
        Image2DArray = 0x9053,
        Image2DMultisample = 0x9055,
        Image2DMultisampleArray = 0x9056,
        IntImage1D = 0x9057,
        IntImage2D = 0x9058,
        IntImage3D = 0x9059,
        IntImageCube = 0x905A,
        IntImage1DArray = 0x905B,
        IntImage2DArray = 0x905C,
        IntImage2DMultisample = 0x905D,
        IntImage2DMultisampleArray = 0x9061,
        UIntImage1D = 0x9062,
        UIntImage2D = 0x9063,
        UIntImage3D = 0x9064,
        UIntImage2DRect = 0x9065,
        UIntImageCube = 0x9066,
        UIntImageBuffer = 0x9067,
        UIntImage1DArray = 0x9068,
        UIntImage2DArray = 0x9069,
        UIntImage2DMultisample = 0x906B,
        UIntImage2DMultisampleArray = 0x906C,
        UIntAtomicCounter = 0x92DB,
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

    internal enum TextureTarget : Enum
    {
        Texture1D = 0x0DE0,
        Texture2D = 0x0DE1,
        Texture3D = 0x806F,
        ProxyTexture2D = 0x8064,
        Texture1DArray = 0x8C18,
        Texture2DArray = 0x8C1A,
        ProxyTexture1DArray = 0x8C19,
        TextureRectangle = 0x84F5,
        ProxyTextureRectangle = 0x84F7,
        TextureCubeMap = 0x8513,
        TextureCubeMapArray = 0x9009,
        TextureCubeMapPositiveX = 0x8515,
        TextureCubeMapNegativeX = 0x8516,
        TextureCubeMapPositiveY = 0x8517,
        TextureCubeMapNegativeY = 0x8518,
        TextureCubeMapPositiveZ = 0x8519,
        TextureCubeMapNegativeZ = 0x851A,
        ProxyTextureCubeMap = 0x851B,
    }

    internal enum SizedInternalFormat : Enum
    {
        R8 = 0x8229,
        R8Snorm = 0x8F94,
        R16 = 0x822A,
        R16Snorm = 0x8F98,
        Rg8 = 0x822B,
        Rg8Snorm = 0x8F95,
        Rg16 = 0x822C,
        Rg16Snorm = 0x8F99,
        R3G3B2 = 0x2A10,
        Rgb4 = 0x804F,
        Rgb5 = 0x8050,
        Rgb8 = 0x8051,
        Rgb8Snorm = 0x8F96,
        Rgb10 = 0x8052,
        Rgb12 = 0x8053,
        Rgb16Snorm = 0x8F9A,
        Rgba2 = 0x8055,
        Rgba4 = 0x8056,
        Rgb5A1 = 0x8057,
        Rgba8 = 0x8058,
        Rgba8Snorm = 0x8F97,
        Rgb10A2 = 0x8059,
        Rgb10A2Ui = 0x906F,
        Rgba12 = 0x805A,
        Rgba16 = 0x805B,
        Srgb8 = 0x8C41,
        Srgb8Alpha8 = 0x8C43,
        R16F = 0x822D,
        Rg16F = 0x822F,
        Rgb16F = 0x881B,
        Rgba16F = 0x881A,
        R32F = 0x822E,
        Rg32F = 0x8230,
        Rgb32F = 0x8815,
        Rgba32F = 0x8814,
        R11FG11FB10F = 0x8C3A,
        Rgb9E5 = 0x8C3D,
        R8I = 0x8231,
        R8Ui = 0x8232,
        R16I = 0x8233,
        R16Ui = 0x8234,
        R32I = 0x8235,
        R32Ui = 0x8236,
        Rg8I = 0x8237,
        Rg8Ui = 0x8238,
        Rg16I = 0x8239,
        Rg16Ui = 0x823A,
        Rg32I = 0x823B,
        Rg32Ui = 0x823C,
        Rgb8I = 0x8D8F,
        Rgb8Ui = 0x8D7D,
        Rgb16I = 0x8D89,
        Rgb16Ui = 0x8D77,
        Rgb32I = 0x8D83,
        Rgb32Ui = 0x8D71,
        Rgba8I = 0x8D8E,
        Rgba8Ui = 0x8D7C,
        Rgba16I = 0x8D88,
        Rgba16Ui = 0x8D76,
        Rgba32I = 0x8D82,
        Rgba32Ui = 0x8D70,
    }

    internal enum PixelFormat : Enum
    {
        Red = 0x1903,
        Rg = 0x8227,
        Rgb = 0x1907,
        Bgr = 0x80E0,
        Rgba = 0x1908,
        Bgra = 0x80E1,
        DepthComponent = 0x1902,
        StencilIndex = 0x1901,
    }
    
    internal enum PixelType : Enum
    {
        UnsignedByte = 0x1401,
        Byte = 0x1400,
        UnsignedShort = 0x1403,
        Short = 0x1402,
        UnsignedInt = 0x1405,
        Int = 0x1404,
        HalfFloat = 0x140B,
        Float = 0x1406,
        UnsignedByte332 = 0x8032,
        UnsignedByte233Rev = 0x8362,
        UnsignedShort4444Rev = 0x8365,
        UnsignedShort5551 = 0x8034,
        UnsignedShort1555Rev = 0x8366,
        UnsignedInt8888 = 0x8367,
        UnsignedInt8888Rev = 0x8367,
        UnsignedInt1010102 = 0x8036,
        UnsignedInt2101010Rev = 0x8368,
    }

    private static readonly delegate* unmanaged<int, int, int, int, void> ViewportPtr =
        (delegate* unmanaged<int, int, int, int, void>)Glfw.GetProcAddress("glViewport");
    private static readonly delegate* unmanaged<EnableCap, void> EnablePtr =
        (delegate* unmanaged<EnableCap, void>)Glfw.GetProcAddress("glEnable");
    private static readonly delegate* unmanaged<EnableCap, void> DisablePtr =
        (delegate* unmanaged<EnableCap, void>)Glfw.GetProcAddress("glDisable");
    private static readonly delegate* unmanaged<BlendFactor, BlendFactor, void> BlendFuncPtr =
        (delegate* unmanaged<BlendFactor, BlendFactor, void>)Glfw.GetProcAddress("glBlendFunc");
    private static readonly delegate* unmanaged<float, float, float, float, void> ClearColorPtr =
        (delegate* unmanaged<float, float, float, float, void>)Glfw.GetProcAddress("glClearColor");
    private static readonly delegate* unmanaged<ClearBufferMask, void> ClearPtr =
        (delegate* unmanaged<ClearBufferMask, void>)Glfw.GetProcAddress("glClear");
    private static readonly delegate* unmanaged<PrimitiveType, SizeI, DrawElementsType, void*, SizeI, void> DrawElementsInstancedPtr =
        (delegate* unmanaged<PrimitiveType, SizeI, DrawElementsType, void*, SizeI, void>)Glfw.GetProcAddress("glDrawElementsInstanced");
    private static readonly delegate* unmanaged<ObjectIdentifier, Uint, SizeI, Char*, void> ObjectLabelPtr =
        (delegate* unmanaged<ObjectIdentifier, Uint, SizeI, Char*, void>)Glfw.GetProcAddress("glObjectLabel");
    
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
    private static readonly delegate* unmanaged<Uint, Uint, SizeI, SizeI*, Int*, AttributeType*, Char*, void> GetActiveAttribPtr =
        (delegate* unmanaged<Uint, Uint, SizeI, SizeI*, Int*, AttributeType*, Char*, void>)Glfw.GetProcAddress("glGetActiveAttrib");
    private static readonly delegate* unmanaged<Uint, Char*, Int> GetAttribLocationPtr =
        (delegate* unmanaged<Uint, Char*, Int>)Glfw.GetProcAddress("glGetAttribLocation");
    private static readonly delegate* unmanaged<Uint, Uint, SizeI, SizeI*, Int*, UniformType*, Char*, void> GetActiveUniformPtr =
        (delegate* unmanaged<Uint, Uint, SizeI, SizeI*, Int*, UniformType*, Char*, void>)Glfw.GetProcAddress("glGetActiveUniform");
    private static readonly delegate* unmanaged<Uint, Char*, Int> GetUniformLocationPtr =
        (delegate* unmanaged<Uint, Char*, Int>)Glfw.GetProcAddress("glGetUniformLocation");
    private static readonly delegate* unmanaged<Uint, Int, Int, void> ProgramUniform1iPtr =
        (delegate* unmanaged<Uint, Int, Int, void>)Glfw.GetProcAddress("glProgramUniform1i");
    
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
    private static readonly delegate* unmanaged<Uint, Uint, Uint, void> VertexArrayBindingDivisorPtr =
        (delegate* unmanaged<Uint, Uint, Uint, void>)Glfw.GetProcAddress("glVertexArrayBindingDivisor");
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

    private static readonly delegate* unmanaged<TextureTarget, SizeI, Uint*, void> CreateTexturesPtr =
        (delegate* unmanaged<TextureTarget, SizeI, Uint*, void>)Glfw.GetProcAddress("glCreateTextures");
    private static readonly delegate* unmanaged<SizeI, Uint*, void> DeleteTexturesPtr =
        (delegate* unmanaged<SizeI, Uint*, void>)Glfw.GetProcAddress("glDeleteTextures");
    private static readonly delegate* unmanaged<Uint, Int, SizedInternalFormat, Int, Int, void> TextureStorage2DPtr =
        (delegate* unmanaged<Uint, Int, SizedInternalFormat, Int, Int, void>)Glfw.GetProcAddress("glTextureStorage2D");
    private static readonly delegate* unmanaged<Uint, Int, Int, Int, Int, Int, PixelFormat, PixelType, void*, void> TextureSubImage2DPtr =
        (delegate* unmanaged<Uint, Int, Int, Int, Int, Int, PixelFormat, PixelType, void*, void>)Glfw.GetProcAddress("glTextureSubImage2D");
    private static readonly delegate* unmanaged<Uint, void> GenerateTextureMipmapPtr =
        (delegate* unmanaged<Uint, void>)Glfw.GetProcAddress("glGenerateTextureMipmap");
    private static readonly delegate* unmanaged<Uint, Uint, void> BindTextureUnitPtr =
        (delegate* unmanaged<Uint, Uint, void>)Glfw.GetProcAddress("glBindTextureUnit");

    
    internal static void Viewport(int x, int y, int width, int height)
    {
        ViewportPtr(x, y, width, height);
    }

    internal static void Enable(EnableCap enableCap)
    {
        EnablePtr(enableCap);
    }

    internal static void Disable(EnableCap enableCap)
    {
        DisablePtr(enableCap);
    }

    internal static void BlendFunc(BlendFactor sfactor, BlendFactor dfactor)
    {
        BlendFuncPtr(sfactor, dfactor);
    }
    
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

    internal static void ObjectLabel(ObjectIdentifier identifier, uint handle, string label)
    {
        nint labelPtr = Marshal.StringToCoTaskMemAnsi(label);
        ObjectLabelPtr(identifier, handle, label.Length, (byte*)labelPtr);
        Marshal.FreeCoTaskMem(labelPtr);
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
    
    internal static void GetActiveAttrib(uint program, uint index, int bufSize, int* lengthPtr, int* sizePtr, AttributeType* typePtr, byte* namePtr)
    {
        GetActiveAttribPtr(program, index, bufSize, lengthPtr, sizePtr, typePtr, namePtr);
    }
    
    internal static int GetAttribLocation(uint program, byte* namePtr)
    {
        return GetAttribLocationPtr(program, namePtr);
    }
    
    internal static void GetActiveUniform(uint program, uint index, int bufSize, int* lengthPtr, int* sizePtr, UniformType* typePtr, byte* namePtr)
    {
        GetActiveUniformPtr(program, index, bufSize, lengthPtr, sizePtr, typePtr, namePtr);
    }
    
    internal static int GetUniformLocation(uint program, byte* namePtr)
    {
        return GetUniformLocationPtr(program, namePtr);
    }

    internal static void Uniform1i(uint program, int location, int v0)
    {
        ProgramUniform1iPtr(program, location, v0);
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
    
    internal static void NamedBufferSubData<T>(uint buffer, nint offset, nint size, ref T data)
        where T : unmanaged
    {
        fixed (T* dataPtr = &data)
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
    
    internal static void VertexArrayBindingDivisor(uint vao, uint bindingIndex, uint divisor)
    {
        VertexArrayBindingDivisorPtr(vao, bindingIndex, divisor);
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
    
    internal static uint CreateTexture(TextureTarget target)
    {
        uint texture = 0;
        CreateTexturesPtr(target, 1, &texture);
        return texture;
    }

    internal static void DeleteTexture(uint texture)
    {
        DeleteVertexArraysPtr(1, &texture);
    }

    internal static void TextureStorage2D(uint texture, int levels, SizedInternalFormat internalformat, int width,
        int height)
    {
        TextureStorage2DPtr(texture, levels, internalformat, width, height);
    }
    
    internal static void TextureSubImage2D<T>(uint texture, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, ref T pixels)
        where T : unmanaged
    {
        fixed (void* pixelPtr = &pixels)
        {
            TextureSubImage2DPtr(texture, level, xoffset, yoffset, width, height, format, type, pixelPtr);
        }
    }

    internal static void GenerateTextureMipmap(uint texture)
    {
        GenerateTextureMipmapPtr(texture);
    }

    internal static void BindTextureUnit(uint unit, uint texture)
    {
        BindTextureUnitPtr(unit, texture);
    }
}
