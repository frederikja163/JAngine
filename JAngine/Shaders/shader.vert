#version 330 core
layout(location = 5) in vec2 vPosition;

void main() {
    gl_Position = vec4(vPosition, 0, 1);
}