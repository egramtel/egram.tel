using System;
using System.Reactive.Disposables;

namespace Tel.Egram.Components
{
    public static class DisposableExtensions
    {
        public static void DisposeWith(this IDisposable disposable, BaseController controller)
        {
            disposable.DisposeWith(controller.ControllerDisposable);
        }
    }
}