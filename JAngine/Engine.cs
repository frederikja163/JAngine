using System.Reflection;
using OpenTK.Windowing.Desktop;

namespace JAngine;

/// <summary>
/// Configuration for the engine.
/// </summary>
public sealed class Config
{
    /// <summary>
    /// Is the engine in debug mode?
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
    private static readonly HashSet<Assembly> Assemblies = new();
    public delegate void AssemblyAddedDelegate(Assembly assembly);
    private static AssemblyAddedDelegate? _onAssemblyAdded = null;
    private static readonly Queue<Thread> Threads = new();

    public static event AssemblyAddedDelegate? OnAssemblyAdded
    {
        add
        {
            _onAssemblyAdded += value;
            foreach (Assembly assembly in Assemblies)
            {
                value?.Invoke(assembly);
            }
        }
        remove => _onAssemblyAdded -= value;
    }

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
        if (_config != null)
        {
            Log.Warning("Engine already started, no need to start it again!");
            return;
        }
        Log.Info("Starting engine.");
        _config = config;
        
        OnAssemblyAdded += BootstrapRunner.OnAddAssembly;
        if (assemblies.Length == 0)
        {
            AddAssembly(Assembly.GetExecutingAssembly());
            AddAssembly(Assembly.GetCallingAssembly());
        }
        foreach (Assembly assembly in assemblies)
        {
            AddAssembly(assembly);
        }

        while (Threads.TryDequeue(out Thread? thread))
        {
            thread.Join();
        }
        Log.Info("Engine execution finished successfully.");
    }

    public static void AddWindow(GameWindow window)
    {
        // TODO: Allow adding non window threads, ex an update thread.
        Thread thread = new Thread(window.Run);
        thread.Start();
        Threads.Enqueue(thread);
        Log.Trace($"Added new window {window}");
    }

    /// <summary>
    /// Adds an assembly to the engine, finds all used classes and methods to use in the engine.
    /// </summary>
    /// <param name="assembly">The assembly to add, if none is provided the calling assembly is used.</param>
    public static void AddAssembly(Assembly? assembly = null)
    {
        Log.Trace("Adding assembly to the engine.");
        if (assembly == null)
        {
            assembly = Assembly.GetCallingAssembly();
            Log.Trace("Null assembly not found, defaulting to calling assembly.");
        }

        if (Assemblies.Contains(assembly))
        {
            Log.Warning($"Assembly added twice {assembly.FullName}.");
        }
        else
        {
            Assemblies.Add(assembly);
            _onAssemblyAdded?.Invoke(assembly);
            Log.Trace($"Assembly added successfully {assembly.FullName}.");
        }
    }
}