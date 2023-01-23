using JAngine;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.OpenGL;

internal abstract class ShaderLoader<T> : IAssetLoader where T : ShaderStage
{
    public Type AssetType => typeof(T);
    
    public void CreateCache(Stream rawStream, Stream cacheStream)
    {
        StreamReader reader = new StreamReader(rawStream);
        string src = reader.ReadToEnd();
        StreamWriter writer = new StreamWriter(cacheStream);
        writer.Write(src);
        writer.Flush();
    }

    public object LoadCache(Stream cacheStream)
    {
        StreamReader reader = new StreamReader(cacheStream);
        string src = reader.ReadToEnd();
        return CreateNew(src);
    }

    protected abstract T CreateNew(string src);
}

/// <summary>
/// A single stage of a <see cref="Shader"/>.
/// </summary>
public abstract class ShaderStage: ObjectBase<ShaderHandle>
{
    internal ShaderStage(ShaderType type, string src) : base(() => GL.CreateShader(type))
    {
        Game.Instance.QueueCommand(() =>
        {
            GL.ShaderSource(Handle, src);
            GL.CompileShader(Handle);
            GL.GetShaderInfoLog(Handle, out string shaderInfo);
            if (!string.IsNullOrEmpty(shaderInfo))
            {
                throw new Exception($"Shader compilation failed: {shaderInfo}");
            }
        });
    }

    public override void Dispose()
    {
        Game.Instance.QueueCommand(() =>
        {
            GL.DeleteShader(Handle);
        });
    }
}

/// <summary>
/// A vertex shader stage.
/// </summary>
public sealed class VertexShader : ShaderStage
{
    internal VertexShader(string src) : base(ShaderType.VertexShader, src) { }
}

internal class VertexLoader : ShaderLoader<VertexShader>
{
    protected override VertexShader CreateNew(string src)
        => new VertexShader(src);
}

/// <summary>
/// A fragment shader stage.
/// </summary>
public sealed class FragmentShader : ShaderStage
{
    internal FragmentShader(string src) : base(ShaderType.FragmentShader, src) { }
}

internal class FragmentLoader : ShaderLoader<FragmentShader>
{
    protected override FragmentShader CreateNew(string src)
        => new FragmentShader(src);
}