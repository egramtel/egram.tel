﻿using System;
using System.IO;

namespace Tel.Egram.Persistance
{
    public class Storage : IStorage
    {
        private Storage(string directory)
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

            DataDirectory = Path.Combine(CacheDirectory, "data");
            Directory.CreateDirectory(DataDirectory);
            
            DatabaseFile = Path.Combine(DataDirectory, "app.db");
        }
        
        public Storage() : this("Egram")
        {
        }

        public string BaseDirectory { get; }
        
        public string LogDirectory { get; }
        
        public string TdLibDirectory { get; }
        
        public string CacheDirectory { get; }
        
        public string DataDirectory { get; }
        
        public string DatabaseFile { get; }
    }
}