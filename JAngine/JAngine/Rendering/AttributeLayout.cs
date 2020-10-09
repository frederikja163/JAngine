using System;
using OpenTK.Graphics.OpenGL4;

namespace JAngine.Rendering
{
    public class AttributeLayout
    {
        internal readonly struct LayoutElement
        {
            public int Location { get; }
            public int Count { get; }
            public int Size { get; }
            public VertexAttribPointerType Type { get; }
            public int Divisor { get; }

            public LayoutElement(int location, int count, int size, VertexAttribPointerType type, int divisor)
            {
                Location = location;
                Count = count;
                Size = size;
                Type = type;
                Divisor = divisor;
            }
        }

        private readonly LayoutElement[] _attributes;
        private int _index;
        private int _stride;
        
        public AttributeLayout(int count)
        {
            _index = 0;
            _stride = 0;
            _attributes = new LayoutElement[count];
        }

        public unsafe AttributeLayout AddAttribute<T>(int count, int location = -1, int divisor = 0) where T : unmanaged
        {
            var size = sizeof(T);
            _attributes[_index] = new LayoutElement(location == -1 ? _index : location, count, size, ParseType<T>(), divisor);
            _stride += size * count;
            _index += 1;
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