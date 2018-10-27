using System;
using System.Reactive.Disposables;

namespace Tel.Egram.Components
{
    public abstract class BaseController<T> : IController<T>
        where T : class, new()
    {
        internal readonly CompositeDisposable ControllerDisposable = new CompositeDisposable();
        
        public T Model { get; } = new T();

        public virtual void Dispose()
        {
            ControllerDisposable.Dispose();
            (Model as IDisposable)?.Dispose();
        }
    }
}