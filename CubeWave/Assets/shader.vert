#version 330 core
layout (location = 0) in vec3 vPos;

uniform mat4 uPerspective;
uniform mat4 uView;
uniform float uTime;

out vec3 fColor;

void main()
{
    vec3 relativePosition = vec3(gl_InstanceID % 100 * 0.25, 0, gl_InstanceID / 100 * 0.25) - vec3(100 * 0.25, 0, 100 * 0.25) / 2;
    float height = sin(sqrt(dot(relativePosition, relativePosition)) / 1 + uTime * 2) * 1f;
    gl_Position = uPerspective * uView * vec4(vPos * 0.1f + relativePosition + vec3(0, height, 0), 1);
    
    fColor = vPos * 0.5f + 0.5f;
}