using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using TdLib;

namespace Tel.Egram.Services.Utils.TdLib
{
    public class Agent : IAgent
    {
        private readonly Hub _hub;
        private readonly Dialer _dialer;

        public Agent(Hub hub, Dialer dialer)
        {
            _hub = hub;
            _dialer = dialer;
        }

        public IObservable<TdApi.Update> Updates
        {
            get
            {
                return Observable.FromEventPattern<TdApi.Object>(h => _hub.Received += h, h => _hub.Received -= h)
                    .Select(a => a.EventArgs)
                    .OfType<TdApi.Update>();
            }
        }

        public IObservable<T> Execute<T>(TdApi.Function<T> function)
            where T : TdApi.Object
        {
            return _dialer.ExecuteAsync(function).ToObservable();
        }

        public IObservable<T> Execute<T>(TdApi.Function<T> function, TimeSpan timeout)
            where T : TdApi.Object
        {
            var delay = Task.Delay(timeout)
                .ContinueWith<T>(_ => throw new TaskCanceledException("Execution timeout"));
            
            var task = Task.WhenAny(delay, _dialer.ExecuteAsync(function))
                .ContinueWith(t => t.Result.Result);

            return task.ToObservable();
        }

        public IObservable<T> Execute<T>(TdApi.Function<T> function, CancellationToken cancellationToken)
            where T : TdApi.Object
        {
            var delay = Task.Delay(Timeout.Infinite, cancellationToken)
                .ContinueWith<T>(_ => throw new TaskCanceledException("Execution timeout"));
            
            var task = Task.WhenAny(delay, _dialer.ExecuteAsync(function))
                .ContinueWith(t => t.Result.Result);

            return task.ToObservable();
        }
    }
}