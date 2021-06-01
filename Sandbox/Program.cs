using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Window = JAngine.Window;

namespace Sandbox
{
    internal static class Program
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
    Color = vec4(0, 0, 1, 1);
}
";
        
        private static void Main()
        {
            using Engine engine = new Engine();
            Window window = new Window(engine, 800, 600, "Sandbox");
            using GameLoop gameLoop = new GameLoop(engine, () =>
            {
                VertexBuffer<Vertex> vertexBuffer =
                    new VertexBuffer<Vertex>(window, new Vertex(0, 0), new Vertex(1, 1), new Vertex(0, 1));
                ElementBuffer elementBuffer = new ElementBuffer(window, 0, 1, 2);
                VertexArray vao = new VertexArray(window, elementBuffer);
                vao.AddVertexBuffer(vertexBuffer, new VertexArray.Attribute(0, 2, VertexAttribType.Float));
                ShaderProgram shader = ShaderProgram.CreateVertexFragment(window, VertexSrc, FragmentSrc);
                window.Draw(vao, shader);
            }, () =>
            {
            },() =>
            {
                window.Dispose();
            });
            engine.Run();
        }
    }
}