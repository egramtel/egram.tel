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

        /// <summary>
        /// <inheritdoc cref="IFileExplorer"/>
        /// </summary>
        public void OpenDirectory(DirectoryInfo directory)
        {
            switch (_platform)
            {
                case WindowsPlatform _:
                    Process.Start("explorer.exe", $"\"{directory.FullName}\"");
                    break;
                
                case MacosPlatform _:
                    Process.Start("open", $"\"{directory.FullName}\"");
                    break;
            }
        }

        /// <summary>
        /// <inheritdoc cref="IFileExplorer"/>
        /// </summary>
        public void OpenDirectory(FileInfo file)
        {
            switch (_platform)
            {
                case WindowsPlatform _:
                    Process.Start("explorer.exe", $"/select,\"{file.FullName}\"");
                    break;

                case MacosPlatform _:
                    Process.Start("open", $"-R \"{file.FullName}\"");
                    break;
            }
        }
    }
}