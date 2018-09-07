using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TdLib;
using Tel.Egram.TdLib;

namespace Tel.Egram.Persistance
{
    public class FileLoader : IFileLoader, IDisposable
    {
        private readonly TdAgent _agent;
        private readonly ConcurrentDictionary<int, Subject<TdApi.File>> _files;
        private readonly IDisposable _updatesSubscription;

        public FileLoader(TdAgent agent)
        {
            _agent = agent;
            _files = new ConcurrentDictionary<int, Subject<TdApi.File>>();
            _updatesSubscription = agent.Updates.OfType<TdApi.Update.UpdateFile>()
                .Subscribe(HandleUpdate);
        }

        public IObservable<TdApi.File> LoadFile(TdApi.File file, LoadPriority priority)
        {
            bool downloadingNeeded = file.Local == null
                                     || !file.Local.IsDownloadingCompleted
                                     || file.Local.Path == null
                                     || !File.Exists(file.Local.Path);
            
            if (downloadingNeeded)
            {
                bool isNew = false;
                var subject = _files.GetOrAdd(file.Id, id =>
                {
                    isNew = true;
                    return new Subject<TdApi.File>();
                });

                return isNew
                    ? _agent.Execute(new TdApi.DownloadFile { FileId = file.Id, Priority = (int) priority })
                            .SelectMany(o => subject)
                    : subject;
            }

            return Observable.Return(file);
        }

        private void HandleUpdate(TdApi.Update.UpdateFile update)
        {
            if (_files.TryGetValue(update.File.Id, out var subject))
            {
                var localFile = update.File?.Local;
                if (localFile != null && localFile.IsDownloadingCompleted)
                {
                    if (_files.TryRemove(update.File.Id, out subject))
                    {
                        subject.OnNext(update.File);
                        subject.OnCompleted();
                    }
                    else
                    {
                        subject.OnNext(update.File);
                    }
                }
            }
        }
        
        public void Dispose()
        {
            _updatesSubscription.Dispose();
        }
    }
}