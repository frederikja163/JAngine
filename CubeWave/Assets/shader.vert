#version 330 core
layout (location = 0) in vec3 vPos;

uniform mat4 uPerspective;
uniform mat4 uView;
uniform float uTime;

out vec3 fColor;

void main()
{
    //sin(dist-t)/(dist+1)*e^(-t)
    vec3 relativePosition = 0.1f*(vec3(gl_InstanceID % 1000, 0, gl_InstanceID / 1000) - vec3(1000, 0, 1000) / 2);
    
    float dist = sqrt(dot(relativePosition, relativePosition));
    float height = sin(dist * 2 - uTime * 10) * 0.5f;
    //float height = (sin(dist - uTime * 10) / (dist * 0.1f + 1))*exp(-uTime / 2);
    gl_Position = uPerspective * uView * vec4(vPos * 0.05f + relativePosition + vec3(0, height, 0), 1);
    //gl_Position = uPerspective * uView * vec4(vPos, 1);
    
    fColor = vPos * 0.5f + 0.5f;
}