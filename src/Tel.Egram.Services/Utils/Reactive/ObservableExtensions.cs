using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Tel.Egram.Services.Utils.Reactive
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Aggregate observable into list
        /// </summary>
        public static IObservable<IList<T>> CollectToList<T>(this IObservable<T> observable)
        {
            return observable.Aggregate(new List<T>(), (list, item) =>
            {
                list.Add(item);
                return list;
            });
        }
        
        /// <summary>
        /// Like SelectMany but ordered
        /// </summary>
        public static IObservable<TResult> SelectSeq<T, TResult>(
            this IObservable<T> observable,
            Func<T, IObservable<TResult>> selector)
        {
            return observable.Select(selector).Concat();
        }

        /// <summary>
        /// Subscribe with error handling
        /// </summary>
        public static IDisposable Accept<T>(this IObservable<T> observable, Action<T> onNext)
        {
            return observable.Subscribe(onNext, e => Console.Error.WriteLine(e));
        }
        
        /// <summary>
        /// Subscribe with error handling
        /// </summary>
        public static IDisposable Accept<T>(this IObservable<T> observable)
        {
            return observable.Subscribe(_ => { }, e => Console.Error.WriteLine(e));
        }
    }
}