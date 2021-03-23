using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FuncR
{
    public interface IFunctionProxy
    {
        void AddFunction(FunctionInfo functionInfo);
    }

    public class FunctionProxy<T> : DispatchProxy, IDisposable, IFunctionProxy
    {
        protected bool disposed = false;
        protected object remoteObject;
        private IServiceProvider serviceProvider;
        private Dictionary<MethodInfo, FunctionInfo> functions;

        public static object New(IServiceProvider serviceProvider)
        {
            var proxy = Create<T, FunctionProxy<T>>();
            (proxy as FunctionProxy<T>).Initialize(serviceProvider);
            return proxy;
        }

        private void Initialize(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.functions = new Dictionary<MethodInfo, FunctionInfo>();
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (!this.functions.ContainsKey(targetMethod))
                throw new NotImplementedException(
                    $"Function {targetMethod.Name} was not registered on type {typeof(T).Name}");
                    
            var method = this.functions[targetMethod];
            var argsForFunction = args;
            if (method.Function.Method.GetParameters().Any(p => p.ParameterType == typeof(IServiceProvider)))
                argsForFunction = new[]
                {
                    this.serviceProvider
                }.Union(args).ToArray();

            return method.Function.DynamicInvoke(argsForFunction);
        }

        public void AddFunction(FunctionInfo functionInfo)
        {
            this.functions[functionInfo.MethodInfo] = functionInfo;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
                this.remoteObject = null;
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}