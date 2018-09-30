using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using TdLib;

namespace Tel.Egram.TdLib
{
    public class TdAgent
    {
        private readonly Hub _hub;
        private readonly Dialer _dialer;

        public TdAgent(Hub hub, Dialer dialer)
        {
            _hub = hub;
            _dialer = dialer;
        }

        public virtual IObservable<TdApi.Update> Updates
        {
            get
            {
                return Observable.FromEventPattern<TdApi.Object>(h => _hub.Received += h, h => _hub.Received -= h)
                    .Select(a => a.EventArgs)
                    .OfType<TdApi.Update>();
            }
        }

        public virtual IObservable<T> Execute<T>(TdApi.Function<T> function)
            where T : TdApi.Object
        {
            return _dialer.ExecuteAsync(function).ToObservable();
        }
    }
}