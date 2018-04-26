using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Egram.Components.App
{
    public class BackgroundWorker
    {
        private readonly TD.Hub _hub;

        public BackgroundWorker(TD.Hub hub)
        {
            _hub = hub;
        }

        public IDisposable Init()
        {
            StartHub();
            
            return Disposable.Create(() =>
            {
                StopHub();
            });
        }

        private void StartHub()
        {
            Task.Run(() => _hub.Start())
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        // TODO: log
                        throw task.Exception;
                    }
                });
        }

        private void StopHub()
        {
            _hub.Stop();
        }
    }
}