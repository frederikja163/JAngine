#version 330 core
in layout(location = 0) vec3 vPosition;
in layout(location = 1) vec3 vNormal;
in layout(location = 2) vec2 vTextureCoordinate;

out vec4 fColor;

void main()
{
    gl_Position = vec4(vPosition, 1);
    fColor = vec4(vTextureCoordinate, 0, 1);
}