using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JAngine.Rendering;

namespace JAngine
{
    public interface IAsset
    {}
    
    public static class Assets
    {
        private static Dictionary<string, (string path, Assembly assembly)> _assets =
            new Dictionary<string, (string path, Assembly assembly)>();
        private static Dictionary<Type, Func<StreamReader, IAsset>> _loaders =
            new Dictionary<Type, Func<StreamReader, IAsset>>(new[]
            {
                new KeyValuePair<Type, Func<StreamReader, IAsset>>(typeof(VertexShader), sr => new VertexShader(sr)), 
                new KeyValuePair<Type, Func<StreamReader, IAsset>>(typeof(FragmentShader), sr => new FragmentShader(sr)), 
            });

        public static void AddLoader<T>(Func<StreamReader, IAsset> loader) where T : IAsset
        {
            if (!_loaders.TryAdd(typeof(T), loader))
            {
                Log.Warning($"Loader already added for type {typeof(T)}");
            }
        }

        public static void AddAssets(Assembly assembly, string assetFolderPath = "")
        {
            assetFolderPath = assetFolderPath.Replace('/', '.');
            var assets = assembly.GetManifestResourceNames();
            foreach (var asset in assets)
            {
                var startIdx = asset.IndexOf(assetFolderPath + '.', StringComparison.Ordinal) + assetFolderPath.Length + 1;
                var assetName = asset.Substring(startIdx);
                if (!_assets.TryAdd(assetName, (assembly.GetName().Name + assetFolderPath, assembly)))
                {
                    Log.Warning($"Could not find asset {assetName}.");
                }
            }
        }

        public static T GetAsset<T>(string name) where T : IAsset
        {
            name = name.Replace('/', '.');
            if (!_assets.TryGetValue(name, out var asset))
            {
                asset = ("", Assembly.GetCallingAssembly());
            }

            var path = string.IsNullOrWhiteSpace(asset.path) ? asset.assembly.GetName().Name + '.' +  name : asset.path + '.' + name;
            var stream = asset.assembly.GetManifestResourceStream(path);
            if (stream == null)
            {
                throw new Exception($"Failed to load {name}.");
            }

            if (!_loaders.TryGetValue(typeof(T), out var loader))
            {
                throw new Exception($"Failed to find loader for asset type {typeof(T)}.");
            }
            var sr = new StreamReader(stream);
            return (T)loader.Invoke(sr);
        }
    }
}