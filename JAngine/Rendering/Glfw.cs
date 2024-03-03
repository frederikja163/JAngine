using System.Reflection;
using System.Runtime.InteropServices;

namespace JAngine.Rendering;

internal static class Glfw
{
    private const string DllName = "glfw";

    static Glfw()
    {
        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), (name, assembly, path) =>
        {
            if (name != DllName)
            {
                return NativeLibrary.Load(name, assembly, path);
            }
            if (OperatingSystem.IsWindows())
            {
                return NativeLibrary.Load("glfw3", assembly, path);
            }

            return NativeLibrary.Load("glfw", assembly, path);
        });
    }
    
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

    public enum Key
    {
        Space =   32,
        Apostrophe =   39, /* ' */
        Comma =   44, /* , */
        Minus =   45, /* - */
        Period =   46, /* . */
        Slash =   47, /* / */
        Num0 =   48,
        Num1 =   49,
        Num2 =   50,
        Num3 =   51,
        Num4 =   52,
        Num5 =   53,
        Num6 =   54,
        Num7 =   55,
        Num8 =   56,
        Num9 =   57,
        Semicolon =   59, /* ; */
        Equal =   61, /* = */
        A =   65,
        B =   66,
        C =   67,
        D =   68,
        E =   69,
        F =   70,
        G =   71,
        H =   72,
        I =   73,
        J =   74,
        K =   75,
        L =   76,
        M =   77,
        N =   78,
        O =   79,
        P =   80,
        Q =   81,
        R =   82,
        S =   83,
        T =   84,
        U =   85,
        V =   86,
        W =   87,
        X =   88,
        Y =   89,
        Z =   90,
        LeftBracket =   91, /* [ */
        Backslash =   92, /* \ */
        RightBracket =   93, /* ] */
        GraveAccent =   96, /* ` */
        World1 =   161, /* non-US #1 */
        World2 =   162, /* non-US #2 */
        Escape =   256,
        Enter =   257,
        Tab =   258,
        Backspace =   259,
        Insert =   260,
        Delete =   261,
        Right =   262,
        Left =   263,
        Down =   264,
        Up =   265,
        PageUp =   266,
        PageDown =   267,
        Home =   268,
        End =   269,
        CapsLock =   280,
        ScrollLock =   281,
        NumLock =   282,
        PrintScreen =   283,
        Pause =   284,
        F1 =   290,
        F2 =   291,
        F3 =   292,
        F4 =   293,
        F5 =   294,
        F6 =   295,
        F7 =   296,
        F8 =   297,
        F9 =   298,
        F10 =   299,
        F11 =   300,
        F12 =   301,
        F13 =   302,
        F14 =   303,
        F15 =   304,
        F16 =   305,
        F17 =   306,
        F18 =   307,
        F19 =   308,
        F20 =   309,
        F21 =   310,
        F22 =   311,
        F23 =   312,
        F24 =   313,
        F25 =   314,
        Kp0 =   320,
        Kp1 =   321,
        Kp2 =   322,
        Kp3 =   323,
        Kp4 =   324,
        Kp5 =   325,
        Kp6 =   326,
        Kp7 =   327,
        Kp8 =   328,
        Kp9 =   329,
        KpDecimal =   330,
        KpDivide =   331,
        KpMultiply =   332,
        KpSubtract =   333,
        KpAdd =   334,
        KpEnter =   335,
        KpEqual =   336,
        LeftShift =   340,
        LeftControl =   341,
        LeftAlt =   342,
        LeftSuper =   343,
        RightShift =   344,
        RightControl =   345,
        RightAlt =   346,
        RightSuper =   347,
        Menu =   348,
        Last =   Menu,
    }

    public enum Action
    {
        Release = 0,
        Press = 1,
        Repeat = 2,
    }

    [Flags]
    public enum Mods
    {
        Shift = 0x0001,
        Control = 0x0002,
        Alt = 0x0004,
        Super = 0x0008,
        CapsLock = 0x0010,
        NumLock = 0x0020,
    }

    public enum OpenGL
    {
        AnyProfile = 0,
        CoreProfile = 0x00032001,
        CompatProfile = 0x00032002,
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
    
    [DllImport(DllName, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = CallingConvention.Cdecl)]
    public static extern void WaitEventsTimeout(double timeout);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void KeyCallback(Window window, Key key, int scancode, Action action, Mods mods);
    
    [DllImport(DllName, EntryPoint = "glfwSetKeyCallback", CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetKeyCallback(Window window, KeyCallback keyCallback);
}
