using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    //TODO: Redo shaders at some point.
    public sealed class Shader : IDisposable
    {
        private readonly int _handle;

        public Shader(string vertexSrc, string fragmentSrc)
        {
            _handle = GL.CreateProgram();

            int AttachShader(string src, ShaderType type)
            {
                int shader = GL.CreateShader(type);
                GL.ShaderSource(shader, src);
                GL.CompileShader(shader);
                GL.GetShaderInfoLog(shader, out var il);
                if (!string.IsNullOrWhiteSpace(il))
                {
                    throw Log.Error(il);
                }

                GL.AttachShader(_handle, shader);

                return shader;
            }

            void RemoveShader(int shader)
            {
                GL.DetachShader(_handle, shader);
                GL.DeleteShader(shader);
            }

            int vert = AttachShader(vertexSrc, ShaderType.VertexShader);
            int frag = AttachShader(fragmentSrc, ShaderType.FragmentShader);

            GL.LinkProgram(_handle);
            GL.GetProgramInfoLog(_handle, out string il);
            if (!string.IsNullOrWhiteSpace(il))
            {
                throw Log.Error(il);
            }

            RemoveShader(vert);
            RemoveShader(frag);
            
            GL.UseProgram(_handle);
        }

        public void Dispose()
        {
            GL.DeleteProgram(_handle);
        }
    }
}