#version 460 core
in vec2 vPosition;
in vec2 vTexCoord;
in vec4 vColor;
in vec4 vTransform0;
in vec4 vTransform1;
in vec4 vTransform2;
in vec4 vTransform3;
in int vTextureIndex;
in vec2 vTextureOffset;
in vec2 vTextureSize;

out vec2 fTexCoord;
out vec4 fColor;
flat out int fTextureIndex;

void main() {
    mat4 transform = mat4(vTransform0, vTransform1, vTransform2, vTransform3);
    
    gl_Position = transform * vec4(vPosition, 0, 1);
    fTexCoord = vTexCoord * vTextureSize + vTextureOffset;
    fColor = vColor;
    fTextureIndex = vTextureIndex;
}