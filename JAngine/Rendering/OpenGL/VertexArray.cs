using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace JAngine.Rendering.OpenGL;

/// <summary>
/// A collection of Vertex Attributes, Vertex Indices and a Shader.
/// </summary>
public sealed class VertexArray : ObjectBase<VertexArrayHandle>
{
    /// <summary>
    /// The Shader of the VertexArray.
    /// </summary>
    public Shader Shader { get; private init; }
    
    private Buffer<uint> _indexBuffer;
    /// <summary>
    /// The IndexBuffer of the VertexArray.
    /// </summary>
    public Buffer<uint> IndexBuffer
    {
        get => _indexBuffer;
        set
        {
            _indexBuffer = value;
            Game.Instance.QueueCommand(() => GL.VertexArrayElementBuffer(Handle, _indexBuffer.Handle));
        }
    }
    
    /// <summary>
    /// Whether or not this VertexArray is Enabled for drawing.
    /// </summary>
    public bool Enabled { get; set; }
    
    /// <summary>
    /// The Order this VertexArray will be drawn in.
    /// </summary>
    public int Order { get; init; }

    private readonly List<ObjectBase<BufferHandle>> _vertexBuffers = new List<ObjectBase<BufferHandle>>();

    /// <summary>
    /// Create a new VertexArray with a Shader and an IndexBuffer.
    /// </summary>
    /// <param name="shader">The Shader of the VertexArray.</param>
    /// <param name="indexBuffer">The IndexBuffer of the VertexArray.</param>
    public VertexArray(Shader shader, Buffer<uint> indexBuffer) : base(GL.CreateVertexArray)
    {
        Game.Instance.AddVertexArray(this);
        Shader = shader;
        _indexBuffer = indexBuffer;
        IndexBuffer = indexBuffer;
    }

    /// <summary>
    /// Add a new Attribute to the VertexArray.
    /// </summary>
    /// <param name="name">The name of the new Attribute.</param>
    /// <param name="buffer">The buffer to get the Attribute data from.</param>
    /// <typeparam name="T">The type of the data for the attribute.</typeparam>
    public unsafe void AddAttribute<T>(string name, Buffer<T> buffer)
        where T : unmanaged
    {
        Game.Instance.QueueCommand(() =>
        {
            uint location = Shader.GetAttribLocation(Shader, name);
            
            _vertexBuffers.Add(buffer);
            uint bindingIndex = (uint)_vertexBuffers.Count;
            
            GL.VertexArrayVertexBuffer(Handle, bindingIndex, buffer.Handle, IntPtr.Zero, sizeof(T));
            GL.VertexArrayAttribBinding(Handle, location, bindingIndex);
            GL.EnableVertexArrayAttrib(Handle, location);
            switch (default(T))
            {
                case float:
                    GL.VertexArrayAttribFormat(Handle, location, 1, VertexAttribType.Float, false, 0);
                    break;
                case Vector2:
                    GL.VertexArrayAttribFormat(Handle, location, 2, VertexAttribType.Float, false, 0);
                    break;
                case Vector3:
                    GL.VertexArrayAttribFormat(Handle, location, 3, VertexAttribType.Float, false, 0);
                    break;
                case Vector4:
                    GL.VertexArrayAttribFormat(Handle, location, 4, VertexAttribType.Float, false, 0);
                    break;
                case int:
                    GL.VertexArrayAttribFormat(Handle, location, 1, VertexAttribType.Int, false, 0);
                    break;
                case Vector2i:
                    GL.VertexArrayAttribFormat(Handle, location, 2, VertexAttribType.Int, false, 0);
                    break;
                case Vector3i:
                    GL.VertexArrayAttribFormat(Handle, location, 3, VertexAttribType.Int, false, 0);
                    break;
                case Vector4i:
                    GL.VertexArrayAttribFormat(Handle, location, 4, VertexAttribType.Int, false, 0);
                    break;
            }
        });
    }

    internal static void Bind(VertexArray vao)
    {
        GL.UseProgram(vao.Shader.Handle);
        GL.BindVertexArray(vao.Handle);
    }

    public override void Dispose()
    {
        Game.Instance.RemoveVertexArray(this);
        Game.Instance.QueueCommand(() =>
        {
            GL.DeleteVertexArray(Handle);
        });
    }
}