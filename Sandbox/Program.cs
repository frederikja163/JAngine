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

    Mesh2D mesh = new Mesh2D(window, "Square", new Vertex2D[]
    {
        new Vertex2D(0, 0),
        new Vertex2D(0, 1),
        new Vertex2D(1, 1),
        new Vertex2D(1, 0),
    }, new uint[] { 0, 1, 2, 0, 2, 3 });
    mesh.AddInstance(new Instance2D(Matrix4x4.Identity));
    mesh.BindToShader(shader);

    KeyBinding a = new KeyBinding(Key.P | Key.LShift | Key.Release);
    a.Enabled = false;
    KeyBinding b = new KeyBinding(Key.Escape | Key.Press);
    a.OnActivate += () =>
    {
        a.Enabled = false;
        b.Enabled = true;
        Log.Info("Menu");
    };
    b.OnActivate += () =>
    {
        a.Enabled = true;
        b.Enabled = false;
        Log.Info("Game");
    };
    window.AddKeyBinding(a);
    window.AddKeyBinding(b);
    window.AddKeyBinding(Key.MouseLeft | Key.Press, () => Log.Info("Pew"));
    
    Window.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.WriteLine(e.StackTrace);
}

Console.ReadKey();
