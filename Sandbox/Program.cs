using System.Diagnostics;
using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.OpenGL;

try
{
    using Window window = new Window("Test", 100, 100);

    ShaderStage vertexShader = Resource.Load<ShaderStage>(window, "Sandbox.shader.vert");
    ShaderStage fragmentShader = Resource.Load<ShaderStage>(window, "Sandbox.shader.frag");
    using Shader shader = new Shader(window, vertexShader, fragmentShader);
    vertexShader.Dispose();
    fragmentShader.Dispose();

    Buffer<float> vbo = new Buffer<float>(window, 8);
    vbo.SetSubData(0, 0, 0, 0, 1, 1, 1);
    vbo.SetSubData(6, 1, 0);
    Buffer<uint> ebo = new Buffer<uint>(window, 0, 1, 2, 0, 2, 3);
    VertexArray vao = new VertexArray(window, ebo);
    vao.AddAttribute(vbo, 0, sizeof(float) * 2, 2);
    window.AttachRender(vao, shader);
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
