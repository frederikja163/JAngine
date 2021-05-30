using System;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    public sealed class ShaderProgram : IDisposable
    {
        private sealed class Shader : IDisposable
        {
            internal uint Handle;

            public Shader(string src, ShaderType type)
            {
                Handle = GL.CreateShader(type);
                GL.ShaderSource(Handle, src);
                GL.CompileShader(Handle);
                int compileStatus = 0;
                GL.GetShaderi(Handle, ShaderParameterName.CompileStatus, ref compileStatus);
                if (compileStatus == 0)
                {
                    GL.GetShaderInfoLog(Handle, out string il);
                    throw new Exception(il);
                }
            }

            public void Dispose()
            {
                GL.DeleteShader(Handle);
            }
        }

        internal readonly uint Handle;

        private ShaderProgram(params Shader[] shaders)
        {
            Handle = GL.CreateProgram();
            foreach (Shader shader in shaders)
            {
                GL.AttachShader(Handle, shader.Handle);
            }
            GL.LinkProgram(Handle);
            int linkStatus = 0;
            GL.GetProgrami(Handle, ProgramPropertyARB.LinkStatus, ref linkStatus);
            if (linkStatus == 0)
            {
                int ilLength = 0;
                GL.GetProgrami(Handle, ProgramPropertyARB.InfoLogLength, ref ilLength);
                int _ = 0;
                GL.GetProgramInfoLog(Handle, ilLength, ref _, out string il);
                throw new Exception(il);
            }

            foreach (Shader shader in shaders)
            {
                GL.DetachShader(Handle, shader.Handle);
            }
        }

        public static ShaderProgram CreateVertexFragment(string vertexSrc, string fragmentSrc)
        {
            using Shader vertex = new Shader(vertexSrc, ShaderType.VertexShader);
            using Shader fragment = new Shader(fragmentSrc, ShaderType.FragmentShader);

            return new ShaderProgram(vertex, fragment);
        }

        public void Bind()
        {
            GL.UseProgram(Handle);
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}