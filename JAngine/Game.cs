using System.Reflection;
using JAngine.Extensions.Reflection;
using JAngine.Rendering.OpenGL;
using OpenTK.Graphics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ClearBufferMask = OpenTK.Graphics.OpenGL.ClearBufferMask;
using DrawElementsType = OpenTK.Graphics.OpenGL.DrawElementsType;
using GL = OpenTK.Graphics.OpenGL.GL;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;
using StringName = OpenTK.Graphics.OpenGL.StringName;

namespace JAngine;

/// <summary>
/// All the general settings for a game.
/// </summary>
/// <param name="LogHandlers">The Logging handlers for the game.</param>
public record GameSettings(IEnumerable<ILogHandler> LogHandlers, int Width = 1080, int Height = 720, string? Title = null)
{
    /// <summary>
    /// Default GameSettings.
    /// </summary>
    public GameSettings() : this(
        new List<ILogHandler>()
            {
                new ConsoleLogHandler(),
                new FileLogHandler($"Log/{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.txt"),
                // TODO: Create latest using hardlink instead.
                new FileLogHandler($"Log/Latest.txt")
            }
        )
    {
        
    }
}

/// <summary>
/// A single instance of this symbolises the entire game.
/// </summary>
public sealed unsafe class Game : IDisposable
{
    private static Game? _instance;
    /// <summary>
    /// The singular instance of the game.
    /// </summary>
    public static Game Instance {
        get
        {
            if (_instance is null)
            {
                throw new Exception(
                    $"Must create an instance of {nameof(Game)} before the singleton instance can be gotten.");
            }

            return _instance;
        }
    }

    private readonly Window* _window;
    private readonly List<Action> _commandQueue = new ();
    private readonly HashSet<VertexArray> _vertexArrays = new ();
    private ManualResetEvent? _resetEvent;
    private readonly object _lockObject = new object();
    
    /// <summary>
    /// Initializes the game.
    /// </summary>
    /// <param name="gameSettings">The settings to use for the game.</param>
    public Game(GameSettings gameSettings)
    {
        Log.SetHandlers(gameSettings.LogHandlers);
        _instance = this;
        GLFW.SetErrorCallback(((error, description) => Log.Error($"{error} - {description}")));
        
        Log.Info("Initializing Game:");
        if (GLFW.Init() == false)
        {
            throw new Exception("Failed to initialize GLFW.");
        }
        Log.Trace("Initialized GLFW.");

        GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
        GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
        GLFW.WindowHint(WindowHintBool.Floating, true);
        GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
        string name = gameSettings.Title ?? Assembly.GetCallingAssembly().FullName ?? "Not Set.";
        _window = GLFW.CreateWindow(gameSettings.Width, gameSettings.Height, name, null, null);
        if (_window is null)
        {
            GLFW.Terminate();
            throw new Exception("Failed to create Window.");
        }
        Log.Trace("Window created.");

        GLFW.MakeContextCurrent(_window);
        GLLoader.LoadBindings(new GLFWBindingsContext());
        Log.Trace("OpenGL initialized.");
        
        GLFW.SetWindowSizeCallback(_window, (_, width, height) => GL.Viewport(0, 0, width * 10, height * 10));
        GL.Viewport(0, 0, gameSettings.Width, gameSettings.Height);
        
        Log.Info($"Renderer: {GL.GetString(StringName.Renderer)}");
        Log.Info($"Vendor: {GL.GetString(StringName.Vendor)}");
        Log.Info($"OpenGL Version: {GL.GetString(StringName.Version)}");
        Log.Info($"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
        Log.Info($"Extensions: {GL.GetString(StringName.Extensions)}");
        
        Assemblies.Add(Assembly.GetCallingAssembly());
        Assemblies.Add(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Called once per frame before rendering.
    /// </summary>
    public event Action? OnUpdate;
    
    /// <summary>
    /// Called once at initialization of the program.
    /// </summary>
    public event Action? OnInit;
    
    /// <summary>
    /// Starts the game and enters the game-loop.
    /// When calling this method, expect it to only return after the game has completed running.
    /// </summary>
    public void Run()
    {
        GLFW.MakeContextCurrent(null);
        Thread renderingThread = new Thread(() =>
        {
            GLFW.MakeContextCurrent(_window);
            while (!GLFW.WindowShouldClose(_window))
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);
                
                RenderFrame();

                GLFW.SwapBuffers(_window);
            }
        });
        renderingThread.Start();

        OnInit?.Invoke();
        
        while (!GLFW.WindowShouldClose(_window))
        {
            OnUpdate?.Invoke();
            GLFW.PollEvents();
        }
    }

    private void RenderFrame()
    {
        VertexArray[] vertexArrays;
        Action[] commandQueue;
        ManualResetEvent? resetEvent;
        lock (_lockObject)
        {
            vertexArrays = _vertexArrays.ToArray();
            commandQueue = _commandQueue.ToArray();
            _commandQueue.Clear();
            resetEvent = _resetEvent;
            _resetEvent = null;
        }

        foreach (Action command in commandQueue)
        {
            command.Invoke();
        }
        resetEvent?.Set();

        foreach (VertexArray vertexArray in vertexArrays)
        {
            VertexArray.Bind(vertexArray);
            GL.DrawElements(PrimitiveType.Triangles, vertexArray.IndexBuffer.Count, DrawElementsType.UnsignedInt, 0);
        }
    }

    internal void AddVertexArray(VertexArray vao)
    {
        lock (_lockObject)
        {
            _vertexArrays.Add(vao);
        }
    }

    internal void RemoveVertexArray(VertexArray vao)
    {
        lock (_lockObject)
        {
            _vertexArrays.Remove(vao);
        }
    }
    
    /// <summary>
    /// Closes any unmanaged resources in the game.
    /// </summary>
    public void Dispose()
    {
        GLFW.Terminate();
        Log.CloseHandlers();
    }

    internal Task QueueCommand(Action command)
    {
        ManualResetEvent resetEvent;
        lock (_lockObject)
        {
            _commandQueue.Add(command);

            if (_resetEvent is null)
            {
                _resetEvent = new ManualResetEvent(false);
            }
            resetEvent = _resetEvent;
        }

        resetEvent.WaitOne();
        return Task.CompletedTask;
    }
}