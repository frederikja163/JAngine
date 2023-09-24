using JAngine.Rendering.OpenGL;

namespace JAngine.Rendering;

/// <summary>
/// A window used for drawing to the screen and as a context to a renderer.
/// </summary>
public sealed class Window : IDisposable
{
    private static readonly HashSet<Window> Windows = new();

    private readonly Glfw.Window _handle;
    private readonly Thread _renderingThread;
    
    /// <summary>
    /// Initialize a new instance of the <see cref="Window"/> class.
    /// </summary>
    /// <param name="title">The title of the new window.</param>
    /// TODO: Use some create new window object and allow restoring old window size and other settings from a file.
    public Window(string title, int width, int height)
    {
        _handle = Glfw.CreateWindow(width, height, title, Glfw.Monitor.Null, Renderer.ShareContext);
        _renderingThread = new Thread(RenderThread);
        _renderingThread.Start();
        Windows.Add(this);
    }

    /// <summary>
    /// Holds the thread and runs while windows are still open.
    /// Checks for events for each of the windows.
    /// </summary>
    public static void Run()
    {
        while (Windows.Any())
        {
            Glfw.PollEvents();
        }
    }

    private void RenderThread()
    {
        Glfw.MakeContextCurrent(_handle);
        
        uint vertexShader = Gl.CreateShader(Gl.ShaderType.VertexShader); 
        Gl.ShaderSource(vertexShader, $@"
#version 330 core

in vec2 vPosition;

void main(){{
    gl_Position = vec4(vPosition, 0, 1);
}}
");
        Gl.CompileShader(vertexShader);
        Gl.GetShader(vertexShader, Gl.ShaderParameterName.InfoLogLength, out int logLength);
        if (logLength != 0)
        {
            Gl.GetShaderInfoLog(vertexShader, logLength, out string infoLog);
            throw new Exception(infoLog);
        }

        uint fragmentShader = Gl.CreateShader(Gl.ShaderType.FragmentShader); 
        Gl.ShaderSource(fragmentShader, $@"
#version 330 core

out vec4 Color;

void main(){{
    Color = vec4(1, 1, 1, 1);
}}
");
        Gl.CompileShader(fragmentShader);
        Gl.GetShader(fragmentShader, Gl.ShaderParameterName.InfoLogLength, out logLength);
        if (logLength != 0)
        {
            Gl.GetShaderInfoLog(fragmentShader, logLength, out string infoLog);
            throw new Exception(infoLog);
        }

        uint program = Gl.CreateProgram();
        Gl.AttachShader(program, vertexShader);
        Gl.AttachShader(program, fragmentShader);
        Gl.LinkProgram(program);
        Gl.GetProgram(program, Gl.ProgramProperty.InfoLogLength, out logLength);
        if (logLength != 0)
        {
            Gl.GetProgramInfoLog(program, logLength, out string infoLog);
            throw new Exception(infoLog);
        }
        
        Gl.DetachShader(program, vertexShader);
        Gl.DeleteShader(vertexShader);
        Gl.DetachShader(program, fragmentShader);
        Gl.DeleteShader(fragmentShader);
        
        uint vbo = Gl.CreateBuffer();
        Gl.NamedBufferStorage<float>(vbo, new float[] {0, 0, 1, 1, 0, 1}, Gl.BufferStorageMask.DynamicStorageBit);
        uint ebo = Gl.CreateBuffer();
        Gl.NamedBufferStorage<uint>(ebo, new uint[] {0, 1, 2}, Gl.BufferStorageMask.DynamicStorageBit);

        uint vao = Gl.CreateVertexArray();
        Gl.VertexArrayElementBuffer(vao, ebo);
        Gl.VertexArrayVertexBuffer(vao, 0, vbo, 0, 2 * sizeof(float));
        Gl.VertexArrayAttribFormat(vao, 0, 2, Gl.VertexAttribType.Float, false, 0);
        Gl.VertexArrayAttribBinding(vao, 0, 0);
        Gl.EnableVertexArrayAttrib(vao, 0);
        
        Renderer.ClearColor(1, 0, 1, 1);
        while (IsOpen)
        {
            Renderer.Clear();
            
            Gl.BindVertexArray(vao);
            Gl.UseProgram(program);
            Gl.DrawElementsInstanced(Gl.PrimitiveType.Triangles, 3, Gl.DrawElementsType.UnsignedInt, 0, 1);
            
            Glfw.SwapBuffers(_handle);
        }
    }

    /// <summary>
    /// Gets or sets whether this window is open.
    /// False indicates the window should close as soon as possible or is already closed.
    /// True indicates the window will open as soon as possible or is already opened.
    /// </summary>
    public bool IsOpen
    {
        get => !Glfw.WindowShouldClose(_handle);
        set => Glfw.WindowSetShouldClose(_handle, !value);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Glfw.DestroyWindow(_handle);
        Windows.Remove(this);
    }
}