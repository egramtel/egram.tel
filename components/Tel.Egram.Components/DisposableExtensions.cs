using System;
using System.Reactive.Disposables;

namespace Tel.Egram.Components
{
    public static class DisposableExtensions
    {
        public static void DisposeWith<T>(this IDisposable disposable, Controller<T> controller)
            where T : class, new()
        {
            disposable.DisposeWith(controller.ControllerDisposable);
        }
    }
}