using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public struct VertexLayout
    {
        internal readonly struct LayoutElement
        {
            public int Count { get; }
            public int Size { get; }
            public VertexAttribPointerType Type { get; }

            public LayoutElement(int count, int size, VertexAttribPointerType type)
            {
                Count = count;
                Size = size;
                Type = type;
            }
        }

        private readonly LayoutElement[] _attributes;
        private int _index;
        private int _stride;
        
        public VertexLayout(int count)
        {
            _index = 0;
            _stride = 0;
            _attributes = new LayoutElement[count];
        }

        public unsafe VertexLayout AddAttribute<T>(int count) where T : unmanaged
        {
            var size = sizeof(T);
            _attributes[_index++] = new LayoutElement(count, size, ParseType<T>());
            _stride += size * count;
            return this;
        }

        internal int Count => _attributes.Length;
        internal LayoutElement this[int i] => _attributes[i];
        internal int Stride => _stride;

        private static VertexAttribPointerType ParseType<T>() where T : unmanaged
        {
            return
                (typeof(T) == typeof(float)) ? VertexAttribPointerType.Float :
                (typeof(T) == typeof(double)) ? VertexAttribPointerType.Double :
                (typeof(T) == typeof(sbyte)) ? VertexAttribPointerType.Byte :
                (typeof(T) == typeof(byte)) ? VertexAttribPointerType.UnsignedByte :
                (typeof(T) == typeof(short)) ? VertexAttribPointerType.Short :
                (typeof(T) == typeof(ushort)) ? VertexAttribPointerType.UnsignedShort :
                (typeof(T) == typeof(int)) ? VertexAttribPointerType.Int :
                (typeof(T) == typeof(uint)) ? VertexAttribPointerType.UnsignedInt :
                throw new ArgumentOutOfRangeException(nameof(T), typeof(T), null);
        }
    }
}