using System.Collections.Generic;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    // TODO: Implement this sprite class properly once asset loading has been worked on more.
    public readonly struct SpriteData : IVertex
    {
        public readonly Vector2 TextureMin;
        public readonly Vector2 TextureMax;
        public readonly Matrix4 Matrix;
        
        private static readonly VertexArray.Attribute[] AttributesField = new[]
        {
            new VertexArray.Attribute(3, 2, VertexAttribType.Float),
            new VertexArray.Attribute(4, 2, VertexAttribType.Float),
            new VertexArray.Attribute(5, 4, VertexAttribType.Float),
            new VertexArray.Attribute(6, 4, VertexAttribType.Float),
            new VertexArray.Attribute(7, 4, VertexAttribType.Float),
            new VertexArray.Attribute(8, 4, VertexAttribType.Float),
        };

        public VertexArray.Attribute[] Attributes => AttributesField;
    }
    
    public sealed class Sprite : Instance<SpriteData>
    {
        public Sprite(ShapeDefinition<SpriteData> shapeDefinition, SpriteData data)
            : base(shapeDefinition, data)
        {
        }
        
        private static readonly string VertexSrc = @"
#version 450 core
layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec2 vTexCoord;
layout(location = 2) in vec2 vTexCoord;
layout(location = 3) in vec2 vTexCoord;
layout(location = 4) in vec4 vInstanceTransform1;
layout(location = 5) in vec4 vInstanceTransform2;
layout(location = 6) in vec4 vInstanceTransform3;
layout(location = 7) in vec4 vInstanceTransform4;

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

        private static readonly Dictionary<Window, List<ShapeDefinition<SpriteData>>> ShapeDefinitions =
            new();
        
        private static ShapeDefinition<SpriteData> GetShapeDefinition(Window window)
        {
            return null;
        }
    }
}