#version 330 core
in layout(location = 0) vec3 vPosition;
in layout(location = 1) vec3 vNormal;
in layout(location = 2) vec2 vTextureCoordinate;

uniform mat4 uView;

out vec4 fColor;

void main()
{
    gl_Position = uView * vec4(vPosition, 1);
    fColor = vec4(vTextureCoordinate, 0, 1);
}