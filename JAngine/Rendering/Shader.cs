using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public abstract class SubShader : IAsset, IDisposable
    {
        internal readonly int Handle;
        
        protected SubShader(ShaderType type, StreamReader reader)
        {
            Handle = GL.CreateShader(type);
            GL.ShaderSource(Handle, reader.ReadToEnd());
            GL.CompileShader(Handle);
            GL.GetShaderInfoLog(Handle, out var il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw new Exception(il);
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
    
    public sealed class Shader : IDisposable
    {
        internal readonly int Handle;

        public Shader(params SubShader[] shaders)
        {
            Handle = GL.CreateProgram();
            foreach (var shader in shaders)
            {
                GL.AttachShader(Handle, shader.Handle);
            }
            
            GL.LinkProgram(Handle);
            GL.GetProgramInfoLog(Handle, out string il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw new Exception(il);
            }
            
            foreach (var shader in shaders)
            {
                GL.DetachShader(Handle, shader.Handle);
            }
        }

        public void Bind()
        {
            GL.UseProgram(Handle);
        }

        public int GetUniformLocation(string uniformName)
        {
            return GL.GetUniformLocation(Handle, uniformName);
        }

        public int GetAttributeLocation(string attributeName)
        {
            return GL.GetAttribLocation(Handle, attributeName);
        }

        public void SetUniform(int location, int value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref int value) =>
            GL.ProgramUniform1(Handle, location, 1, ref value);
        
        public void SetUniform(int location, float value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref float value) =>
            GL.ProgramUniform1(Handle, location, 1, ref value);
        
        public void SetUniform(int location, double value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref double value) =>
            GL.ProgramUniform1(Handle, location, 1, ref value);
        
        public void SetUniform(int location, Vector2 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector2 value) =>
            GL.ProgramUniform2(Handle, location, 1, ref value.X);
        
        public void SetUniform(int location, Vector3 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector3 value) =>
            GL.ProgramUniform3(Handle, location, 1, ref value.X);
        
        public void SetUniform(int location, Vector4 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Vector4 value) =>
            GL.ProgramUniform4(Handle, location, 1, ref value.X);
        
        
        public void SetUniform(int location, Matrix4 value) =>
            SetUniform(location, ref value);
        public void SetUniform(int location, ref Matrix4 value) =>
            GL.ProgramUniformMatrix4(Handle, location, 1, false, ref value.Row0.X);

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}