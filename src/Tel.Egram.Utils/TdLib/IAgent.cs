using System;
using TdLib;

namespace Tel.Egram.Utils.TdLib
{
    public interface IAgent
    {
        IObservable<TdApi.Update> Updates { get; }

        IObservable<T> Execute<T>(TdApi.Function<T> function)
            where T : TdApi.Object;
    }
}