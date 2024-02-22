using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.OpenGL;

try
{
    using Window window = new Window("Test", 100, 100);

    ShaderStage vertexShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.shader.vert");
    ShaderStage fragmentShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.shader.frag");
    using Shader shader = new Shader(window, "Shader Program", vertexShader, fragmentShader);
    vertexShader.Dispose();
    fragmentShader.Dispose();

    Mesh2D mesh = new Mesh2D(window, "Square", new Vertex2D[]
    {
        new Vertex2D(0, 0),
        new Vertex2D(0, 1),
        new Vertex2D(1, 1),
        new Vertex2D(1, 0),
    }, new uint[] { 0, 1, 2, 0, 2, 3 });
    mesh.AddInstance();
    mesh.BindToShader(shader);
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
