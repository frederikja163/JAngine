using System.Collections.Generic;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public interface IDrawable
    {
        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
    }

    public record Drawable(VertexArray VertexArray, ShaderProgram Shader, TextureArray Textures) : IDrawable
    {
    }
}