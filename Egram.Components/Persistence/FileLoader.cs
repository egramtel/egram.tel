using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Egram.Components.TDLib;

namespace Egram.Components.Persistence
{
    public class FileLoader : IDisposable
    {
        private readonly IAgent _agent;
        private readonly ConcurrentDictionary<int, TaskCompletionSource<TD.File>> _downloadingFiles;
        private readonly IDisposable _fileUpdateSubscription;

        public FileLoader(IAgent agent)
        {
            _agent = agent;
            _downloadingFiles = new ConcurrentDictionary<int, TaskCompletionSource<TD.File>>();

            _fileUpdateSubscription = Observable.FromEventPattern<TD.Structure>(
                    h => _agent.Received += h,
                    h => _agent.Received -= h
                )
                .Select(s => s.EventArgs as TD.Update.UpdateFile)
                .Where(u => u != null)
                .Subscribe(ObserveFileUpdate);
        }

        private void ObserveFileUpdate(TD.Update.UpdateFile updateFile)
        {
            if (!string.IsNullOrWhiteSpace(updateFile?.File?.Local?.Path))
            {
                if (_downloadingFiles.TryRemove(updateFile.File.Id, out var tcs))
                {
                    tcs.SetResult(updateFile.File);
                }
            }
        }

        public async Task<TD.LocalFile> LoadFileAsync(TD.File file)
        {
            if (file == null)
            {
                return null;
            }
            
            if (!File.Exists(file?.Local?.Path))
            {
                var tcs = new TaskCompletionSource<TD.File>();

                if (_downloadingFiles.TryAdd(file.Id, tcs))
                {
                    file = await _agent.ExecuteAsync(new TD.DownloadFile
                    {
                        FileId = file.Id,
                        Priority = 32 // highest
                    });

                    file = await tcs.Task;
                }
                else if (_downloadingFiles.TryGetValue(file.Id, out tcs))
                {
                    file = await tcs.Task;
                }
            }
            
            return File.Exists(file?.Local?.Path) ? file.Local : null;
        }

        public void Dispose()
        {
            _fileUpdateSubscription?.Dispose();
        }
    }
}
