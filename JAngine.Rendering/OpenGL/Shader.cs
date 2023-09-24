using JAngine.Core;

namespace JAngine.Rendering.OpenGL;

internal sealed class ShaderStage : IDisposable
{
    internal uint Handle { get; private set; }

    internal ShaderStage(string source, Gl.ShaderType type)
    {
        using (Renderer.EnsureRenderThread())
        {
            Handle = Gl.CreateShader(type);
            Gl.ShaderSource(Handle, source);
            Gl.CompileShader(Handle);
            Gl.GetShader(Handle, Gl.ShaderParameterName.InfoLogLength, out int logLength);
            if (logLength != 0)
            {
                Gl.GetShaderInfoLog(Handle, logLength, out string infoLog);
                throw new Exception(infoLog);
            }
        }
    }

    public void Dispose()
    {
        using (Renderer.EnsureRenderThread())
        {
            Gl.DeleteShader(Handle);
            Handle = 0;
        }
    }
}

internal sealed class Shader : IDisposable
{
    internal uint Handle { get; private set; }

    internal Shader(params ShaderStage[] stages)
    {
        using (Renderer.EnsureRenderThread())
        {
            Handle = Gl.CreateProgram();

            foreach (ShaderStage stage in stages)
            {
                Gl.AttachShader(Handle, stage.Handle);
            }
            
            Gl.LinkProgram(Handle);
            Gl.GetProgram(Handle, Gl.ProgramProperty.InfoLogLength, out int logLength);
            if (logLength != 0)
            {
                Gl.GetProgramInfoLog(Handle, logLength, out string infoLog);
                throw new Exception(infoLog);
            }
            
            foreach (ShaderStage stage in stages)
            {
                Gl.DetachShader(Handle, stage.Handle);
            }
        }
    }

    public void Dispose()
    {
        using (Renderer.EnsureRenderThread())
        {
            Gl.DeleteProgram(Handle);
            Handle = 0;
        }
    }
}