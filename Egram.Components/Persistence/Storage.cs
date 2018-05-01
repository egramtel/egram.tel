using System;
using System.IO;

namespace Egram.Components.Persistence
{
    public class Storage
    {
        public Storage()
        {
            string appName = "Egram";
            
            BaseDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);
            Directory.CreateDirectory(BaseDirectory);

            LogDirectory = Path.Combine(BaseDirectory, "logs");
            Directory.CreateDirectory(LogDirectory);

            TdlibDirectory = Path.Combine(BaseDirectory, "tdlib");
            Directory.CreateDirectory(TdlibDirectory);

            CacheDirectory = Path.Combine(BaseDirectory, "cache");
            Directory.CreateDirectory(CacheDirectory);

            AvatarCacheDirectory = Path.Combine(CacheDirectory, "avatars");
            Directory.CreateDirectory(AvatarCacheDirectory);
        }

        public string BaseDirectory { get; }

        public string LogDirectory { get; }

        public string TdlibDirectory { get; }
        
        public string CacheDirectory { get; }
        
        public string AvatarCacheDirectory { get; }
    }
}