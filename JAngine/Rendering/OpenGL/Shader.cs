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
    /// Construct a new Shader using shader stages.
    /// </summary>
    /// <param name="stages">The stages of the shader program.</param>
    /// <exception cref="Exception">Thrown in the case of a compilation error, or a linking error.</exception>
    public Shader(params ShaderStage[] stages) : base(GL.CreateProgram)
    {
        Game.Instance.QueueCommand(() =>
        {
            foreach (ShaderStage stage in stages)
            {
                GL.AttachShader(Handle, stage.Handle);
            }
            
            GL.LinkProgram(Handle);
            GL.GetProgramInfoLog(Handle, out string programInfo);
            if (!string.IsNullOrEmpty(programInfo))
            {
                throw new Exception($"Shader linking failed: {programInfo}");
            }

            foreach (ShaderStage stage in stages)
            {
                GL.DetachShader(Handle, stage.Handle);
            }
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