using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FuncR
{
    public class FunctionBuilder
    {
        protected Type type;
        protected IServiceCollection services;
        protected MethodInfo method;
        protected Delegate function;
        protected string name;
        protected string internalName;

        public Type Type => type;

        protected FunctionBuilder(string name, IServiceCollection services)
        {
            this.internalName = Guid.NewGuid().ToString();
            this.name = name;
            this.services = services;
        }

        protected MethodInfo GetMethod(Type targetType, string name, Type returnType, params ParameterInfo[] parameters)
        {
            bool isNameCorrect(MethodInfo targetMethod) => targetMethod.Name == name;

            bool isParameterCountCorrect(MethodInfo targetMethod) =>
                targetMethod.GetParameters().Count() == parameters.Length;

            bool isReturnTypeCorrect(MethodInfo targetMethod) => targetMethod.ReturnType == returnType;

            bool doAllParametersMatch(MethodInfo targetMethod)
            {
                var callingMethodParameters = parameters;
                var targetMethodParameters = targetMethod.GetParameters();
                for (var index = 0; index < callingMethodParameters.Count(); index++)
                {
                    var callingParam = callingMethodParameters[index];
                    var targetParam = targetMethodParameters[index];
                    if (!(targetParam.Name == callingParam.Name &&
                          targetParam.ParameterType.Name == callingParam.ParameterType.Name))
                        return false;
                }

                return true;
            }

            var methods = targetType.GetMethods().AsEnumerable();

            methods = methods.Where(targetMethod =>
                isNameCorrect(targetMethod) &&
                isParameterCountCorrect(targetMethod) &&
                isReturnTypeCorrect(targetMethod) &&
                doAllParametersMatch(targetMethod)
            );

            if (!methods.Any())
                throw new ArgumentException($"Function {this.name} could not be found on type {targetType.Name}");

            if (methods.Count() > 1)
                throw new ArgumentException($"Function {this.name} could not be determined on type {targetType.Name}");

            return methods.First();
        }

        internal FunctionInfo Build() => new FunctionInfo()
        {
            MethodInfo = this.method,
            Function = this.function
        };
    }

    public class FunctionBuilder<T> : FunctionBuilder
    {
        public override string ToString() => this.internalName;

        public FunctionBuilder(string name, IServiceCollection services) : base(name, services)
        {
            this.type = typeof(T);
        }

        public IServiceCollection Runs<TResult>(Func<IServiceProvider, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, TResult>(Func<IServiceProvider, P1, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, TResult>(Func<IServiceProvider, P1, P2, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, TResult>(Func<IServiceProvider, P1, P2, P3, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, TResult>(Func<IServiceProvider, P1, P2, P3, P4, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, P6, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, P6, P7, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, P6, P7, P8, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, P9, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, P6, P7, P8, P9, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, TResult>(
            Func<IServiceProvider, P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<TResult>(Func<TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, TResult>(Func<P1, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, TResult>(Func<P1, P2, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, TResult>(Func<P1, P2, P3, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, TResult>(Func<P1, P2, P3, P4, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, TResult>(
            Func<P1, P2, P3, P4, P5, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, TResult>(
            Func<P1, P2, P3, P4, P5, P6, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, TResult>(
            Func<P1, P2, P3, P4, P5, P6, P7, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, TResult>(
            Func<P1, P2, P3, P4, P5, P6, P7, P8, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, P9, TResult>(
            Func<P1, P2, P3, P4, P5, P6, P7, P8, P9, TResult> func)
            => RegisterFunction(func);

        public IServiceCollection Runs<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, TResult>(
            Func<P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, TResult> func)
            => RegisterFunction(func);

        private IServiceCollection RegisterFunction(Delegate func)
        {
            var parametersExcludingServiceProvider =
                func.Method
                    .GetParameters()
                    .Where(p => p.ParameterType != typeof(IServiceProvider))
                    .ToArray();
            this.method = GetMethod(typeof(T), this.name, func.Method.ReturnType, parametersExcludingServiceProvider);
            this.function = func;
            return services;
        }
    }
}