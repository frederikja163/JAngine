using JAngine;
using JAngine.Rendering;
using JAngine.Rendering.LowLevel;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
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

out vec2 fTexCoord;

void main()
{
    gl_Position = vec4(vPosition, 0, 1);
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
            Shape<TextureVertex> shape = new Shape<TextureVertex>(window,
                ShaderProgram.CreateVertexFragment(window, VertexSrc, FragmentSrc),
                new TextureArray(window, "test.png"),
                new TextureVertex(0, 0.5f),
                new TextureVertex(0.5f, 0),
                new TextureVertex(0.5f, -0.5f),
                new TextureVertex(-0.5f, -0.5f),
                new TextureVertex(-0.5f, 0));
            engine.Run();
        }
    }
}