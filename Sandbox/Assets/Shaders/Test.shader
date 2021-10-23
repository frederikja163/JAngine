#stage Vertex
layout(location = 0) in vec2 vPosition;
layout(location = 1) in vec2 vTexCoord;
layout(location = 2) in vec4 vInstanceTransform1;
layout(location = 3) in vec4 vInstanceTransform2;
layout(location = 4) in vec4 vInstanceTransform3;
layout(location = 5) in vec4 vInstanceTransform4;

uniform mat4 uCamera;

out vec2 fPosition;
out vec2 fTexCoord;

void main()
{
    mat4 instanceTransform = mat4(vInstanceTransform1, vInstanceTransform2, vInstanceTransform3, vInstanceTransform4);
    gl_Position = uCamera * instanceTransform * vec4(vPosition, 0, 1);
    fPosition = gl_Position.xy;
    fTexCoord = vTexCoord;
}

#stage Fragment
in vec2 fPosition;
in vec2 fTexCoord;

uniform sampler2D uTexture[32];

out vec4 Color;

vec4 sampleTexture(int textureId, vec2 texCoord)
{
    return texture(uTexture[textureId], texCoord);
}

void main()
{
    Color = sampleTexture(fPosition.y <= 0.1 ? 0 : 1, fTexCoord);
}