using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering
{
    public sealed class Geometry : IDrawable
    {
        private static readonly string VertexSrc = @"
#version 450 core
layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec4 vColor;

out vec4 fColor;

void main()
{
    gl_Position = vec4(vPosition, 0, 1);
    fColor = vColor;
}
";

        private static readonly string FragmentSrc = @"
#version 450 core
in vec4 fColor;

out vec4 Color;

void main()
{
    Color = fColor;
}
";
        
        private VertexBuffer<Vertex> _vertexBuffer;
        private ElementBuffer _elementBuffer;
        
        public Geometry(Window window, params Vertex[] points)
        {
            _vertexBuffer = new VertexBuffer<Vertex>(window, points);
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
            VertexArray.AddVertexBuffer(_vertexBuffer, new VertexArray.Attribute(0, 2, VertexAttribType.Float),
                new VertexArray.Attribute(1, 4, VertexAttribType.Float));

            Shader = ShaderProgram.CreateVertexFragment(window, VertexSrc, FragmentSrc);
            window.Draw(this);
        }

        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
    }
}