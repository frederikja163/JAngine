#version 460 core
in vec2 fTexCoord;
in vec4 fColor;
flat in int fTextureIndex;

uniform sampler2D[32] uTextures;

out vec4 Color;

void main() {
    Color = fColor * texture(uTextures[fTextureIndex], fTexCoord);
}