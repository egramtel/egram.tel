using Tel.Egram.Utils;

namespace Tel.Egram.Components
{
    public class Activator<T> : IActivator<T>
        where T : class, new()
    {
        private readonly IFactory<IController<T>> _factory;

        public Activator(IFactory<IController<T>> factory)
        {
            _factory = factory;
        }
        
        public T Activate(ref IController<T> controller)
        {
            controller = _factory.Create();
            return controller.Model;
        }

        public T Deactivate(ref IController<T> controller)
        {
            controller?.Dispose();
            controller = null;
            return null;
        }
    }

    public class Activator<TArg, T> : IActivator<TArg, T>
        where T : class, new()
    {
        private readonly IFactory<TArg, IController<T>> _factory;

        public Activator(IFactory<TArg, IController<T>> factory)
        {
            _factory = factory;
        }
        
        public T Activate(TArg arg, ref IController<T> controller)
        {
            controller = _factory.Create(arg);
            return controller.Model;
        }

        public T Deactivate(ref IController<T> controller)
        {
            controller?.Dispose();
            controller = null;
            return null;
        }
    }
}