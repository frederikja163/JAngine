using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public abstract class SubShader : IAsset, IDisposable
    {
        private readonly int _handle;

        internal int Handle => _handle;
        
        protected SubShader(ShaderType type, StreamReader reader)
        {
            _handle = GL.CreateShader(type);
            GL.ShaderSource(_handle, reader.ReadToEnd());
            GL.CompileShader(_handle);
            GL.GetShaderInfoLog(_handle, out var il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw Log.Error(il);
            }
        }
        
        public void Dispose()
        {
            GL.DeleteShader(_handle);
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
    
    public sealed class Shader : IDisposable
    {
        private readonly int _handle;
        
        internal int Handle => _handle;

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
        }

        public void Bind()
        {
            GL.UseProgram(_handle);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(_handle, uniformName);
        }

        public int GetAttributeLocation(string attributeName)
        {
            return GL.GetAttribLocation(_handle, attributeName);
        }

        public void SetUniform(int location, int value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref int value) =>
            GL.ProgramUniform1(_handle, location, 1, ref value);
        
        public void SetUniform(int location, float value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref float value) =>
            GL.ProgramUniform1(_handle, location, 1, ref value);
        
        public void SetUniform(int location, double value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref double value) =>
            GL.ProgramUniform1(_handle, location, 1, ref value);
        
        public void SetUniform(int location, Vector2 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector2 value) =>
            GL.ProgramUniform2(_handle, location, 1, ref value.X);
        
        public void SetUniform(int location, Vector3 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector3 value) =>
            GL.ProgramUniform3(_handle, location, 1, ref value.X);
        
        public void SetUniform(int location, Vector4 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector4 value) =>
            GL.ProgramUniform4(_handle, location, 1, ref value.X);
        
        
        public void SetUniform(int location, Matrix4 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Matrix4 value) =>
            GL.ProgramUniformMatrix4(_handle, location, 1, false, ref value.Row0.X);

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}