using System.Numerics;

namespace JAngine.Rendering.OpenGL;

public sealed class Texture : IGlObject
{
    private static Dictionary<Window, Texture> _whiteTextures = new Dictionary<Window, Texture>();
    private uint _handle;
    private readonly Window _window;
    private readonly Vector4[,] _pixels;
    
    public Texture(Window window, string name, Vector4[,] pixels)
    {
        _window = window;
        Name = name;
        Width = pixels.GetLength(0);
        Height = pixels.GetLength(1);
        
        window.QueueUpdate(this, CreateEvent.Singleton);
        window.QueueUpdate(this, UpdateDataEvent.Default);
        _pixels = pixels;
    }
    
    public int Width { get; }
    public int Height { get; }

    uint IGlObject.Handle => _handle;
    public string Name { get; }
    Window IGlObject.Window => _window;
    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                _handle = Gl.CreateTexture(Gl.TextureTarget.Texture2D);
                Gl.ObjectLabel(Gl.ObjectIdentifier.Texture, _handle, Name);
                Gl.TextureStorage2D(_handle, 1, Gl.SizedInternalFormat.Rgba32F, Width, Height);
                break;
            case UpdateDataEvent:
                Gl.TextureSubImage2D(_handle, 0, 0, 0, Width, Height, Gl.PixelFormat.Rgba, Gl.PixelType.Float, ref _pixels[0, 0]);
                Gl.GenerateTextureMipmap(_handle);
                break;
        }
    }

    internal void Bind(uint unit)
    {
        Gl.BindTextureUnit(unit, _handle);
    }

    public static Texture White(Window window)
    {
        if (!_whiteTextures.TryGetValue(window, out Texture? texture))
        {
            texture = new Texture(window, "White", new Vector4[,] { { new Vector4(1, 1, 1, 1) } });
            _whiteTextures.Add(window, texture);
        }

        return texture;
    }
}
