namespace JAngine.Rendering
{
    public sealed class Quad : IDrawable
    {
        private static VertexArray _vao;
        private static VertexBuffer<Vertex> _vbo;
        private static ElementBuffer _ebo;

        static Quad()
        {
            _ebo = new ElementBuffer(new uint[]{0, 1, 2, 0, 2, 3});
            _vao = new VertexArray(_ebo);
            _vbo = new VertexBuffer<Vertex>(new []
            {
                new Vertex(-0.5f, -0.5f, 0, 0, 0, -1, 0, 0),
                new Vertex( 0.5f, -0.5f, 0, 0, 0, -1, 1, 0),
                new Vertex( 0.5f,  0.5f, 0, 0, 0, -1, 1, 1),
                new Vertex( -0.5f, 0.5f, 0, 0, 0, -1, 0, 1),
            });
            _vao.AddAttributes(_vbo, Vertex.Layout);
        }

        public void Draw()
        {
            _vao.Draw();
        }
    }
}