using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JAngine.Rendering.LowLevel;

namespace JAngine.Rendering
{
    public abstract class ShapeDefinition<TInstanceData> : IDrawable, IDisposable
        where TInstanceData : unmanaged, IVertex
    {
        protected readonly ElementBuffer ElementBuffer;
        protected VertexBuffer<TInstanceData> InstanceBuffer;
        protected readonly Queue<int> AvailableIndices = new ();

        internal ShapeDefinition(Window window, ShaderProgram shader, TextureArray textures, int triCount)
        {
            Window = window;
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
            
            
            VertexArray = new VertexArray(Window, ElementBuffer);

            Window.AddDrawable(this);
        }

        public VertexArray VertexArray { get; protected set; }
        public ShaderProgram Shader { get; }
        public TextureArray Textures { get; }
        public int InstanceCount { get; set; }
        public Window Window { get; }

        internal void Update(int index, TInstanceData data)
        {
            InstanceBuffer.SetSubData(index, data);
        }

        internal abstract int Add(object instance);

        internal abstract void Remove(int index);

        public virtual void Dispose()
        {
            InstanceBuffer.Dispose();
            VertexArray.Dispose();
            Shader.Dispose();
            Textures.Dispose();
            Window.Dispose();
        }
    }
    
    public class ShapeDefinition<TVertex, TInstance, TInstanceData> : ShapeDefinition<TInstanceData>, IEnumerable<TInstance>
        where TVertex : unmanaged, IVertex
        where TInstance : Instance<TInstanceData>
        where TInstanceData : unmanaged, IVertex
    {
        private readonly VertexBuffer<TVertex> _vertexBuffer;
        protected TInstance?[] Instances;
        
        public ShapeDefinition(Window window, ShaderProgram shader, TextureArray textures, int size, params TVertex[] points)
            : base(window, shader, textures, points.Length - 2)
        {
            _vertexBuffer = new VertexBuffer<TVertex>(Window, points);
            VertexArray.AddVertexBuffer(_vertexBuffer, 0, points[0].Attributes);
            
            Instances = new TInstance[size];
            InstanceBuffer = new VertexBuffer<TInstanceData>(window, size);
            VertexArray.AddVertexBuffer(InstanceBuffer, 1, default(TInstanceData).Attributes);
            InstanceCount = 0;
        }
        
        public TInstance? this[int i] => Instances[i];
        
        public ShapeDefinition(Window window, ShaderProgram shader, TextureArray textures, params TVertex[] points)
            : this(window, shader, textures, 1, points)
        {
        }
        
        public void Resize(int newSize)
        {
            TInstance[] oldArray = Instances;
            Instances = new TInstance[newSize];
            Array.Copy(oldArray, Instances, Math.Min(oldArray.Length, newSize));
            InstanceBuffer.Dispose();
            InstanceBuffer = new VertexBuffer<TInstanceData>(Window, GetData(Instances));
            VertexArray.AddVertexBuffer(InstanceBuffer, 1, default(TInstanceData).Attributes);
        }

        internal override int Add(object instanceObj)
        {
            TInstance instance = (TInstance)instanceObj;
            int index = AvailableIndices.Count >= 1 ?
                AvailableIndices.Dequeue() :
                InstanceCount++;
            if (index >= Instances.Length)
            {
                Resize(InstanceCount * 2);
            }

            Instances[index] = instance;
            InstanceBuffer.SetSubData(index, instance.Data);
            return index;
        }

        internal override void Remove(int index)
        {
            InstanceBuffer.SetSubData(index, default(TInstanceData));
            Instances[index] = null;
            AvailableIndices.Enqueue(index);
        }

        public IEnumerator<TInstance> GetEnumerator()
        {
            for (int i = 0; i < InstanceCount; i++)
            {
                if (Instances[i] != null)
                {
                    continue;
                }
                yield return Instances[i]!;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static TInstanceData[] GetData(TInstance[] instances)
        {
            return instances.Select(i => i?.Data ?? new TInstanceData()).ToArray();
        }

        public override void Dispose()
        {
            base.Dispose();
            _vertexBuffer.Dispose();
        }
    }
}