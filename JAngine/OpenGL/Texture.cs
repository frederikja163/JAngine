using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormatDrawing = System.Drawing.Imaging.PixelFormat;

namespace JAngine.OpenGL;

public sealed class Texture : IDisposable
{
    internal TextureHandle Handle { get; private init; }

    public Texture(string path)
    {
        Handle = GL.CreateTexture(TextureTarget.Texture2d);

        Bitmap bitmap = new Bitmap(path);
        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormatDrawing.Format32bppRgb);
        IntPtr ptr = data.Scan0;

        GL.TextureStorage2D(Handle, 1, SizedInternalFormat.Rgba32f, bitmap.Width, bitmap.Height);
        GL.TextureSubImage2D(Handle, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, ptr);

        GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TextureParameteri(Handle, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
        GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.GenerateTextureMipmap(Handle);
    }

    public void Bind(int textureUnit = 0)
    {
        GL.ActiveTexture((TextureUnit)((int)TextureUnit.Texture0 + textureUnit));
        GL.BindTexture(TextureTarget.Texture2d, Handle);
    }

    public void Dispose()
    {
        GL.DeleteTexture(Handle);
    }
}
