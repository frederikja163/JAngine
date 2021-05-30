using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public readonly struct Vertex
    {
        public readonly Vector2 Position;

        public Vertex(Vector2 position)
        {
            Position = position;
        }

        public Vertex(float x, float y)
        {
            Position = new Vector2(x, y);
        }
    }
}