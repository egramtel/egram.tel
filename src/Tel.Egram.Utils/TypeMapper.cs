using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Tel.Egram.Utils
{
    public class TypeMapper : ITypeMapper
    {
        private Dictionary<Type, Type> _map;

        public TypeMapper(IServiceCollection services)
        {
            _map = services.ToDictionary(
                descriptor => descriptor.ServiceType,
                descriptor => descriptor.ImplementationType);
        }

        public Type this[Type type]
        {
            get
            {
                _map.TryGetValue(type, out var val);
                return val;
            }
        }
    }
}