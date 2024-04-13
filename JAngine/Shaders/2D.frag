#version 330 core
in vec2 fTexCoord;
in vec4 fColor;

uniform sampler2D[32] uTextures;

out vec4 Color;

void main() {
    Color = vec4(1) * fColor * texture(uTextures[0], fTexCoord);
}