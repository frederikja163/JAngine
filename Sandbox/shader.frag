#version 400 core

uniform vec4 uTest;

out vec4 Color;

void main()
{
    Color = vec4(uTest.w, 1, uTest.y, 1);
}