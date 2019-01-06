using System;
using TdLib;

namespace Tel.Egram.Services.Persistance
{
    public interface IFileLoader
    {
        IObservable<TdApi.File> LoadFile(TdApi.File file, LoadPriority priority);
    }
}