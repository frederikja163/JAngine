using System;
using System.Collections.Generic;
using JAngine.Rendering.LowLevel;
using OpenTK.Graphics.OpenGL;

namespace JAngine.Rendering
{
    public sealed class Shape : Shape<Vertex>
    {
        // TODO: Implement this class
        public Shape(Window window, ShaderProgram shader, TextureArray textures, int instanceCount, params Vertex[] points) : base(window, shader, textures, points)
        {
        }
    }

    public class Shape<TVertex> : ShapeArray<TVertex, BaseInstance> where TVertex : unmanaged, IVertex
    {
        // TODO: Implement this class.
        public Shape(Window window, ShaderProgram shader, TextureArray textures, params TVertex[] points) : base(window, shader, textures, new BaseInstance[0], points)
        {
        }
    }
    
    public class ShapeArray<TVertex, TInstance> : IDrawable, IDisposable
        where TVertex : unmanaged, IVertex
        where TInstance : unmanaged, IInstance
    {
        private readonly VertexBuffer<TVertex> _vertexBuffer;
        private VertexBuffer<TInstance> _instanceBuffer;
        private TInstance[] _instances;
        private readonly ElementBuffer _elementBuffer;

        public ShapeArray(Window window, ShaderProgram shader, TextureArray textures, TInstance[] instances, params TVertex[] points)
        {
            int triCount = points.Length - 2;
            int elemCount = triCount * 3;
            uint[] elements = new uint[elemCount];
            for (uint i = 0; i < triCount; i++)
            {
                elements[i * 3 + 0] = 0;
                elements[i * 3 + 1] = i + 1;
                elements[i * 3 + 2] = i + 2;
            }
            _elementBuffer = new ElementBuffer(window, elements);
            _vertexBuffer = new VertexBuffer<TVertex>(window, points);
            _instanceBuffer = new VertexBuffer<TInstance>(window, instances);
            
            VertexArray = new VertexArray(window, _elementBuffer);
            VertexArray.AddVertexBuffer(_vertexBuffer, 0, points[0].Attributes);
            VertexArray.AddVertexBuffer(_instanceBuffer, 1, instances[0].Attributes);

            Shader = shader;
            Textures = textures;
            InstanceCount = instances.Length;
            window.AddDrawable(this);
        }

        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
        public int InstanceCount { get; set; }
        public TInstance this[int i] => _instances[i];

        public void Dispose()
        {
            _vertexBuffer.Dispose();
            _elementBuffer.Dispose();
            VertexArray.Dispose();
            Shader.Dispose();
        }
    }
}