using System.Reactive.Concurrency;

namespace Tel.Egram.Utils
{
    public interface ISchedulers
    {
        IScheduler Main { get; }
        IScheduler Pool { get; }
    }
}