using OpenTK.Mathematics;

namespace JAngine
{
    public sealed class Transform
    {
        public Vector2 Position { get; } = Vector2.Zero;
        public float Rotation { get; } = 0;
        public Vector2 Scale { get; } = Vector2.One;

        public Matrix4 GetMatrix()
        {
            return
                Matrix4.CreateScale(Scale.X, Scale.Y, 1) *
                Matrix4.CreateTranslation(Position.X, Position.Y, 0) *
                Matrix4.CreateRotationZ(Rotation);
        }
    }
}