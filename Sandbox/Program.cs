using JAngine;
using JAngine.Rendering.OpenGL;
using OpenTK.Mathematics;

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
        var shader = new Shader("Assets/shader.vert", "Assets/shader.frag");
        var vbo = new Buffer<Vector2>(new Vector2(-1, -1), new Vector2(-1, 0), new Vector2(0, -1));
        var vao = new VertexArray(shader, ebo);
        vao.AddAttribute("vPos", vbo);
        
        game.Run();
    }
}

