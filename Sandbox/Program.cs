using System.Numerics;
using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.OpenGL;

try
{
    using Window window = new Window("Test", 100, 100);

    ShaderStage vertexShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.2D.vert");
    ShaderStage fragmentShader = Resource.Load<ShaderStage>(window, "JAngine.Shaders.2D.frag");
    using Shader shader = new Shader(window, "Shader Program", vertexShader, fragmentShader);
    vertexShader.Dispose();
    fragmentShader.Dispose();

    Mesh mesh = new Mesh(window, "Square");
    // mesh.AddIndices(new uint[] { 0, 1, 2, 0, 2, 3 });
    mesh.AddVertexAttribute<Vertex2D>();
    // mesh.AddVertices<Vertex2D>(new Vertex2D[]{
    //     new Vertex2D(0, 0),
    //     new Vertex2D(0, 0.5f),
    //     new Vertex2D(1, 1),
    //     new Vertex2D(1, 0),
    // });
    mesh.AddInstanceAttribute<Instance2D>();
    mesh.AddInstance(new Instance2D(Matrix4x4.Identity));
    mesh.BindToShader(shader);
    
    window.AddKeyBinding(Key.A | Key.Held, () =>
    {
        //mesh.GetVertex<Vertex2D>(0).Data = new Vertex2D(Random.Shared.NextSingle(), Random.Shared.NextSingle());
        BufferDataReference<Vertex2D> reference = mesh.AddVertex(new Vertex2D(Random.Shared.NextSingle(), Random.Shared.NextSingle()));
        mesh.AddIndex((uint)reference.Index);
    });
    window.AddKeyBinding(Key.Escape | Key.Press, () =>
    {
        mesh.ClearIndices();
    });
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
