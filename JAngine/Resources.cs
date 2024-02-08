using System.Diagnostics;
using System.Reflection;
using JAngine.Rendering;

namespace JAngine;

/// <summary>
/// Provides the ability to load resources of a specific kind.
/// </summary>
/// <typeparam name="T">The type of resource to load.</typeparam>
public interface IResourceLoader<out T>
{
    /// <summary>
    /// Loads a single resource of the specified type from a stream.
    /// </summary>
    /// <param name="window">The window this resource will belong to.</param>
    /// <param name="filePath">The file path of the loaded file.</param>
    /// <param name="stream">The stream to load the resource from.</param>
    /// <returns>The loaded resource.</returns>
    public T Load(Window window, string filePath, Stream stream);
}

/// <summary>
/// Utility for a single type of resource.
/// </summary>
/// <typeparam name="T">The type of resource to provide utility for.</typeparam>
public static class Resource
{
    private static readonly Dictionary<string, Assembly> s_resourcePaths = new();

    static Resource()
    {
        foreach (Assembly assembly in Assemblies.AllAssemblies())
        {
            foreach (string name in assembly.GetManifestResourceNames())
            {
                s_resourcePaths.Add(name, assembly);
            }
        }
    }

    internal static Stream GetPath(string path)
    {
        string p = path.Replace(Path.DirectorySeparatorChar, '.');
        if (s_resourcePaths.TryGetValue(p, out Assembly? assembly))
        {
            return assembly.GetManifestResourceStream(p) ?? throw new UnreachableException();
        }

        if (File.Exists(path))
        {
            return File.Open(path, FileMode.Open, FileAccess.Read);
        }

        throw new FileNotFoundException($"Couldn't find resource {path}");
    }
    
    /// <summary>
    /// Load a resource from path. Either as an embedded resource or as a path from the file system.
    /// </summary>
    /// <param name="window">The window the resource belongs to.</param>
    /// <param name="path">The path to load resources from.</param>
    /// <returns>The loaded resource.</returns>
    public static T Load<T>(Window window, string path)
    {
        return Resource<T>.Load(window, path);
    }
}

// Handles resource loading for a single resource type.
internal static class Resource<T>
{
    private static readonly IResourceLoader<T> s_resourceLoader;
    
    static Resource()
    {
        try
        {
            s_resourceLoader = Assemblies.CreateInstances<IResourceLoader<T>>().First();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"No resource loader found for {typeof(T).FullName}. Create a class implementing {nameof(IResourceLoader<T>)}<{typeof(T).FullName}> to fix this.", ex);
        }
    }
    
    internal static T Load(Window window, string path)
    {
        using Stream stream = Resource.GetPath(path);
        return s_resourceLoader.Load(window, path, stream);
    }
}
