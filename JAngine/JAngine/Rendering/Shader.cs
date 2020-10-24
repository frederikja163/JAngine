using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public abstract class SubShader : IAsset, IDisposable
    {
        //TODO: Turn into private field with internal property, for entire solution.
        internal readonly int Handle;

        protected SubShader(ShaderType type, StreamReader reader)
        {
            Handle = GL.CreateShader(type);
            GL.ShaderSource(Handle, reader.ReadToEnd());
            GL.CompileShader(Handle);
            GL.GetShaderInfoLog(Handle, out var il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw Log.Error(il);
            }
        }
        
        public void Dispose()
        {
            GL.DeleteShader(Handle);
        }
    }

    public sealed class VertexShader : SubShader
    {
        public VertexShader(StreamReader reader) : base(ShaderType.VertexShader, reader)
        {
        }
    }
    
    public sealed class FragmentShader : SubShader
    {
        public FragmentShader(StreamReader reader) : base(ShaderType.FragmentShader, reader)
        {
        }
    }
    
    //TODO: Redo shaders at some point.
    public sealed class Shader : IDisposable
    {
        private readonly int _handle;

        public Shader(params SubShader[] shaders)
        {
            _handle = GL.CreateProgram();
            foreach (var shader in shaders)
            {
                GL.AttachShader(_handle, shader.Handle);
            }
            
            GL.LinkProgram(_handle);
            GL.GetProgramInfoLog(_handle, out string il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw Log.Error(il);
            }
            
            foreach (var shader in shaders)
            {
                GL.DetachShader(_handle, shader.Handle);
            }
            
            GL.UseProgram(_handle);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}