using System.Runtime.InteropServices;

namespace JAngine.Rendering;


internal static class Glfw
{
    private const string DllName = "glfw";
    
    // Add new functions and enums from: https://www.glfw.org/docs/latest/modules.html
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Window
    {
        private readonly IntPtr _handle;

        private Window(IntPtr handle)
        {
            _handle = handle;
        }

        public static Window Null { get; } = new Window(IntPtr.Zero);
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Monitor
    {
        private readonly IntPtr _handle;
        
        private Monitor(IntPtr handle)
        {
            _handle = handle;
        }

        public static Monitor Null { get; } = new Monitor(IntPtr.Zero);
    }

    [DllImport(DllName, EntryPoint = "glfwGetVersion", CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetVersion(out int major, out int minor, out int rev);

    [DllImport(DllName, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool Init();
    
    [DllImport(DllName, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
    public static extern void Terminate();
    
    [DllImport(DllName, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetProcAddress([MarshalAs(UnmanagedType.LPStr)] string procName);
    
    [DllImport(DllName, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
    public static extern void MakeContextCurrent(Glfw.Window window);

    [DllImport(DllName, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SwapBuffers(Glfw.Window window);

    [DllImport(DllName, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool WindowShouldClose(Glfw.Window window);
    
    [DllImport(DllName, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WindowSetShouldClose(Glfw.Window window, bool value);

    [DllImport(DllName, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WindowHint(Hint hint, int value);
    public static void WindowHint(Hint hint, bool value) => WindowHint(hint, value ? 1 : 0);
    public static void WindowHint(Hint hint, OpenGL value) => WindowHint(hint, (int)value);
    
    [DllImport(DllName, EntryPoint = "glfwWindowHintString", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WindowHint(Hint hint, [MarshalAs(UnmanagedType.LPStr)] string value);
    
    [DllImport(DllName, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern Glfw.Window CreateWindow(int width, int height, [MarshalAs(UnmanagedType.LPStr)] string title, Monitor monitor, Window share);
    
    [DllImport(DllName, EntryPoint = "glfwDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern void DestroyWindow(Window window);
    
    
    [DllImport(DllName, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
    public static extern void PollEvents();

    
    public enum Hint : int
    {
        /// Window iconification window attribute.
        Iconified = 0x00020002,

        /// Window resize-ability window hint and window attribute.
        Resizable = 0x00020003,

        /// Window visibility window hint and window attribute.
        Visible = 0x00020004,

        /// Window decoration window hint and window attribute.
        Decorated = 0x00020005,

        /// Window auto-iconification window hint and window attribute.
        AutoIconify = 0x00020006,

        /// Window decoration window hint and window attribute.
        Floating = 0x00020007,

        /// Window maximization window hint and window attribute.
        Maximized = 0x00020008,

        /// Cursor centering window hint.
        CenterCursor = 0x00020009,

        /// Window framebuffer transparency window hint and window attribute.
        TransparentFramebuffer = 0x0002000A,

        /// Mouse cursor hover window attribute.
        Hovered = 0x0002000B,

        /// Input focus window hint or window attribute.
        FocusOnShow = 0x0002000C,

        /// Framebuffer bit depth hint.
        RedBits = 0x00021001,

        /// Framebuffer bit depth hint.
        GreenBits = 0x00021002,

        /// Framebuffer bit depth hint.
        BlueBits = 0x00021003,

        /// Framebuffer bit depth hint.
        AlphaBits = 0x00021004,

        /// Framebuffer bit depth hint.
        DepthBits = 0x00021005,

        /// Framebuffer bit depth hint.
        StencilBits = 0x00021006,

        /// Framebuffer bit depth hint.
        AccumRedBits = 0x00021007,

        /// Framebuffer bit depth hint.
        AccumGreenBits = 0x00021008,

        /// Framebuffer bit depth hint.
        AccumBlueBits = 0x00021009,

        /// Framebuffer bit depth hint.
        AccumAlphaBits = 0x0002100A,

        /// Framebuffer auxiliary buffer hint.
        AuxBuffers = 0x0002100B,

        /// OpenGL stereoscopic rendering hint.
        Stereo = 0x0002100C,

        /// Framebuffer MSAA samples hint.
        Samples = 0x0002100D,

        /// Framebuffer sRGB hint.
        SrgbCapable = 0x0002100E,

        /// Monitor refresh rate hint.
        RefreshRate = 0x0002100F,

        /// Framebuffer double buffering hint.
        Doublebuffer = 0x00021010,

        /// Context client API hint and attribute.
        ClientApi = 0x00022001,

        /// Context client API major version hint and attribute.
        ContextVersionMajor = 0x00022002,

        /// Context client API minor version hint and attribute.
        ContextVersionMinor = 0x00022003,

        /// Context client API revision number attribute.q
        ContextRevision = 0x00022004,

        /// Context client API revision number hint and attribute.
        ContextRobustness = 0x00022005,

        /// OpenGL forward-compatibility hint and attribute.
        OpenglForwardCompat = 0x00022006,

        /// Debug mode context hint and attribute.
        OpenglDebugContext = 0x00022007,

        /// OpenGL profile hint and attribute.
        OpenglProfile = 0x00022008,

        /// Context flush-on-release hint and attribute.
        ContextReleaseBehavior = 0x00022009,

        /// Context error suppression hint and attribute.
        ContextNoError = 0x0002200A,

        /// Context creation API hint and attribute.
        ContextCreationApi = 0x0002200B,
        ScaleToMonitor = 0x0002200C,
        CocoaRetinaFramebuffer = 0x00023001,
        CocoaFrameName = 0x00023002,
        CocoaGraphicsSwitching = 0x00023003,
        X11ClassName = 0x00024001,
        X11InstanceName = 0x00024002,
    }

    public enum OpenGL
    {
        AnyProfile = 0,
        CoreProfile = 0x00032001,
        CompatProfile = 0x00032002,
    }
}