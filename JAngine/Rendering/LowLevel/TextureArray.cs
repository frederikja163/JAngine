using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormatDrawing = System.Drawing.Imaging.PixelFormat;

namespace JAngine.Rendering.LowLevel
{
    public sealed class TextureArray : IDisposable
    {
        internal readonly Window Window;
        internal readonly uint[] TextureHandles;

        public TextureArray(Window window)
        {
            Window = window;
            TextureHandles = new uint[32];
            Window.Queue(() =>
            {
                GL.CreateTextures(TextureTarget.Texture2d, TextureHandles);
            });
        }
        
        public TextureArray(Window window, string filePath) : this(window)
        {
            Bitmap bitmap = new Bitmap(filePath);
            window.Queue(() =>
            {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly,
                    PixelFormatDrawing.Format32bppRgb);
                IntPtr ptr = data.Scan0;

                GL.TextureStorage2D(TextureHandles[0], 1, SizedInternalFormat.Rgba32f, bitmap.Width, bitmap.Height);
                GL.TextureSubImage2D(TextureHandles[0], 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
                
                GL.TextureParameteri(TextureHandles[0], TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TextureParameteri(TextureHandles[0], TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TextureParameteri(TextureHandles[0], TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                GL.TextureParameteri(TextureHandles[0], TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.GenerateTextureMipmap(TextureHandles[0]);
                
                bitmap.Dispose();
            });
        }

        public void Dispose()
        {
            Window.Queue(() => GL.DeleteTextures(TextureHandles));
        }
    }
}