using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public sealed class Shader : IDisposable
    {
        private readonly int _handle;

        public Shader(StreamReader vertex, StreamReader fragment) : this(vertex.ReadToEnd(), fragment.ReadToEnd())
        {
        }
        
        public Shader(string vertexSrc, string fragmentSrc)
        {
            _handle = GL.CreateProgram();
            var vert = CreateShader(vertexSrc, ShaderType.VertexShader);
            var frag = CreateShader(fragmentSrc, ShaderType.FragmentShader);

            GL.AttachShader(_handle, vert);
            GL.AttachShader(_handle, frag);
            
            GL.LinkProgram(_handle);
            
            GL.DetachShader(_handle, vert);
            GL.DetachShader(_handle, frag);
            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }

        private int CreateShader(string src, ShaderType type)
        {
            var shader = GL.CreateShader(type);

            GL.ShaderSource(shader, src);
            GL.CompileShader(shader);

#if DEBUG
            var ilog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrWhiteSpace(ilog))
            {
                throw new Exception($"Failed compiling {type} shader with error: {ilog}");
            }
#endif

            return shader;
        }
        
        public void SetUniform(string name, int value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.Uniform1(location, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.Uniform2(location, value);
        }
        
        public void SetUniform(string name, Vector4 value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.Uniform4(location, value);
        }
        
        public void SetUniform(string name, Color4 value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.Uniform4(location, value);
        }

        public void SetUniform(string name, ref Matrix4 value)
        {
            var location = GL.GetUniformLocation(_handle, name);
            GL.UniformMatrix4(location, false, ref value);
        }

        public void Bind()
        {
            GL.UseProgram(_handle);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}