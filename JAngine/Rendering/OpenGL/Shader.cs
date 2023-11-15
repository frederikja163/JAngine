using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JAngine;

namespace JAngine.Rendering.OpenGL;

public abstract class ShaderStage : IGlObject, IDisposable
{
    private readonly Gl.ShaderType _type;
    private readonly string _source;

    internal ShaderStage(Window window, string name, Gl.ShaderType type, string source)
    {
        Window = window;
        Name = name;
        _type = type;
        _source = source;
        Window.QueueUpdate(this, CreateEvent.Singleton);
    }

    public string Name { get; }
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
                Gl.ObjectLabel(Gl.ObjectIdentifier.Shader, Handle, Name);
                Gl.ShaderSource(Handle, _source);
                Gl.CompileShader(Handle);
                Gl.GetShader(Handle, Gl.ShaderParameterName.InfoLogLength, out int logLength);
                if (logLength != 0)
                {
                    Gl.GetShaderInfoLog(Handle, logLength, out string infoLog);
                    throw new Exception($"{_type} {Name} failed to compile: {infoLog}");
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
    ShaderStage IResourceLoader<ShaderStage>.Load(Window window, string filePath, Stream stream)
    {
        switch (Path.GetExtension(filePath.ToLower()))
        {
            case ".vertex":
            case ".vert":
                return ((IResourceLoader<VertexShader>)this).Load(window, filePath, stream);
            case ".frag":
            case ".fragment":
                return ((IResourceLoader<FragmentShader>)this).Load(window, filePath, stream);
            default:
                throw new Exception(
                    $"Could not resolve type of shader {filePath}, either load as a specific type or change the name of the file.");
        }
    }

    VertexShader IResourceLoader<VertexShader>.Load(Window window, string filePath, Stream stream)
    {
        StreamReader reader = new StreamReader(stream);
        string src = reader.ReadToEnd();
        return new VertexShader(window, filePath, src);
    }

    FragmentShader IResourceLoader<FragmentShader>.Load(Window window, string filePath, Stream stream)
    {
        StreamReader reader = new StreamReader(stream);
        string src = reader.ReadToEnd();
        return new FragmentShader(window, filePath, src);
    }
}

public sealed class VertexShader : ShaderStage
{
    public VertexShader(Window window, string name, string source) : base(window, name, Gl.ShaderType.VertexShader, source)
    {
    }
}

public sealed class FragmentShader : ShaderStage
{
    public FragmentShader(Window window, string name, string source) : base(window, name, Gl.ShaderType.FragmentShader, source)
    {
    }
}

public sealed class Shader : IGlObject, IDisposable
{
    internal sealed record Attribute
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
    
    internal sealed class Uniform
    {
        public Uniform(string name, int location, int size, Gl.UniformType type)
        {
            Name = name;
            Location = location;
            Size = size;
            Type = type;
        }

        internal string Name { get; }
        internal int Location { get; }
        internal int Size { get; }
        internal Gl.UniformType Type { get; }
    }
    
    private Dictionary<string, Attribute> _attributes = new Dictionary<string, Attribute>();
    private Dictionary<string, Uniform> _uniforms = new Dictionary<string, Uniform>();
    private readonly ShaderStage[] _stages;
    public string Name { get; }
    internal Window Window { get; }
    Window IGlObject.Window => Window;
    internal uint Handle { get; private set; }
    uint IGlObject.Handle => Handle;

    public Shader(Window window, string name, params ShaderStage[] stages)
    {
        Window = window;
        Name = name;
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
                Gl.ObjectLabel(Gl.ObjectIdentifier.Program, Handle, Name);
                foreach (ShaderStage stage in _stages)
                {
                    Gl.AttachShader(Handle, stage.Handle);
                }
                Gl.LinkProgram(Handle);
                Gl.GetProgram(Handle, Gl.ProgramProperty.InfoLogLength, out int logLength);
                if (logLength != 0)
                {
                    Gl.GetProgramInfoLog(Handle, logLength, out string infoLog);
                    throw new Exception($"Failed to link shader {Name}: {infoLog}");
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
                unsafe
                {
                    Gl.GetProgram(Handle, Gl.ProgramProperty.ActiveUniforms, out int uniformCount);
                    Gl.GetProgram(Handle, Gl.ProgramProperty.ActiveUniformMaxLength, out int maxLength); 
                    nint namePtr = Marshal.AllocCoTaskMem(maxLength);
                    for (uint i = 0; i < uniformCount; i++)
                    {
                        int size = 0;
                        Gl.UniformType type = 0;
                        Gl.GetActiveUniform(Handle, i, maxLength, null, &size, &type, (byte*)namePtr);
                        int location = Gl.GetUniformLocation(Handle, (byte*)namePtr);
                        string name = Marshal.PtrToStringAnsi(namePtr)!;
                        
                        _uniforms.Add(name, new Uniform(name, location, size, type));
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

    internal void Bind()
    {
        Gl.UseProgram(Handle);
    }
}
