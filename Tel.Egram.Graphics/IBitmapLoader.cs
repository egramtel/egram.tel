using System;
using Avalonia.Media.Imaging;
using TdLib;
using Tel.Egram.Persistance;

namespace Tel.Egram.Graphics
{
    public interface IBitmapLoader
    {
        IObservable<IBitmap> LoadFile(TdApi.File file, LoadPriority priority);
    }
}