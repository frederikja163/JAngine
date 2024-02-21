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

    Mesh3D mesh = new Mesh3D(window, "Square", new Vertex3D[]
    {
        new Vertex3D(0, 0, 0),
        new Vertex3D(0, 1, 0),
        new Vertex3D(1, 1, 0),
        new Vertex3D(1, 0, 0),
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
