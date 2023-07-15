using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace JAngine.Platform.X11Backend;

[SupportedOSPlatform("linux")]
internal sealed class X11Window : IWindowBackend
{
    private readonly WindowHandle _windowHandle;
    private readonly DisplayHandle _displayHandle;

    private const EventMask EventMask = X11Backend.EventMask.NoEventMask;
    
    public unsafe X11Window(DisplayHandle displayHandle, WindowInfo windowInfo)
    {
        _displayHandle = displayHandle;
        WindowHandle rootWindow = X11.DefaultRootWindow(_displayHandle);
        ScreenHandle screenHandle = X11.DefaultScreen(_displayHandle);
        
        int[] attribList = new int[]{
            // Glx.FbConfigId, Glx.DontCare,
            // Glx.BufferSize, 0,
            // Glx.Level, 0,
            Glx.DoubleBuffer, 1,
            // Glx.Stereo, 0,
            // Glx.AuxBuffers, 0,
            Glx.RedSize, 8,
            Glx.GreenSize, 8,
            Glx.BlueSize, 8,
            Glx.AlphaSize, 8,
            Glx.DepthSize, 24,
            Glx.StencilSize, 8,
            // Glx.AccumRedSize, 0,
            // Glx.AccumGreenSize, 0,
            // Glx.AccumBlueSize, 0,
            // Glx.AccumAlphaSize, 0,
            // Glx.RenderType, Glx.RgbaBit,
            // Glx.DrawableType, Glx.WindowBit,
            // Glx.XRenderAble, 1,
            // Glx.XVisualType, Glx.DontCare,
            // Glx.TransparentType, Glx.None,
            // Glx.TransparentIndex, Glx.DontCare,
            // Glx.TransparentRedValue, Glx.DontCare,
            // Glx.TransparentBlueValue, Glx.DontCare,
            // Glx.TransparentGreenValue, Glx.DontCare,
            // Glx.TransparentAlphaValue, Glx.DontCare,
            Glx.None,
        };
        if (!Glx.QueryVersion(_displayHandle, out int major, out int minor) || (major == 1 && minor < 3))
        {
            throw new PlatformNotSupportedException("Requires a GLX version bigger than 1.3 for X11 backend.");
        }
        
        FbConfig* fbConfig  = Glx.ChooseFbConfig(_displayHandle, screenHandle, attribList, out int items);
        VisualInfoHandle* visualInfoHandle = Glx.GetVisualFromFbConfig(displayHandle, fbConfig[0]);

        ColorMap colorMap = X11.CreateColorMap(_displayHandle, rootWindow, visualInfoHandle->Visual, 0);
        SetWindowAttributes windowAttributes = new SetWindowAttributes(
            borderPixel: X11.BlackPixel(displayHandle, screenHandle),
            colorMap: colorMap
        );
        
        _windowHandle = X11.CreateWindow(_displayHandle,
            rootWindow, 
            0,
            0,
            (uint)windowInfo.Width,
            (uint)windowInfo.Height,
            0,
            visualInfoHandle->Depth,
            1,
            visualInfoHandle->Visual,
            ValueMask.BackPixmap | ValueMask.ColorMap | ValueMask.BorderPixel | ValueMask.EventMask,
            windowAttributes);

        X11.StoreName(_displayHandle, _windowHandle, windowInfo.Title);
        
        X11.ClearWindow(_displayHandle, _windowHandle);
        X11.MapRaised(_displayHandle, _windowHandle);
        X11.SelectInput(_displayHandle, _windowHandle, EventMask);

        X11.FreeColorMap(displayHandle, colorMap);
        X11.Free(visualInfoHandle);
    }

    public void HandleEvents()
    {
        X11.CheckWindowEvent(_displayHandle, _windowHandle, EventMask, out Event ev);
    }

    public void Dispose()
    {
        X11.DestroyWindow(_displayHandle, _windowHandle);
    }
}