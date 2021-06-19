using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2 = System.Numerics.Vector2;
using Window = JAngine.Window;

namespace Sandbox
{
    // TODO: Create sprite system.
    internal static class Program
    {
        private static readonly string VertexSrc = @"
#version 450 core
layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec2 vTexCoord;
layout(location = 2) in vec4 vInstanceTransform1;
layout(location = 3) in vec4 vInstanceTransform2;
layout(location = 4) in vec4 vInstanceTransform3;
layout(location = 5) in vec4 vInstanceTransform4;

uniform mat4 uCamera;

out vec2 fTexCoord;

void main()
{
    mat4 instanceTransform = mat4(vInstanceTransform1, vInstanceTransform2, vInstanceTransform3, vInstanceTransform4);
    gl_Position = uCamera * instanceTransform * vec4(vPosition, 0, 1);
    fTexCoord = vTexCoord;
}
";

        private static readonly string FragmentSrc = @"
#version 450 core
in vec2 fTexCoord;

uniform sampler2D uTexture[32];

out vec4 Color;

vec4 sampleTexture(int textureId, vec2 texCoord)
{
    return texture(uTexture[textureId], texCoord);
}

void main()
{
    Color = sampleTexture(0, fTexCoord);
}
";
        
        private static void Main()
        {
            using Engine engine = new Engine();

            Window window = new Window(engine, 800, 600, "Sandbox");
            ShapeDefinition<TextureVertex, TransformInstance, TransformData> shape = new (window,
                ShaderProgram.CreateVertexFragment(window, VertexSrc, FragmentSrc),
                new TextureArray(window, "test.png"),
                new TextureVertex(0, 0.5f),
                new TextureVertex(0.5f, 0),
                new TextureVertex(0.5f, -0.5f),
                new TextureVertex(-0.5f, -0.5f),
                new TextureVertex(-0.5f, 0));
            for (int i = 0; i < 100; i++)
            {
                new TransformInstance(shape, new Vector2(i, 1), Vector2.One * 50);
            }
            var big = new TransformInstance(shape, Vector2.One * 100, Vector2.One * 100);

            int x = 0;
            var loop = new GameLoop(engine, Init, Loop);

            void Init()
            {
            }
            void Loop(float dt)
            {
                big.Position += Vector2.One * -5f * dt;
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