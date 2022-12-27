using System.Reflection;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL.Compatibility;
using OpenTK.Windowing.GraphicsLibraryFramework;
using GL = OpenTK.Graphics.OpenGL.GL;
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
                new FileLogHandler($"Log/{DateTime.Now.ToString("yyyy-MM/dd HH:mm:ss")}.txt")
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
    
    /// <summary>
    /// Initializes the game.
    /// </summary>
    /// <param name="gameSettings">The settings to use for the game.</param>
    public Game(GameSettings gameSettings)
    {
        Log.SetHandlers(gameSettings.LogHandlers);
        _instance = this;

        Log.Info("Initializing Game:");
        if (!GLFW.Init())
        {
            throw new Exception("Failed to initialize GLFW.");
        }
        Log.Trace("Initialized GLFW.");
        
        // GLFW.WindowHint(WindowHintInt.ContextVersionMajor, 4);
        // GLFW.WindowHint(WindowHintInt.ContextVersionMinor, 5);
        // GLFW.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
        string name = gameSettings.Title ?? Assembly.GetCallingAssembly().FullName ?? "Not Set.";
        _window = GLFW.CreateWindow(gameSettings.Width, gameSettings.Height, gameSettings.Title, null, null);
        if (_window is null)
        {
            GLFW.Terminate();
            throw new Exception("Failed to create Window.");
        }
        Log.Trace("Window created.");
        
        GLFW.MakeContextCurrent(_window);
        GLLoader.LoadBindings(new GLFWBindingsContext());
        Log.Trace("OpenGL initialized.");
        
        Log.Info($"Renderer: {GL.GetString(StringName.Renderer)}");
        Log.Info($"Vendor: {GL.GetString(StringName.Vendor)}");
        Log.Info($"Extensions: {GL.GetString(StringName.Extensions)}");
        Log.Info($"OpenGL Version: {GL.GetString(StringName.Version)}");
        Log.Info($"GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");
    }
    
    public void Dispose()
    {
        GLFW.Terminate();
        Log.CloseHandlers();
    }
}