﻿using System;
using System.IO;

namespace CmlLib.Core
{
    public class MinecraftPath
    {
        public readonly static string
    MacDefaultPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Application Support/minecraft",
    LinuxDefaultPath = Environment.GetEnvironmentVariable("HOME") + "/.minecraft",
    WindowsDefaultPath = Environment.GetEnvironmentVariable("appdata") + "\\.minecraft";

        public static string GetOSDefaultPath()
        {
            switch (MRule.OSName)
            {
                case "osx":
                    return MacDefaultPath;
                case "linux":
                    return LinuxDefaultPath;
                case "windows":
                    return WindowsDefaultPath;
                default:
                    return Environment.CurrentDirectory;
            }
        }

        public MinecraftPath()
        {
            var basePath = GetOSDefaultPath();
            Initialize(basePath, basePath);
        }

        public MinecraftPath(string basePath)
        {
            Initialize(basePath, basePath);
        }

        public MinecraftPath(string basePath, string basePathForAssets)
        {
            Initialize(basePath, basePathForAssets);
        }

        private void Initialize(string p, string assetsPath)
        {
            BasePath = Dir(p);

            Library = Dir(BasePath + "/libraries");
            Versions = Dir(BasePath + "/versions");
            Resource = Dir(BasePath + "/resources");

            Runtime = Dir(BasePath + "/runtime");
            Assets = Dir(assetsPath + "/assets");
        }

        public string BasePath { get; set; }
        public string Library { get; set; }
        public string Versions { get; set; }
        public string Resource { get; set; }
        public string Assets { get; set; }
        public string Runtime { get; set; }

        public virtual string GetIndexFilePath(string assetId)
            => CreateFileDir($"{Assets}/indexes/{assetId}.json");

        public virtual string GetAssetObjectPath()
            => Dir($"{Assets}/objects");

        public virtual string GetAssetLegacyPath()
            => Dir($"{Assets}/virtual/legacy");

        public virtual string GetVersionJarPath(string id)
            => CreateFileDir($"{Versions}/{id}/{id}.jar");

        public virtual string GetVersionJsonPath(string id)
            => CreateFileDir($"{Versions}/{id}/{id}.json");

        public virtual string GetNativePath(string id)
            => Dir($"{Versions}/{id}/natives");

        public override string ToString()
        {
            return BasePath;
        }

        protected static string Dir(string path)
        {
            var p = Path.GetFullPath(path);
            if (!Directory.Exists(p))
                Directory.CreateDirectory(p);

            return p;
        }

        protected static string CreateFileDir(string file)
        {
            var p = Path.GetFullPath(file);
            if (File.Exists(p))
                return p;

            Dir(Path.GetDirectoryName(p));
            return p;
        }
    }
}
