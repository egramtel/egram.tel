using System;
using System.IO;

namespace Tel.Egram.Persistance
{
    public class Storage : IStorage
    {
        public Storage(string directory)
        {
            BaseDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), directory);
            Directory.CreateDirectory(BaseDirectory);

            LogDirectory = Path.Combine(BaseDirectory, "logs");
            Directory.CreateDirectory(LogDirectory);

            TdLibDirectory = Path.Combine(BaseDirectory, "tdlib");
            Directory.CreateDirectory(TdLibDirectory);

            CacheDirectory = Path.Combine(BaseDirectory, "cache");
            Directory.CreateDirectory(CacheDirectory);
            
            AvatarDirectory = Path.Combine(CacheDirectory, "avatars");
            Directory.CreateDirectory(AvatarDirectory);
        }

        public string BaseDirectory { get; }
        
        public string LogDirectory { get; }
        
        public string TdLibDirectory { get; }
        
        public string CacheDirectory { get; }
        
        public string AvatarDirectory { get; }
    }
}