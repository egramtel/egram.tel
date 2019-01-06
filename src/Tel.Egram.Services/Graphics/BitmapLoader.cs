using System;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using TdLib;
using Tel.Egram.Services.Persistance;

namespace Tel.Egram.Services.Graphics
{
    public class BitmapLoader : IBitmapLoader, IDisposable
    {
        private readonly IFileLoader _fileLoader;

        public BitmapLoader(IFileLoader fileLoader)
        {
            _fileLoader = fileLoader;
        }

        public IObservable<IBitmap> LoadFile(TdApi.File file, LoadPriority priority)
        {
            return _fileLoader.LoadFile(file, priority)
                .FirstAsync(f => f.Local != null && f.Local.IsDownloadingCompleted)
                .Select(f => new Bitmap(f.Local.Path));
        }
        
        public void Dispose()
        {
        }
    }
}