using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Mathematics;

namespace Sandbox
{
    // TODO: Create sprite system.
    internal static class Program
    {        
        private static void Main()
        {
            using Engine engine = new Engine();

            Window window = new Window(engine, 800, 600, "Sandbox");
            ShapeDefinition<TextureVertex, TransformInstance, TransformData> shape = new(
                Assets.Load<ShaderProgram>(window, "Shaders/Test.shader"),
                new TextureArray(window, "Textures/test.png", "Textures/test2.jpg"),
                new TextureVertex(0, 0.5f),
                new TextureVertex(0.5f, 0),
                new TextureVertex(0.5f, -0.5f),
                new TextureVertex(-0.5f, -0.5f),
                new TextureVertex(-0.5f, 0));

            TransformInstance[] instances = new TransformInstance[100];
            for (int i = 0; i < 100; i++)
            {
                instances[i] = new TransformInstance(shape, new Vector2(i, 1), Vector2.One * 50);
            }
            var big = new TransformInstance(shape, Vector2.One * 100, Vector2.One * 100);

            int x = 0;
            var loop = new GameLoop(engine, Init, Loop);
            loop.Start();
            
            void Init()
            {
            }
            
            void Loop(float dt)
            {
                big.Position += Vector2.One * -5f * dt;
                big.Rotation += dt;
                if (x++ > 100)
                {
                    x -= 100;
                    new TransformInstance(shape, big.Position, Vector2.One * 100);
                }
            }
            
            engine.Run();
        }
    }
}