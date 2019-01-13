using System.IO;

namespace Tel.Egram.Services.Persistance
{
    public interface IFileExplorer
    {
        void Open(DirectoryInfo directory);
    }
}