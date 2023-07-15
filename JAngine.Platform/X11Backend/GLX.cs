using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace JAngine.Platform.X11Backend;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct VisualInfoHandle
{
    private readonly IntPtr _visual;
    private readonly ulong _visualId;
    private readonly int _screen;
    private readonly int _depth;
    private readonly int _class;
    private readonly ulong _redMask;
    private readonly ulong _greenMask;
    private readonly ulong _blueMask;
    private readonly int _colorMapSize;
    private readonly int _bitsPerRgb;

    public IntPtr Visual => _visual;
    public ulong VisualId => _visualId;
    public int Screen => _screen;
    public int Depth => _depth;
    public int Class => _class;
    public ulong RedMask => _redMask;
    public ulong GreenMask => _greenMask;
    public ulong BlueMask => _blueMask;
    public int ColorMapSize => _colorMapSize;
    public int BitsPerRgb => _bitsPerRgb;
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct Context
{
    private readonly IntPtr _handle;

    private Context(IntPtr handle)
    {
        _handle = handle;
    }

    public static Context Zero => new Context(IntPtr.Zero);
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct FbConfig
{
    private readonly IntPtr _handle;
}

internal static class Glx
{
    private const string GlxDllName = "libGLX.so";
    public const int UseGl = 1;
    public const int BufferSize = 2;
    public const int Level = 3;
    public const int Rgba = 4;
    public const int DoubleBuffer = 5;
    public const int Stereo = 6;
    public const int AuxBuffers = 7;
    public const int RedSize = 8;
    public const int GreenSize = 9;
    public const int BlueSize = 10;
    public const int AlphaSize = 11;
    public const int DepthSize = 12;
    public const int StencilSize = 13;
    public const int AccumRedSize = 14;
    public const int AccumGreenSize = 15;
    public const int AccumBlueSize = 16;
    public const int AccumAlphaSize = 17;
    public const int BadScreen = 1;
    public const int BadAttribute = 2;
    public const int NoExtension = 3;
    public const int BadVisual = 4;
    public const int BadContext = 5;
    public const int BadValue = 6;
    public const int BadEnum = 7;
    public const int Vendor = 1;
    public const int Version = 2;
    public const int Extensions = 3;
    public const int ConfigCaveat = 0x20;
    public const int DontCare = unchecked((int)0xFFFFFFFF);
    public const int XVisualType = 0x22;
    public const int TransparentType = 0x23;
    public const int TransparentIndexValue = 0x24;
    public const int TransparentRedValue = 0x25;
    public const int TransparentGreenValue = 0x26;
    public const int TransparentBlueValue = 0x27;
    public const int TransparentAlphaValue = 0x28;
    public const int WindowBit = 0x00000001;
    public const int PixmapBit = 0x00000002;
    public const int PBufferBit = 0x00000004;
    public const int AuxBuffersBit = 0x00000010;
    public const int FrontLeftBufferBit = 0x00000001;
    public const int FrontRightBufferBit = 0x00000002;
    public const int BackLeftBufferBit = 0x00000004;
    public const int BackRightBufferBit = 0x00000008;
    public const int DepthBufferBit = 0x00000020;
    public const int StencilBufferBit = 0x00000040;
    public const int AccumBufferBit = 0x00000080;
    public const int None = 0x8000;
    public const int SlowConfig = 0x8001;
    public const int TrueColor = 0x8002;
    public const int DirectColor = 0x8003;
    public const int PseudoColor = 0x8004;
    public const int StaticColor = 0x8005;
    public const int GrayScale = 0x8006;
    public const int StaticGray = 0x8007;
    public const int TransparentRgb = 0x8008;
    public const int TransparentIndex = 0x8009;
    public const int VisualId = 0x800B;
    public const int Screen = 0x800C;
    public const int NonConformanceConfig = 0x800D;
    public const int DrawableType = 0x8010;
    public const int RenderType = 0x8011;
    public const int XRenderAble = 0x8012;
    public const int FbConfigId = 0x8013;
    public const int RgbaType = 0x8014;
    public const int ColorIndexType = 0x8015;
    public const int MaxPBufferWidth = 0x8016;
    public const int MaxPBufferHeight = 0x8017;
    public const int MaxPBufferPixels = 0x8018;
    public const int PreservedContents = 0x801B;
    public const int LargestPBuffer = 0x801C;
    public const int Width = 0x801D;
    public const int Height = 0x801E;
    public const int EventMask = 0x801F;
    public const int Damaged = 0x8020;
    public const int Saved = 0x8021;
    public const int Window = 0x8022;
    public const int PBuffer = 0x8023;
    public const int PBufferHeight = 0x8040;
    public const int PBufferWidth = 0x8041;
    public const int RgbaBit = 0x00000001;
    public const int ColorIndexBit = 0x00000002;
    public const int PBufferClobberMask = 0x08000000;

    [DllImport(GlxDllName, EntryPoint = "glXQueryVersion", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool QueryVersion(DisplayHandle displayHandle, out int major, out int minor);

    [DllImport(GlxDllName, EntryPoint = "glXChooseFBConfig", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe FbConfig* ChooseFbConfig(DisplayHandle displayHandle, ScreenHandle screen,
        [MarshalAs(UnmanagedType.LPArray)] int[] attribList, out int items);
    
    [DllImport(GlxDllName, EntryPoint = "glXGetVisualFromFbConfig", CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.SafeArray)]
    public static extern unsafe VisualInfoHandle* GetVisualFromFbConfig(DisplayHandle displayHandle, FbConfig config);

    [DllImport(GlxDllName, EntryPoint = "glXGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe IntPtr GetProcAddress([MarshalAs(UnmanagedType.LPStr)] string name);

    public static class CreateContextArb
    {
        public const int ContextMajorVersionArb = 0x2091;
        public const int ContextMinorVersionArb = 0x2092;
        public const int ContextFlagsArb = 0x2094;
        public const int ContextProfileMaskArb = 0x9126;

        public delegate Context CreateContextAttribsDelegate(DisplayHandle displayHandle,
            FbConfig fbConfig,
            Context shareContext, bool direct,
            [MarshalAs(UnmanagedType.LPArray)] int[] attribList);
        public static CreateContextAttribsDelegate CreateContextAttribs =
           Marshal.GetDelegateForFunctionPointer<CreateContextAttribsDelegate>(
               GetProcAddress("glXCreateContextAttribsARB"));
    }
}
