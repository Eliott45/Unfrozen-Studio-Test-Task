using System;
using System.Collections.Generic;

namespace Infrastructure.Container
{
    public class Container
    {
        private readonly Dictionary<Type, object> _bindings = new();

        public void Bind<T>(T instance)
        {
            _bindings[typeof(T)] = instance;
        }

        public T Resolve<T>()
        {
            if (_bindings.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            throw new Exception($"Dependency of type {typeof(T)} not found in the container.");
        }
    }
}

