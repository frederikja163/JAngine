using System;
using System.Collections;
using System.Collections.Generic;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public sealed class Shape : Shape<Vertex>
    {
        // TODO: Implement this class
        public Shape(Window window, ShaderProgram shader, TextureArray textures, params Vertex[] points) : base(window, shader, textures, points)
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
    
    public class ShapeArray<TVertex, TInstance> : IDrawable, IDisposable, IEnumerable<TInstance>
        where TVertex : unmanaged, IVertex
        where TInstance : unmanaged, IInstance
    {
        private readonly VertexBuffer<TVertex> _vertexBuffer;
        private VertexBuffer<TInstance> _instanceBuffer;
        private TInstance[] _instances;
        private readonly ElementBuffer _elementBuffer;

        // Non-nullable fields not initialized from constructor.
#pragma warning disable 8618
        private ShapeArray(Window window, ShaderProgram shader, TextureArray textures, params TVertex[] points)
#pragma warning restore 8618
        {
            Window = window;
            int triCount = points.Length - 2;
            int elemCount = triCount * 3;
            uint[] elements = new uint[elemCount];
            for (uint i = 0; i < triCount; i++)
            {
                elements[i * 3 + 0] = 0;
                elements[i * 3 + 1] = i + 1;
                elements[i * 3 + 2] = i + 2;
            }
            _elementBuffer = new ElementBuffer(Window, elements);
            _vertexBuffer = new VertexBuffer<TVertex>(Window, points);
            
            VertexArray = new VertexArray(Window, _elementBuffer);
            VertexArray.AddVertexBuffer(_vertexBuffer, 0, points[0].Attributes);

            Shader = shader;
            Textures = textures;
            Window.AddDrawable(this);
        }
        
        public ShapeArray(Window window, ShaderProgram shader, TextureArray textures, int instanceCount, params TVertex[] points) : this(window, shader, textures, points)
        {
            _instances = new TInstance[instanceCount];
            _instanceBuffer = new VertexBuffer<TInstance>(window, instanceCount);
            VertexArray.AddVertexBuffer(_instanceBuffer, 1, default(TInstance).Attributes);
            InstanceCount = instanceCount;
        }
        
        public ShapeArray(Window window, ShaderProgram shader, TextureArray textures, TInstance[] instances, params TVertex[] points) : this(window, shader, textures, points)
        {
            _instances = instances;
            _instanceBuffer = new VertexBuffer<TInstance>(window, instances);
            VertexArray.AddVertexBuffer(_instanceBuffer, 1, instances[0].Attributes);
            InstanceCount = instances.Length;
        }

        public VertexArray VertexArray { get; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
        public int InstanceCount { get; set; }
        public Window Window { get; }
        public TInstance this[int i] => _instances[i];

        public void Resize(int newSize)
        {
            TInstance[] oldArray = _instances;
            _instances = new TInstance[newSize];
            Array.Copy(oldArray, _instances, Math.Min(oldArray.Length, newSize));
            _instanceBuffer.Dispose();
            _instanceBuffer = new VertexBuffer<TInstance>(Window, _instances);
            VertexArray.AddVertexBuffer(_instanceBuffer, 1, default(TInstance).Attributes);
        }

        public void Add(params TInstance[] instance)
        {
            int index = InstanceCount++;
            if (index + instance.Length >= _instances.Length)
            {
                Resize(InstanceCount * 2);
            }
            Array.Copy(instance, 0, _instances, index, instance.Length);
            _instanceBuffer.SetSubData(index, instance);
        }

        public void Dispose()
        {
            _elementBuffer.Dispose();
            _vertexBuffer.Dispose();
            _instanceBuffer.Dispose();
            VertexArray.Dispose();
            Shader.Dispose();
        }

        public IEnumerator<TInstance> GetEnumerator()
        {
            for (int i = 0; i < InstanceCount; i++)
            {
                yield return _instances[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}