using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    // TODO: Create custom shading language.
    // TODO: Create asset loading system.
    public sealed class ShaderProgram : GlObject
    {
        private sealed class Shader : GlObject
        {
            public Shader(Window window, string src, ShaderType type) : base(window, () => GL.CreateShader(type))
            {
                Window.Queue(() =>
                {
                    GL.ShaderSource(Handle, src);
                    GL.CompileShader(Handle);
                    int compileStatus = 0;
                    GL.GetShaderi(Handle, ShaderParameterName.CompileStatus, ref compileStatus);
                    if (compileStatus == 0)
                    {
                        GL.GetShaderInfoLog(Handle, out string il);
                        throw new Exception(il);
                    }
                });
            }

            public override void Dispose()
            {
                Window.Queue(() =>
                {
                    GL.DeleteShader(Handle);
                });
            }
        }

        private ShaderProgram(Window window, params Shader[] shaders) : base(window, GL.CreateProgram)
        {
            window.Queue(() =>
            {
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
            });
        }

        public static ShaderProgram CreateVertexFragment(Window window, string vertexSrc, string fragmentSrc)
        {
            using Shader vertex = new Shader(window, vertexSrc, ShaderType.VertexShader);
            using Shader fragment = new Shader(window, fragmentSrc, ShaderType.FragmentShader);

            return new ShaderProgram(window, vertex, fragment);
        }

        public override void Dispose()
        {
            Window.Queue(() =>
            {
                GL.DeleteProgram(Handle);
            });
        }
    }
}