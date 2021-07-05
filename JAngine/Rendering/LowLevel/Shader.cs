using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.LowLevel
{
    // TODO: Create custom shading language.
    // TODO: Create asset loading system.
    public sealed class ShaderProgram : GlObject<ProgramHandle>
    {
        private sealed class Shader : GlObject<ShaderHandle>
        {
            private static void Delete(in ShaderHandle shader) => GL.DeleteShader(shader);

            public Shader(Window window, string src, ShaderType type) : base(window, () => GL.CreateShader(type), Delete)
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

        private static void Delete(in ProgramHandle program) => GL.DeleteProgram(program);
        private ShaderProgram(params Shader[] shaders) : base(shaders[0].Window, GL.CreateProgram, Delete)
        {
            Window.Queue(() =>
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

        // Create a ShaderProgram from raw strings of pure glsl code.
        public static ShaderProgram CreateVertexFragment(Window window, string vertexSrc, string fragmentSrc)
        {
            using Shader vertex = new Shader(window, vertexSrc, ShaderType.VertexShader);
            using Shader fragment = new Shader(window, fragmentSrc, ShaderType.FragmentShader);

            return new ShaderProgram(vertex, fragment);
        }

        internal static void CreateCache(StreamReader reader, StreamWriter writer)
        {
            // TODO: Parse custom shading language.
            ShaderType? stage = null;
            Dictionary<ShaderType, StringBuilder> stages = new ();
            StringBuilder? stageBuilder = null;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine()!;
                if (line.StartsWith("#"))
                {
                    string[] args = line.Split(' ');
                    switch(args[0])
                    {
                        // TOOD: Support other stages here than just vertex and fragment stages.
                        case "#stage":
                            stage = args[1] switch
                            {
                                "Vertex" => ShaderType.VertexShader,
                                "Fragment" => ShaderType.FragmentShader,
                                _ => throw new Exception("That is not a supported shader type.")
                            };
                            if (!stages.TryGetValue(stage.Value, out StringBuilder? builder))
                            {
                                builder = new StringBuilder();
                                stages.Add(stage.Value, builder);
                                // Write a global header to all shaders.
                                builder.AppendLine("#version 450 core");
                            }
                            stageBuilder = builder;
                            break;
                    }
                }
                else if (stage != null && stageBuilder != null)
                {
                    stageBuilder.AppendLine(line);
                }
            }

            void WriteStage(StringBuilder builder)
            {
                char length = (char)builder.Length;
                writer.Write(length);
                writer.Write(builder.ToString());
            }

            WriteStage(stages[ShaderType.VertexShader]);
            WriteStage(stages[ShaderType.FragmentShader]);
        }

        internal static ShaderProgram CreateObject(Window window, StreamReader reader)
        {
            string ReadStage()
            {
                int length = reader.Read();
                char[] buffer = new char[length];
                reader.Read(buffer);
                return new string(buffer);
            }

            string vertexSrc = ReadStage();
            string fragmentSrc = ReadStage();
            return ShaderProgram.CreateVertexFragment(window, vertexSrc, fragmentSrc);
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