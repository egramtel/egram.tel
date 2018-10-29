using System;
using Microsoft.Extensions.DependencyInjection;
using Tel.Egram.Utils;

namespace Tel.Egram.Components
{
    public class ControllerActivator
    {
        public static ControllerActivator Instance = new ControllerActivator();

        internal IServiceCollection ServiceCollection { get; set; }
        internal IServiceProvider ServiceProvider { get; set; }

        private ControllerActivator() { }
        
        public T Activate<T>(ref IController<T> controller)
        {   
            var factory = ServiceProvider.GetService<IFactory<IController<T>>>();
            controller = factory.Create();
            return controller.Model;
        }
        
        public T Activate<T, TArg>(TArg arg, ref IController<T> controller)
        {   
            var factory = ServiceProvider.GetService<IFactory<TArg, IController<T>>>();
            controller = factory.Create(arg);
            return controller.Model;
        }

        public T Deactivate<T>(ref IController<T> controller)
        {
            controller?.Dispose();
            controller = null;
            return default(T);
        }
    }

    public static class ControllerActivatorExtensions
    {
        public static void SetServiceCollection(
            this ControllerActivator activator,
            IServiceCollection serviceCollection)
        {
            activator.ServiceCollection = serviceCollection;
        }
        
        public static void SetServiceProvider(
            this ControllerActivator activator,
            IServiceProvider serviceProvider)
        {
            activator.ServiceProvider = serviceProvider;
        }
    }
}