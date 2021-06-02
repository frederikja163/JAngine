using JAngine;
using JAngine.Rendering;
using OpenTK.Mathematics;
using Window = JAngine.Window;

namespace Sandbox
{
    internal static class Program
    {
        private static void Main()
        {
            using Engine engine = new Engine();
            Window window = new Window(engine, 800, 600, "Sandbox");
            Geometry geometry;
            using GameLoop gameLoop = new GameLoop(engine, () =>
            {
                geometry = new Geometry(window,
                    new Vertex(0, 0.5f, Color4.Blue),
                    new Vertex(0.5f, 0, Color4.Purple),
                    new Vertex(0.5f, -0.5f),
                    new Vertex(-0.5f, -0.5f),
                    new Vertex(-0.5f, 0));
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