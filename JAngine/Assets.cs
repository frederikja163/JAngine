using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.AccessControl;
using JAngine.Extensions.Reflection;

namespace JAngine;

/// <summary>
/// The interface of asset loaders, all asset loaders must implement this interface.
/// Classes of this interface will automatically be added to the Assets class, when their assemblies are tracked
/// <see cref="Assemblies.Add"/>.
/// </summary>
public interface IAssetLoader
{
    /// <summary>
    /// The type of asset this loader generates.
    /// </summary>
    public Type AssetType { get; }
    
    /// <summary>
    /// Creates a cache for an asset.
    /// </summary>
    /// <param name="rawStream">The raw stream of the asset, this is readonly.</param>
    /// <param name="cacheStream">The cached stream of the asset, this is write only.</param>
    public void CreateCache(Stream rawStream, Stream cacheStream);
    /// <summary>
    /// Loads a cached asset.
    /// </summary>
    /// <param name="cacheStream">A readonly copy of the cacheStream from <see cref="IAssetLoader.CreateCache"/>.</param>
    /// <returns>Returns the loaded asset of type <see cref="IAssetLoader.AssetType"/>.</returns>
    public object LoadCache(Stream cacheStream);
}

/// <summary>
/// Loads and manages assets.
/// </summary>
public static class Assets
{
    private static readonly Dictionary<Type, IAssetLoader> Loaders = new Dictionary<Type, IAssetLoader>();
    static Assets()
    {
        Assemblies.OnAddAssembly += assembly =>
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsAssignableTo(typeof(IAssetLoader)))
                {
                    continue;
                }

                IAssetLoader? loader = null;
                try
                {
                    loader = (IAssetLoader?)Activator.CreateInstance(type);
                }
                catch
                {
                    // Ignore as loader is already null.
                }
                
                if (loader is null)
                {
                    Log.Warn($"{type.FullName} must contain an empty constructor when inheriting from ${nameof(IAssetLoader)}");
                    continue;
                }

                if (!Loaders.TryAdd(loader.AssetType, loader))
                {
                    Log.Warn($"Multiple loaders found for asset type {type.FullName}:" +
                             $"{loader.GetType().FullName}, {Loaders[loader.AssetType].GetType().FullName}");
                }
            }
        };
    }

    /// <summary>
    /// Tries to get the loader of a specific asset type.
    /// </summary>
    /// <param name="loader">When true; contains the asset loader otherwise null.</param>
    /// <typeparam name="T">The type of the asset loader.</typeparam>
    /// <returns>A boolean containing true if the asset loader was found, otherwise false.</returns>
    public static bool TryGetLoader<T>([NotNullWhen(true)] out IAssetLoader? loader)
    {
        return Loaders.TryGetValue(typeof(T), out loader);
    }

    private static void CreateCache<T>(string path, out IAssetLoader loader)
    {
        if (!TryGetLoader<T>(out IAssetLoader? assetLoader))
        {
            throw new Exception(
                $"Didn't find asset loader for type {typeof(T).FullName} try implementing {nameof(IAssetLoader)}.");
        }
        loader = assetLoader;
        
        Stream? rawStream = null;
        Stream? cacheStream = null;
        try
        {
            // TODO: Consider creating a sub-folder for all cached files instead of renaming them.
            // Should make it easier to remove all cached files.
            string cachePath = CreateCachePath(path);
            if (File.Exists(path))
            {
                rawStream = File.Open(path, FileMode.Open, FileAccess.Read);
            }
            else
            {
                // TODO: Load from embedded resources.
                throw new FileNotFoundException($"Couldn't find file {path} in any assemblies.");
            }
            
            // Don't create cache if it already exists and is newer than raw asset.
            if (File.Exists(cachePath) &&
                !File.Exists(path) || File.GetLastWriteTimeUtc(path) <= File.GetLastWriteTimeUtc(cachePath))
            {
                // TODO: Always create cache if source is embedded resource
                return;
            }
            cacheStream = File.Open(cachePath, FileMode.Create, FileAccess.Write);
            
            // Create the cache.
            loader.CreateCache(rawStream, cacheStream);
        }
        finally
        {
            rawStream?.Dispose();
            cacheStream?.Dispose();
        }
    }

    /// <summary>
    /// Create a cached version of an asset.
    /// </summary>
    /// <param name="path">The path of the asset.</param>
    /// <typeparam name="T">The type of the asset.</typeparam>
    public static void CreateCache<T>(string path)
    {
        CreateCache<T>(path, out _);
    }
    
    /// <summary>
    /// Load a single asset resource.
    /// </summary>
    /// <param name="path">The path of the resource to load.</param>
    /// <typeparam name="T">The type of resource to load.</typeparam>
    /// <returns>The loaded resource.</returns>
    public static T Load<T>(string path)
    {
        CreateCache<T>(path, out IAssetLoader loader);

        string cachePath = CreateCachePath(path);
        using Stream stream = File.Open(cachePath, FileMode.Open, FileAccess.Read);
        return (T)loader.LoadCache(stream);
    }

    private static string CreateCachePath(string path)
    {
        return path + ".cache";
    }
}