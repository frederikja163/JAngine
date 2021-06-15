using System;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering
{
    public sealed class Shape : Shape<Vertex>
    {
        private static readonly string VertexSrc = @"
#version 450 core
layout(location = 0) in vec2 vPosition;

void main()
{
    gl_Position = vec4(vPosition, 0, 1);
}
";

        private static readonly string FragmentSrc = @"
#version 450 core

out vec4 Color;

void main()
{
    Color = vec4(1, 1, 1, 1);
}
";
        public Shape(Window window, params Vertex[] points) : base(window, ShaderProgram.CreateVertexFragment(window, VertexSrc, FragmentSrc), new TextureArray(window), points)
        {
        }
    }
    
    public class Shape<T> : IDrawable, IDisposable
        where T : unmanaged, IVertex
    {
        private readonly VertexBuffer<T> _vertexBuffer;
        private readonly ElementBuffer _elementBuffer;

        public Shape(Window window, ShaderProgram shader, TextureArray textures, params T[] points)
        {
            _vertexBuffer = new VertexBuffer<T>(window, points);
            int triCount = points.Length - 2;
            int elemCount = triCount * 3;
            uint[] elements = new uint[elemCount];
            for (uint i = 0; i < triCount; i++)
            {
                elements[i * 3 + 0] = 0;
                elements[i * 3 + 1] = i + 1;
                elements[i * 3 + 2] = i + 2;
            }

            _elementBuffer = new ElementBuffer(window, elements);

            VertexArray = new VertexArray(window, _elementBuffer);
            VertexArray.AddVertexBuffer(_vertexBuffer, points[0].Attributes);

            Shader = shader;
            Textures = textures;
            window.AddDrawable(this);
        }

        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }

        public void Dispose()
        {
            _vertexBuffer.Dispose();
            _elementBuffer.Dispose();
            VertexArray.Dispose();
            Shader.Dispose();
        }
    }
}