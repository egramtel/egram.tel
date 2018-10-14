using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Tel.Egram.Components.Messenger.Explorer
{
    public class ExplorerTrigger : IExplorerTrigger
    {
        private readonly Subject<ExplorerSignal> _subject = new Subject<ExplorerSignal>();
        
        public IDisposable Subscribe(IObserver<ExplorerSignal> observer)
        {
            return _subject.Subscribe(observer);
        }

        public IDisposable Trigger(ExplorerSignal signal)
        {
            return Observable.Timer(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ =>
                {
                    _subject.OnNext(signal);
                });
        }
    }
}