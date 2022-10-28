using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.OpenGL;

public sealed class Shader : IDisposable
{
    private readonly Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();
    public ProgramHandle Handle { get; private init; }

    public Shader(string vertexPath, string framgentPath)
    {
        Handle = GL.CreateProgram();

        ShaderHandle vertex = CreateShader(vertexPath, ShaderType.VertexShader);
        ShaderHandle fragment = CreateShader(framgentPath, ShaderType.FragmentShader);

        GL.LinkProgram(Handle);

        GL.DetachShader(Handle, vertex);
        GL.DeleteShader(vertex);
        GL.DetachShader(Handle, fragment);
        GL.DeleteShader(fragment);

        ShaderHandle CreateShader(string path, ShaderType type)
        {
            string src = File.ReadAllText(path);
            ShaderHandle handle = GL.CreateShader(type);
            GL.ShaderSource(handle, src);
            GL.CompileShader(handle);
            GL.GetShaderInfoLog(handle, out string info);
            if (!string.IsNullOrWhiteSpace(info))
            {
                throw new Exception($"{path} couldn't compile with error: {info}");
            }
            GL.AttachShader(Handle, handle);
            return handle;
        }
    }

    public void Bind()
    {
        GL.UseProgram(Handle);
    }

    private int GetUniformLocation(string name)
    {
        if (!_uniformLocations.TryGetValue(name, out int location))
        {
            location = GL.GetUniformLocation(Handle, name);
            _uniformLocations.Add(name, location);
        }
        return location;
    }

    public void SetUniform(string name, Matrix4 value)
    {
        int location = GetUniformLocation(name);
        GL.ProgramUniformMatrix4f(Handle, location, false, value);
    }
    public void SetUniform(string name, int value)
    {
        int location = GetUniformLocation(name);
        GL.ProgramUniform1i(Handle, location, value);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
    }
}
