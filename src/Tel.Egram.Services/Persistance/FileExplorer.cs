using System.Diagnostics;
using System.IO;
using Tel.Egram.Services.Utils.Platforms;

namespace Tel.Egram.Services.Persistance
{
    public class FileExplorer : IFileExplorer
    {
        private readonly IPlatform _platform;

        public FileExplorer(
            IPlatform platform)
        {
            _platform = platform;
        }

        public void Open(DirectoryInfo directory)
        {
            switch (_platform)
            {
                case WindowsPlatform _:
                    Process.Start("explorer.exe", directory.FullName);
                    break;
                
                case MacosPlatform _:
                    Process.Start("open", directory.FullName);
                    break;
            }
        }
    }
}