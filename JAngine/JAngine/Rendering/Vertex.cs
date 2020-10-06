using OpenTK.Mathematics;

namespace JAngine.Rendering
{
    public struct Vertex
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TextureCoordinate { get; set; }

        public Vertex(float x, float y, float z, float nx, float ny, float nz, float u, float v)
        {
            Position = new Vector3(x, y, z);
            Normal = new Vector3(nx, ny, nz);
            TextureCoordinate = new Vector2(u, v);
        }
        
        public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }

        static Vertex()
        {
            Layout = new AttributeLayout(3)
                .AddAttribute<float>(3)
                .AddAttribute<float>(3)
                .AddAttribute<float>(2);
        }

        public static AttributeLayout Layout { get; }
    }
}