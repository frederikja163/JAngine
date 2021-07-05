using JAngine.Rendering.LowLevel;
using System;
using System.Collections.Generic;
using System.IO;

namespace JAngine
{
    public static class Assets
    {
        public delegate void CacheObjectDelegate(StreamReader reader, StreamWriter writer);
        public delegate object LoadObjectDelegate(Window window, StreamReader reader);
        
        private record Asset(
            Type Type,
            CacheObjectDelegate CacheObject,
            LoadObjectDelegate LoadObject);

        private static readonly Dictionary<Type, Asset> _assets = new ();

        static Assets()
        {
            if (!Directory.Exists(GetCachePath("")))
            {
                DirectoryInfo info = Directory.CreateDirectory(GetCachePath(""));
                info.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            Assets.AddType<ShaderProgram>(ShaderProgram.CreateCache, ShaderProgram.CreateObject);
            Assets.AddType<Texture>(Texture.CreateCache, Texture.CreateObject);
        }

        public static void AddType<T>(CacheObjectDelegate cacheObject, LoadObjectDelegate loadObject)
        {
            _assets.TryAdd(typeof(T), new Asset(typeof(T), cacheObject, loadObject));
        }

        public static T Load<T>(Window window, string path)
        {
            if (!_assets.TryGetValue(typeof(T), out Asset? asset))
            {
                throw new Exception($"Asset type {typeof(T)} not added yet!");
            }

            if (!CacheExists(path) || IsCacheOutdated(path))
            {
                CreateCacheFile<T>(path);
            }

            using StreamReader reader = new StreamReader(GetCachePath(path));
            return (T)asset.LoadObject(window, reader);
        }

        public static bool CacheExists(string path)
        {
            return Directory.Exists(GetCachePath(path));
        }
        
        public static bool IsCacheOutdated(string path)
        {
            string assetPath = GetAssetPath(path);
            FileInfo assetInfo = new FileInfo(assetPath);
            
            string cachePath = GetCachePath(path);
            FileInfo cacheInfo = new FileInfo(cachePath);

            return assetInfo.LastWriteTime > cacheInfo.LastWriteTime;
        }

        public static void CreateCacheFile<T>(string path)
        {
            if (!_assets.TryGetValue(typeof(T), out Asset? asset))
            {
                throw new Exception($"Asset type {typeof(T)} not added yet!");
            }

            using StreamReader reader = new StreamReader(GetAssetPath(path));

            string cachePath = GetCachePath(path);
            string cacheDirectoryName = Path.GetDirectoryName(cachePath)!;
            Directory.CreateDirectory(cacheDirectoryName);
            using StreamWriter writer = File.CreateText(cachePath);

            asset!.CacheObject(reader, writer);
        }

        private static string GetAssetPath(string path)
        {
            return Path.Combine("Assets", path);
        }

        private static string GetCachePath(string path)
        {
            return Path.Combine("Assets", ".Cache", path + ".cache");
        }
    }
}