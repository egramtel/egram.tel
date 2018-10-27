using System;
using System.Reactive.Disposables;

namespace Tel.Egram.Components
{
    public abstract class BaseController : IDisposable
    {
        internal readonly CompositeDisposable ControllerDisposable = new CompositeDisposable();
        
        public virtual void Dispose()
        {
            ControllerDisposable.Dispose();
        }
    }

    public abstract class BaseController<T> : BaseController
        where T : class, new()
    {
        public T Model { get; } = new T();

        public override void Dispose()
        {
            base.Dispose();
            (Model as IDisposable)?.Dispose();
        }
    }
}