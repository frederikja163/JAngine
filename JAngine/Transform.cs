using OpenTK.Mathematics;

namespace JAngine
{
    public sealed class Transform
    {
        public Vector2 Position { get; } = Vector2.Zero;
        public float Rotation { get; } = 0;
        public Vector2 Scale { get; } = Vector2.One;

        public static implicit operator Matrix4(Transform transform)
        {
            return
                Matrix4.CreateScale(transform.Scale.X, transform.Scale.Y, 1) *
                Matrix4.CreateTranslation(transform.Position.X, transform.Position.Y, 0) *
                Matrix4.CreateRotationZ(transform.Rotation);
        }
    }
}