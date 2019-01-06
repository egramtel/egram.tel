using System;
using Avalonia.Media.Imaging;
using TdLib;
using Tel.Egram.Services.Persistance;

namespace Tel.Egram.Services.Graphics
{
    public interface IBitmapLoader
    {
        IObservable<IBitmap> LoadFile(TdApi.File file, LoadPriority priority);
    }
}