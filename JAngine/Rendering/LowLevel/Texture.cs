using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelFormatDrawing = System.Drawing.Imaging.PixelFormat;

namespace JAngine.Rendering.LowLevel
{
    public sealed class Texture : GlObject<TextureHandle>
    {
        private static TextureHandle Create() => GL.CreateTexture(TextureTarget.Texture2d);
        public Texture(Window window, int width, int height, byte[] data) : base(window, Create, GL.DeleteTexture)
        {
            window.Queue(() =>
            {
                GL.TextureStorage2D(Handle, 1, SizedInternalFormat.Rgba32f, width, height);
                GL.TextureSubImage2D(Handle, 0, 0, 0, width, height, PixelFormat.Bgra, PixelType.UnsignedByte, data[0]);

                GL.TextureParameteri(Handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TextureParameteri(Handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TextureParameteri(Handle, TextureParameterName.TextureWrapR, (int)TextureWrapMode.Repeat);
                GL.TextureParameteri(Handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.GenerateTextureMipmap(Handle);
            });
        }
        
        internal static void CreateCache(StreamReader reader, StreamWriter writer)
        {
            // TODO: Look into using bitmap.save here instead maybe?
            Bitmap bitmap = new Bitmap(reader.BaseStream);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                PixelFormatDrawing.Format32bppRgb);
            IntPtr ptr = data.Scan0;
            byte[] bytes = new byte[data.Stride * data.Height];
            Marshal.Copy(ptr, bytes, 0, bytes.Length);

            BinaryWriter bWriter = new BinaryWriter(writer.BaseStream);
            bWriter.Write(data.Width);
            bWriter.Write(data.Height);
            bWriter.Write(bytes);
        }

        internal static Texture CreateObject(Window window, StreamReader reader)
        {
            BinaryReader bReader = new BinaryReader(reader.BaseStream);
            int width = bReader.ReadInt32();
            int height = bReader.ReadInt32();
            byte[] data = bReader.ReadBytes(width * height * 4);

            return new Texture(window, width, height, data);
        }
    }

    public sealed class TextureArray : IDisposable
    {
        internal readonly Window Window;
        internal readonly Texture[] Textures;
        internal readonly TextureHandle[] TextureHandles;

        public TextureArray(Window window, params string[] texturePaths)
        {
            Window = window;
            Textures = new Texture[window.MaxTextureUnits];
            TextureHandles = new TextureHandle[window.MaxTextureUnits];

            Add(texturePaths);
        }

        public TextureArray(params Texture[] textures) : this(textures[0].Window)
        {
            Add(textures);
        }

        public int Length { get; private set; } = 0;
        public Texture this[int i] => Textures[i];

        public void Add(params Texture[] textures)
        {
            foreach (Texture texture in textures)
            {
                if (texture.Window != this.Window)
                {
                    throw new Exception("The texture must match the window context of the texture array.");
                }
                int index = Length++;
                Textures[index] = texture;
                Window.Queue(() =>
                {

                    TextureHandles[index] = texture.Handle;
                });
            }
        }

        public void Add(params string[] texturePaths)
        {
            Texture[] textures = new Texture[texturePaths.Length];
            for (int i = 0; i < texturePaths.Length; i++)
            {
                textures[i] = Assets.Load<Texture>(Window, texturePaths[i]);
            }
            Add(textures);
        }

        public void Dispose()
        {
            Window.Queue(() => GL.DeleteTextures(TextureHandles));
        }
    }
}