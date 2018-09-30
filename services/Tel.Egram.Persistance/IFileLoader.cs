using System;
using TdLib;

namespace Tel.Egram.Persistance
{
    public interface IFileLoader
    {
        IObservable<TdApi.File> LoadFile(TdApi.File file, LoadPriority priority);
    }
}