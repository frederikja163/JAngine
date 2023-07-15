using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace JAngine.Platform.X11Backend;

[StructLayout(LayoutKind.Sequential)]
internal readonly struct DisplayHandle
{
    private readonly IntPtr _handle;
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct ScreenHandle
{
    private readonly int _handle;
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct ColorMap
{
    private readonly IntPtr _handle;
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct WindowHandle
{
    private readonly ulong _handle;
}

[StructLayout(LayoutKind.Sequential)]
internal readonly struct SetWindowAttributes
{
    private readonly IntPtr _backgroundPixmap;
    private readonly ulong _backgroundPixel;
    private readonly IntPtr _borderPixmap;
    private readonly ulong _borderPixel;
    private readonly int _bitGravity;
    private readonly int _winGravity;
    private readonly int _backingStore;
    private readonly ulong _backingPlanes;
    private readonly ulong _backingPixel;
    private readonly bool _saveUnder;
    private readonly long _eventMask;
    private readonly long _doNotPropagateMask;
    private readonly bool _overrideRedirect;
    private readonly ColorMap _colorMap;
    private readonly IntPtr _cursor;

    public SetWindowAttributes(IntPtr backgroundPixmap = default,
        ulong backgroundPixel = default,
        IntPtr borderPixmap = default,
        ulong borderPixel = default,
        int bitGravity = default,
        int winGravity = default,
        int backingStore = default,
        ulong backingPlanes = default,
        ulong backingPixel = default,
        bool saveUnder = default,
        long eventMask = default,
        long doNotPropagateMask = default,
        bool overrideRedirect = default,
        ColorMap colorMap = default,
        IntPtr cursor = default)
    {
        _backgroundPixmap = backgroundPixmap;
        _backgroundPixel = backgroundPixel;
        _borderPixmap = borderPixmap;
        _borderPixel = borderPixel;
        _bitGravity = bitGravity;
        _winGravity = winGravity;
        _backingStore = backingStore;
        _backingPlanes = backingPlanes;
        _backingPixel = backingPixel;
        _saveUnder = saveUnder;
        _eventMask = eventMask;
        _doNotPropagateMask = doNotPropagateMask;
        _overrideRedirect = overrideRedirect;
        _colorMap = colorMap;
        _cursor = cursor;
    }
}

[Flags]
public enum ValueMask : ulong
{
    None = 0,
    BackPixmap = (1L<<0),
    BackPixel = (1L<<1),
    BorderPixmap = (1L<<2),
    BorderPixel = (1L<<3),
    BitGravity = (1L<<4),
    WinGravity = (1L<<5),
    BackingStore = (1L<<6),
    BackingPlanes = (1L<<7),
    BackingPixel = (1L<<8),
    OverrideRedirect = (1L<<9),
    SaveUnder = (1L<<10),
    EventMask = (1L<<11),
    DontPropagate = (1L<<12),
    ColorMap = (1L<<13),
    Cursor = (1L<<14),
}

[SupportedOSPlatform("linux")]
internal static class X11
{
    private const string X11DllName = "X11";

    [DllImport(X11DllName, EntryPoint = "XOpenDisplay", CallingConvention = CallingConvention.Cdecl)]
    public static extern DisplayHandle OpenDisplay([MarshalAs(UnmanagedType.LPStr)] string? name = null);

    [DllImport(X11DllName, EntryPoint = "XCloseDisplay", CallingConvention = CallingConvention.Cdecl)]
    public static extern int CloseDisplay(DisplayHandle displayHandle);
    
    [DllImport(X11DllName, EntryPoint = "XDefaultRootWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern WindowHandle DefaultRootWindow(DisplayHandle displayHandle);
    
    [DllImport(X11DllName, EntryPoint = "XFree", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Free(IntPtr ptr);
    
    [DllImport(X11DllName, EntryPoint = "XFree", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe int Free(void* ptr);

    [DllImport(X11DllName, EntryPoint = "XDefaultScreen", CallingConvention = CallingConvention.Cdecl)]
    public static extern ScreenHandle DefaultScreen(DisplayHandle displayHandle);
    
    [DllImport(X11DllName, EntryPoint = "XRootWindowOfScreen", CallingConvention = CallingConvention.Cdecl)]
    public static extern WindowHandle RootWindowOfScreen(ScreenHandle displayHandle);
    
    [DllImport(X11DllName, EntryPoint = "XBlackPixel", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong BlackPixel(DisplayHandle displayHandle, ScreenHandle screenHandle);
    
    [DllImport(X11DllName, EntryPoint = "XWhitePixel", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong WhitePixel(DisplayHandle displayHandle, ScreenHandle screenHandle);
    
    [DllImport(X11DllName, EntryPoint = "XCreateWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern WindowHandle CreateWindow(DisplayHandle displayHandle, WindowHandle parentHandle, int x, int y, uint width,
        uint height, uint borderWidth, int depth, uint @class, IntPtr visualPtr, ValueMask valueMask, in SetWindowAttributes attributes);
    
    [DllImport(X11DllName, EntryPoint = "XDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern WindowHandle DestroyWindow(DisplayHandle displayHandle, WindowHandle windowHandle);
    
    [DllImport(X11DllName, EntryPoint = "XCreateColormap", CallingConvention = CallingConvention.Cdecl)]
    public static extern ColorMap CreateColorMap(DisplayHandle displayHandle, WindowHandle windowHandle, IntPtr visualPtr, int alloc);
    
    [DllImport(X11DllName, EntryPoint = "XFreeColormap", CallingConvention = CallingConvention.Cdecl)]
    public static extern int FreeColorMap(DisplayHandle displayHandle, ColorMap colorMap);

    [DllImport(X11DllName, EntryPoint = "XClearWindow", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong ClearWindow(DisplayHandle displayHandle, WindowHandle windowHandle);
    
    [DllImport(X11DllName, EntryPoint = "XStoreName", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong StoreName(DisplayHandle displayHandle, WindowHandle windowHandle, [MarshalAs(UnmanagedType.LPStr)] string title);
    
    [DllImport(X11DllName, EntryPoint = "XMapRaised", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong MapRaised(DisplayHandle displayHandle, WindowHandle windowHandle);
    
    [DllImport(X11DllName, EntryPoint = "XSelectInput", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong SelectInput(DisplayHandle displayHandle, WindowHandle windowHandle, EventMask mask);
    
    [DllImport(X11DllName, EntryPoint = "XCheckWindowEvent", CallingConvention = CallingConvention.Cdecl)]
    public static extern void CheckWindowEvent(DisplayHandle displayHandle, WindowHandle windowHandle, EventMask mask,
        out Event ev);
}

internal enum EventType
{
    None = 0,
    KeyPress = 02,
    KeyRelease = 03,
    ButtonPress = 04,
    ButtonRelease = 05,
    MotionNotify = 06,
    EnterNotify = 07,
    LeaveNotify = 08,
    FocusIn = 09,
    FocusOut = 10,
    KeymapNotify = 11,
    Expose = 12,
    GraphicsExpose = 13,
    NoExpose = 14,
    VisibilityNotify = 15,
    CreateNotify = 16,
    DestroyNotify = 17,
    UnmapNotify = 18,
    MapNotify = 19,
    MapRequest = 20,
    ReParentNotify = 21,
    ConfigureNotify = 22,
    ConfigureRequest = 23,
    GravityNotify = 24,
    ResizeRequest = 25,
    CirculateNotify = 26,
    CirculateRequest = 27,
    PropertyNotify = 28,
    SelectionClear = 29,
    SelectionRequest = 30,
    SelectionNotify = 31,
    ColorMapNotify = 32,
    ClientMessage = 33,
    MappingNotify = 34,
    GenericEvent = 35,
    LastEvent = 36,
}

[Flags]
internal enum EventMask : long
{
    NoEventMask = 0L,
    KeyPressMask = 1L << 0,
    KeyReleaseMask = 1L << 1,
    ButtonPressMask = 1L << 2,
    ButtonReleaseMask = 1L << 3,
    EnterWindowMask = 1L << 4,
    LeaveWindowMask = 1L << 5,
    PointerMotionMask = 1L << 6,
    PointerMotionHintMask = 1L << 7,
    Button1MotionMask = 1L << 8,
    Button2MotionMask = 1L << 9,
    Button3MotionMask = 1L << 10,
    Button4MotionMask = 1L << 11,
    Button5MotionMask = 1L << 12,
    ButtonMotionMask = 1L << 13,
    KeymapStateMask = 1L << 14,
    ExposureMask = 1L << 15,
    VisibilityChangeMask = 1L << 16,
    StructureNotifyMask = 1L << 17,
    ResizeRedirectMask = 1L << 18,
    SubstructureNotifyMask = 1L << 19,
    SubstructureRedirectMask = 1L << 20,
    FocusChangeMask = 1L << 21,
    PropertyChangeMask = 1L << 22,
    ColorMapChangeMask = 1L << 23,
    OwnerGrabButtonMask = 1L << 24,
}

[StructLayout(LayoutKind.Sequential, Size = 192)]
internal readonly struct Event
{
    public EventType Type { get; }
}