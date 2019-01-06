using System;
using System.Threading;
using TdLib;

namespace Tel.Egram.Services.Utils.TdLib
{
    public interface IAgent
    {
        IObservable<TdApi.Update> Updates { get; }

        IObservable<T> Execute<T>(TdApi.Function<T> function)
            where T : TdApi.Object;
        
        IObservable<T> Execute<T>(TdApi.Function<T> function, TimeSpan timeout)
            where T : TdApi.Object;

        IObservable<T> Execute<T>(TdApi.Function<T> function, CancellationToken cancellationToken)
            where T : TdApi.Object;
    }
}