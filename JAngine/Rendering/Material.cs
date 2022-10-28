using JAngine.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering;

public sealed class Material : IDisposable
{
    private static readonly string ShaderPath = Path.Combine("Assets", "Shaders");
    private readonly Shader _shader;
    private readonly Dictionary<string, object> _uniforms = new Dictionary<string, object>();
    private readonly Texture?[] _textures = new Texture?[Renderer.MaxTextures];

    public Material(string shaderName)
    {
        string path = Path.Combine(ShaderPath, shaderName);
        _shader = new Shader(path + ".vert", path + ".frag");
    }

    public void SetTexture(string name, string path)
    {
        int index = Get(name, -1);
        if (index == -1)
        {
            for (int i = 0; i < _textures.Length; i++)
            {
                if (_textures[i] == null)
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            _textures[index]?.Dispose();
        }
        Set(name, index);
        _textures[index] = new Texture(path);

    }

    public void Set<T>(string name, T value) where T : unmanaged
    {
        if (Get<T>(name).Equals(value))
        {
            return;
        }

        switch (value)
        {
            case int:
                _uniforms[name] = value;
                _shader.SetUniform(name, (int)(object)value);
                break;
            case Matrix4:
                _uniforms[name] = value;
                _shader.SetUniform(name, (Matrix4)(object)value);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Uniform type {typeof(T).Name} not supported yet.");
        }
    }

    public T Get<T>(string name, T defaultValue = default) where T : unmanaged
    {
        switch (defaultValue)
        {
            case int:
            case Matrix4:
                return (T)_uniforms.GetValueOrDefault(name, defaultValue);
            default:
                throw new ArgumentOutOfRangeException($"Uniform type {typeof(T).Name} not supported yet.");
        }
    }


    public void Bind()
    {
        _shader.Bind();
        Set("uProjection", Camera3D.Main?.ProjectionMatrix ?? Matrix4.Identity);
        Set("uView", Camera3D.Main?.ViewMatrix ?? Matrix4.Identity);

        for (int i = 0; i < _textures.Length; i++)
        {
            Texture? texture = _textures[i];
            if (texture != null)
            {
                texture.Bind(i);
            }
        }
    }

    public void Dispose()
    {
        _shader.Dispose();
    }
}
