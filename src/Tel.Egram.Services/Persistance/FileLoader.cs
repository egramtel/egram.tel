using System;
using System.IO;
using System.Reactive.Linq;
using TdLib;
using Tel.Egram.Services.Utils.Reactive;
using Tel.Egram.Services.Utils.TdLib;

namespace Tel.Egram.Services.Persistance
{
    public class FileLoader : IFileLoader
    {
        private readonly IAgent _agent;

        public FileLoader(IAgent agent)
        {
            _agent = agent;
        }

        public IObservable<TdApi.File> LoadFile(TdApi.File file, LoadPriority priority)
        {
            if (IsDownloadingNeeded(file))
            {
                return _agent.Execute(new TdApi.DownloadFile
                    {
                        FileId = file.Id,
                        Priority = (int) priority
                    })
                    .SelectSeq(downloading =>
                    {
                        return _agent.Updates
                            .OfType<TdApi.Update.UpdateFile>()
                            .Select(u => u.File)
                            .Where(f => f.Id == downloading.Id)
                            .TakeWhile(f => IsDownloadingNeeded(f));
                    })
                    .Concat(Observable.Defer(() => _agent.Execute(new TdApi.GetFile
                    {
                        FileId = file.Id
                    })));
            }

            return Observable.Return(file);
        }

        private bool IsDownloadingNeeded(TdApi.File file)
        {
            return file.Local == null
                || !file.Local.IsDownloadingCompleted
                || file.Local.Path == null
                || !File.Exists(file.Local.Path);
        }
    }
}