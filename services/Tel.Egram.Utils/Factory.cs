using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Tel.Egram.Utils
{
    public sealed class Factory<TResult> : IFactory<TResult> where TResult : class
    {
        private readonly IServiceProvider _provider;

        public Factory(IServiceProvider provider)
        {
            _provider = provider;
        }
        
        public TResult Create()
        {
            var service = _provider.GetService<TResult>();
            if (service == null)
            {
                throw new NullReferenceException($"Service {typeof(TResult).FullName} cannot be resolved");
            }

            return service;
        }
    }

    public sealed class Factory<TParam1, TResult> : IFactory<TParam1, TResult> where TResult : class
    {
        private readonly IServiceProvider _provider;
        private readonly ITypeMapper _typeMapper;

        public Factory(IServiceProvider provider, ITypeMapper typeMapper)
        {
            _provider = provider;
            _typeMapper = typeMapper;
        }
        
        public TResult Create(TParam1 param1)
        {
            var param1Type = typeof(TParam1);
            var resultType = _typeMapper[typeof(TResult)];
            
            var ctor = resultType.GetConstructors()[0];

            var args = ctor.GetParameters()
                .Select(p => p.ParameterType)
                .Select(t => t.IsAssignableFrom(param1Type) ? param1 : _provider.GetService(t))
                .ToArray();

            return (TResult)ctor.Invoke(args);
        }
    }
}