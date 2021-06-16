using System.Collections.Generic;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public interface IDrawable
    {
        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
        public int InstanceCount { get; set; }
    }

    public record Drawable(VertexArray VertexArray, ShaderProgram Shader, TextureArray Textures) : IDrawable
    {
        public int InstanceCount { get; set; }

        public Drawable(VertexArray vertexArray, ShaderProgram shader, TextureArray textures, int instanceCount = 1) : this(vertexArray, shader, textures)
        {
            InstanceCount = instanceCount;
        }
    }
}