using JAngine;
using JAngine.Rendering.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Sandbox;

internal static class Program
{
    internal static void Main()
    {
        using var game = new Game(new GameSettings()
        {
            Title = "Sandbox"
        });
        
        var ebo = new Buffer<uint>(0, 1, 2);
        using VertexShader vert = Assets.Load<VertexShader>("Assets/shader.vert");
        using FragmentShader frag = Assets.Load<FragmentShader>("Assets/shader.frag");
        var shader = new Shader(vert, frag);
        var vbo = new Buffer<Vector2>(new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1));
        var vao = new VertexArray(shader, ebo);
        vao.AddAttribute("vPos", vbo);

        var random = new Random();
        game.OnUpdate += () =>
        {
            vbo.Add(new Vector2(random.NextSingle(), random.NextSingle()));
            vbo.Add(new Vector2(random.NextSingle(), random.NextSingle()));
            vbo.Add(new Vector2(random.NextSingle(), random.NextSingle()));
            ebo.Add((uint)ebo.Count);
            ebo.Add((uint)ebo.Count);
            ebo.Add((uint)ebo.Count);
        };
        
        game.Run();
    }
}

