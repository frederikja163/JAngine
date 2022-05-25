using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public abstract class ShapeDefinition<TInstance> : IDrawable, IDisposable, IEnumerable<TInstance>
        where TInstance : unmanaged, IVertex
    {
        protected readonly ElementBuffer ElementBuffer;
        protected TInstance?[] Instances;
        protected VertexBuffer<TInstance> InstanceBuffer;
        protected readonly Queue<int> AvailableIndices = new ();

        internal ShapeDefinition(ShaderProgram shader, TextureArray textures, int size, int triCount)
        {
            if (shader.Window != textures.Window)
            {
                throw new Exception("The shader and the texturearray must come from the same window context.");
            }

            Window = shader.Window;
            Shader = shader;
            Textures = textures;
            
            int elemCount = triCount * 3;
            uint[] elements = new uint[elemCount];
            for (uint i = 0; i < triCount; i++)
            {
                elements[i * 3 + 0] = 0;
                elements[i * 3 + 1] = i + 1;
                elements[i * 3 + 2] = i + 2;
            }
            ElementBuffer = new ElementBuffer(Window, elements);

            Instances = new TInstance?[size];
            unsafe
            {
                InstanceBuffer = new VertexBuffer<TInstance>(Window, size * sizeof(TInstance));
            }

            VertexArray = new VertexArray(ElementBuffer);
            VertexArray.AddVertexBuffer(InstanceBuffer, 1);
            InstanceCount = 0;

            Window.AddDrawable(this);
        }

        public VertexArray VertexArray { get; protected set; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
        public int InstanceCount { get; set; }
        public Window Window { get; }
        public TInstance? this[int i] => Instances[i];

        internal void Update(int index, TInstance data)
        {
            InstanceBuffer.SetSubData(index, data);
        }

        public void Resize(int newSize)
        {
            TInstance?[] oldArray = Instances;
            Instances = new TInstance?[newSize];
            Array.Copy(oldArray, Instances, Math.Min(oldArray.Length, newSize));
            InstanceBuffer.Dispose();
            InstanceBuffer = new VertexBuffer<TInstance>(Window, GetData(Instances));
            VertexArray.AddVertexBuffer(InstanceBuffer, 1);
        }

        internal int Add(TInstance instance)
        {
            int index = AvailableIndices.Count >= 1 ?
                AvailableIndices.Dequeue() :
                InstanceCount++;
            if (index >= Instances.Length)
            {
                Resize(InstanceCount * 2);
            }

            Instances[index] = instance;
            InstanceBuffer.SetSubData(index, instance);
            return index;
        }

        internal void Remove(int index)
        {
            InstanceBuffer.SetSubData(index, default(TInstance));
            Instances[index] = null;
            AvailableIndices.Enqueue(index);
        }

        private static TInstance[] GetData(TInstance?[] instances)
        {
            return instances.Select(i => i.HasValue ? i.Value : default(TInstance)).ToArray();
        }

        public IEnumerator<TInstance> GetEnumerator()
        {
            foreach (TInstance? instance in Instances)
            {
                if (instance is not null)
                {
                    yield return instance.Value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Dispose()
        {
            InstanceBuffer.Dispose();
            VertexArray.Dispose();
            Shader.Dispose();
            Textures.Dispose();
            Window.Dispose();
        }
    }
    
    public class ShapeDefinition<TVertex, TInstance> : ShapeDefinition<TInstance>
        where TVertex : unmanaged, IVertex
        where TInstance : unmanaged, IVertex
    {
        private readonly VertexBuffer<TVertex> _vertexBuffer;
        
        public ShapeDefinition(ShaderProgram shader, TextureArray textures, int size, params TVertex[] points)
            : base(shader, textures, size, points.Length - 2)
        {
            _vertexBuffer = new VertexBuffer<TVertex>(Window, points);
            VertexArray.AddVertexBuffer(_vertexBuffer, 0);
            
        }
        
        public ShapeDefinition(ShaderProgram shader, TextureArray textures, params TVertex[] points)
            : this(shader, textures, 1, points)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            _vertexBuffer.Dispose();
        }
    }
}