using JAngine.Core;

namespace JAngine.Rendering.OpenGL;

internal sealed class ShaderStage : IGlObject, IDisposable
{
    private readonly Gl.ShaderType _type;
    private readonly string _source;

    internal ShaderStage(Window window, Gl.ShaderType type, string source)
    {
        Window = window;
        _type = type;
        _source = source;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }
    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;

    public void Dispose()
    {
        Window.QueueUpdate(this, DisposeEvent.Singleton);
    }

    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateShader(_type);
                Gl.ShaderSource(Handle, _source);
                Gl.CompileShader(Handle);
                Gl.GetShader(Handle, Gl.ShaderParameterName.InfoLogLength, out int logLength);
                if (logLength != 0)
                {
                    Gl.GetShaderInfoLog(Handle, logLength, out string infoLog);
                    throw new Exception(infoLog);
                }
                break;
            case DisposeEvent:
                Gl.DeleteShader(Handle);
                Handle = 0;
                break;
        }
    }
}

internal sealed class Shader : IGlObject, IDisposable
{
    private readonly ShaderStage[] _stages;
    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;

    internal Shader(Window window, params ShaderStage[] stages)
    {
        Window = window;
        _stages = stages;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }

    public void Dispose()
    {
        Window.QueueUpdate(this, DisposeEvent.Singleton);
    }

    void IGlObject.DispatchEvent(IGlEvent glEvent)
    {
        switch (glEvent)
        {
            case CreateEvent:
                Handle = Gl.CreateProgram();
                foreach (ShaderStage stage in _stages)
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
                foreach (ShaderStage stage in _stages)
                {
                    Gl.DetachShader(Handle, stage.Handle);
                }
                break;
            case DisposeEvent:
                Gl.DeleteProgram(Handle);
                Handle = 0;
                break;
        }
    }
}