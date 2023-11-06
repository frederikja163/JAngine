using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JAngine;

namespace JAngine.Rendering.OpenGL;

public abstract class ShaderStage : IGlObject, IDisposable
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
                    throw new Exception($"{_type} failed to compile: {infoLog}");
                }
                break;
            case DisposeEvent:
                Gl.DeleteShader(Handle);
                Handle = 0;
                break;
        }
    }
}

internal sealed class ShaderStageLoaderBase : IResourceLoader<ShaderStage>, IResourceLoader<VertexShader>, IResourceLoader<FragmentShader>
{
    ShaderStage IResourceLoader<ShaderStage>.Load(Window window, string fileExtension, Stream stream)
    {
        switch (fileExtension.ToLower())
        {
            case ".vertex":
            case ".vert":
                return ((IResourceLoader<VertexShader>)this).Load(window, fileExtension, stream);
            case ".frag":
            case ".fragment":
                return ((IResourceLoader<FragmentShader>)this).Load(window, fileExtension, stream);
            default:
                throw new Exception(
                    $"Could not resolve shader type {fileExtension}, either load as a specific type or change the name of the file.");
        }
    }

    VertexShader IResourceLoader<VertexShader>.Load(Window window, string fileExtension, Stream stream)
    {
        StreamReader reader = new StreamReader(stream);
        string src = reader.ReadToEnd();
        return new VertexShader(window, src);
    }

    FragmentShader IResourceLoader<FragmentShader>.Load(Window window, string fileExtension, Stream stream)
    {
        StreamReader reader = new StreamReader(stream);
        string src = reader.ReadToEnd();
        return new FragmentShader(window, src);
    }
}

public sealed class VertexShader : ShaderStage
{
    public VertexShader(Window window, string source) : base(window, Gl.ShaderType.VertexShader, source)
    {
    }
}

public sealed class FragmentShader : ShaderStage
{
    public FragmentShader(Window window, string source) : base(window, Gl.ShaderType.FragmentShader, source)
    {
    }
}

public sealed class Shader : IGlObject, IDisposable
{
    internal sealed class Attribute
    {
        public Attribute(string name, int location, int size, Gl.AttributeType type)
        {
            Name = name;
            Location = location;
            Size = size;
            Type = type;
        }

        internal string Name { get; }
        internal int Location { get; }
        internal int Size { get; }
        internal Gl.AttributeType Type { get; }
    }
    
    private Dictionary<string, Attribute> _attributes = new Dictionary<string, Attribute>();
    private readonly ShaderStage[] _stages;
    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;

    public Shader(Window window, params ShaderStage[] stages)
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

                unsafe
                {
                    Gl.GetProgram(Handle, Gl.ProgramProperty.ActiveAttributes, out int attributeCount);
                    Gl.GetProgram(Handle, Gl.ProgramProperty.ActiveAttributeMaxLength, out int maxLength); 
                    nint namePtr = Marshal.AllocCoTaskMem(maxLength);
                    for (uint i = 0; i < attributeCount; i++)
                    {
                        int size = 0;
                        Gl.AttributeType type = 0;
                        Gl.GetActiveAttrib(Handle, i, maxLength, null, &size, &type, (byte*)namePtr);
                        int location = Gl.GetAttribLocation(Handle, (byte*)namePtr);
                        string name = Marshal.PtrToStringAnsi(namePtr)!;
                        
                        _attributes.Add(name, new Attribute(name, location, size, type));
                    }
                    Marshal.FreeCoTaskMem(namePtr);
                }
                break;
            case DisposeEvent:
                Gl.DeleteProgram(Handle);
                Handle = 0;
                break;
        }
    }

    internal bool TryGetAttribute(string name, [NotNullWhen(true)] out Attribute? attribute)
    {
        return _attributes.TryGetValue(name, out attribute);
    }

    internal Attribute GetAttribute(string name)
    {
        if (_attributes.TryGetValue(name, out Attribute? attribute))
        {
            return attribute;
        }

        throw new Exception($"Attribute with name {name} does not exist on this shader.");
    }

    internal void Bind()
    {
        Gl.UseProgram(Handle);
    }
}
