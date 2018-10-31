using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Tel.Egram.Components
{
    public abstract class Controller<T> : Controller, IController<T>
        where T : new()
    {
        internal readonly CompositeDisposable ControllerDisposable = new CompositeDisposable();
        
        public T Model { get; } = new T();

        public override void Dispose()
        {
            base.Dispose();
            ControllerDisposable.Dispose();
            (Model as IDisposable)?.Dispose();
        }
    }

    public abstract class Controller : IController
    {
        private readonly HashSet<IController> _controllers = new HashSet<IController>();
        
        public T Activate<T>(ref IController<T> controller)
        {
            Deactivate(ref controller);
            var model = ControllerActivator.Instance.Activate(ref controller);
            if (controller != null)
            {
                _controllers.Add(controller);
            }
            return model;
        }

        public T Activate<T, TArg>(TArg arg, ref IController<T> controller)
        {
            Deactivate(ref controller);
            var model = ControllerActivator.Instance.Activate(arg, ref controller);
            if (controller != null)
            {
                _controllers.Add(controller);
            }
            return model;
        }

        public T Deactivate<T>(ref IController<T> controller)
        {
            if (controller != null)
            {
                _controllers.Remove(controller);
            }
            return ControllerActivator.Instance.Deactivate(ref controller);
        }

        public virtual void Dispose()
        {
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
            
            _controllers.Clear();
        }
    }
}