using System.Reflection;

namespace JAngine;

/// <summary>
/// Configuration for the engine.
/// </summary>
public sealed class Config
{
    /// <summary>
    ///  Is the engine in debug mode?
    /// Debug mode means the engine outputs debug information to the console.
    /// </summary>
    public bool Debug { get; init; } = true;
    /// <summary>
    /// Is the engine in tracing mode.
    /// Tracing mode means the engine outputs verbosely to the console.
    /// </summary>
    public bool Trace { get; init; } = true;
}

public static class Engine
{
    private static Config? _config = null;

    /// <summary>
    /// Configuration for the running engine.
    /// </summary>
    /// <exception cref="Exception">Thrown when <see cref="Start"/> has not yet been called.</exception>
    public static Config Config
    {
        get
        {
            if (_config == null)
            {
                throw new Exception("JAngine.Engine.Start() has not been called yet.");
            }
            return _config;
        }
    }

    /// <summary>
    /// Initializes the engine with configuration and assemblies.
    /// If no assemblies are provided the calling assembly will get added automatically.
    /// </summary>
    /// <param name="config">Configuration of the engine.</param>
    /// <param name="assemblies">Assemblies to add to the engine,
    /// if no assemblies are provided the engine will use the calling assembly.</param>
    public static void Start(Config config, params Assembly[] assemblies)
    {
        if (_config == null)
        {
            return;
        }
        _config = config;
        
        if (assemblies.Length == 0)
        {
            AddAssembly(Assembly.GetCallingAssembly());
        }
        foreach (Assembly assembly in assemblies)
        {
            AddAssembly(assembly);
        }
    }

    /// <summary>
    /// Adds an assembly to the engine, finds all used classes and methods to use in the engine.
    /// </summary>
    /// <param name="assembly">The assembly to add, if none is provided the calling assembly is used.</param>
    public static void AddAssembly(Assembly? assembly = null)
    {
        if (assembly == null)
        {
            assembly = Assembly.GetCallingAssembly();
        }
        
    }
}