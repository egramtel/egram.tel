using System.Reactive.Concurrency;
using ReactiveUI;

namespace Tel.Egram.Utils
{
    public class Schedulers : ISchedulers
    {
        public IScheduler Main => RxApp.MainThreadScheduler;

        public IScheduler Pool => TaskPoolScheduler.Default;
    }
}