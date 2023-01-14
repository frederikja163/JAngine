using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering.OpenGL;

/// <summary>
/// A Shader used to store programs on the GPU.
/// </summary>
public sealed class Shader : ObjectBase<ProgramHandle>
{
    private readonly Dictionary<string, uint> _attributeCache = new Dictionary<string, uint>();

    /// <summary>
    /// Construct a new Shader for drawing using a VertexShader and a FragmentShader.
    /// </summary>
    /// <param name="vertPath">The path on the disc of the VertexShader.</param>
    /// <param name="fragPath">The path on the disc of the FragmentShader.</param>
    /// <exception cref="Exception">Thrown in the case of a compilation error, or a linking error.</exception>
    public Shader(string vertPath, string fragPath) : base(GL.CreateProgram)
    {
        Game.Instance.QueueCommand(() =>
        {
            ShaderHandle CreateShader(ShaderType type, string path)
            {
                string src = File.ReadAllText(path);
                ShaderHandle handle = GL.CreateShader(type);
                GL.ShaderSource(handle, src);
                GL.CompileShader(handle);
                GL.GetShaderInfoLog(handle, out string shaderInfo);
                if (!string.IsNullOrEmpty(shaderInfo))
                {
                    throw new Exception($"Shader compilation failed: {path}");
                }

                return handle;
            }

            ShaderHandle vertHandle = CreateShader(ShaderType.VertexShader, vertPath);
            ShaderHandle fragHandle = CreateShader(ShaderType.FragmentShader, fragPath);

            GL.AttachShader(Handle, vertHandle);
            GL.AttachShader(Handle, fragHandle);
            GL.LinkProgram(Handle);
            GL.GetProgramInfoLog(Handle, out string programInfo);
            if (!string.IsNullOrEmpty(programInfo))
            {
                throw new Exception($"Shader linking failed: {programInfo}");
            }

            GL.DetachShader(Handle, vertHandle);
            GL.DetachShader(Handle, fragHandle);
            GL.DeleteShader(vertHandle);
            GL.DeleteShader(fragHandle);
        });
    }

    internal static uint GetAttribLocation(Shader shader, string name)
    {
        if (!shader._attributeCache.TryGetValue(name, out uint location))
        {
            location = (uint)GL.GetAttribLocation(shader.Handle, name);
            shader._attributeCache[name] = location;
        }

        return location;
    }

    public override void Dispose()
    {
        Game.Instance.QueueCommand(() => GL.DeleteProgram(Handle));
    }
}