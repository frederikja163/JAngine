#version 330 core
in vec2 fTexCoord;
in vec4 fColor;

out vec4 Color;

void main() {
    Color = vec4(1) * fColor;
}