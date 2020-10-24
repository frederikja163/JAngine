using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JAngine
{
    public static class Assets
    {
        private static Dictionary<string, (string path, Assembly assembly)> _assets =
            new Dictionary<string, (string path, Assembly assembly)>();
        
        public static void AddAssets(Assembly assembly, string assetFolderPath = null)
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

        public static StreamReader GetAsset(string name)
        {
            name = name.Replace('/', '.');
            if (!_assets.TryGetValue(name, out var asset))
            {
                asset = ("", Assembly.GetCallingAssembly());
            }

            var path = string.IsNullOrWhiteSpace(asset.path) ? name : asset.path + '.' + name;
            var stream = asset.assembly.GetManifestResourceStream(path);
            if (stream == null)
            {
                throw Log.Error($"Failed to load {name}.");
            }
            
            return new StreamReader(stream);;
        }
    }
}