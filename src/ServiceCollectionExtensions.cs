using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace FuncR
{
    public static class ServiceCollectionExtensions
    {
        public static FunctionBuilder<T> AddScopedFunction<T>(this IServiceCollection services,
            string function)
            where T : class
        {
            return AddFunction<T>(services, ServiceLifetime.Scoped, function);
        }

        public static FunctionBuilder<T> AddSingletonFunction<T>(this IServiceCollection services,
            string function)
            where T : class
        {
            return AddFunction<T>(services, ServiceLifetime.Singleton, function);
        }

        public static FunctionBuilder<T> AddTransientFunction<T>(this IServiceCollection services,
            string function)
            where T : class
        {
            return AddFunction<T>(services, ServiceLifetime.Transient, function);
        }

        private static IList<FunctionBuilder> _functionBuilders = new List<FunctionBuilder>();

        private static FunctionBuilder<T> AddFunction<T>(IServiceCollection services, ServiceLifetime lifetime,
            string function)
            where T : class
        {
            var type = typeof(T);
            if (!type.IsInterface)
                throw new NotSupportedException($"Type {type.Name} must be an interface type.");

            var builder = new FunctionBuilder<T>(function, services);
            _functionBuilders.Add(builder);

            var functionProxyServiceDescriptor =
                new ServiceDescriptor(typeof(T), (sp) =>
                {
                    var functionProxy =
                        (FunctionProxy<T>) FunctionProxy<T>.New(sp);
                    foreach (var builder in _functionBuilders.Where(f => f.Type == typeof(T)))
                        functionProxy.AddFunction(builder.Build());
                    return functionProxy as T;
                }, lifetime);

            services.Add(functionProxyServiceDescriptor);

            return builder;
        }
    }
}