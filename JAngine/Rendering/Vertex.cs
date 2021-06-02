using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public readonly struct Vertex
    {
        public readonly Vector2 Position;
        public readonly Color4<Rgba> Color;

        public Vertex(Vector2 position)
        {
            Position = position;
            Color = Color4.White;
        }

        public Vertex(float x, float y)
        {
            Position = new Vector2(x, y);
            Color = Color4.White;
        }
        
        public Vertex(Vector2 position, Color4<Rgba> color)
        {
            Position = position;
            Color = color;
        }

        public Vertex(float x, float y, Color4<Rgba> color)
        {
            Position = new Vector2(x, y);
            Color = color;
        }
    }
}