using System;
using FuncR;
using Microsoft.Extensions.DependencyInjection;

namespace Your.First.Function
{
    public interface IServiceWithoutImplementation
    {
        string Foo(string bar);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScopedFunction<IServiceWithoutImplementation>
                // Implements                  string Foo(string bar)
                (nameof(IServiceWithoutImplementation.Foo)).Runs<string, string>(bar =>
                {
                    return $"From func '{bar}'";
                });

            var myServiceWithoutImplementation =
                services.BuildServiceProvider().GetRequiredService<IServiceWithoutImplementation>();

            // Prints: "From func 'Bar'"
            Console.WriteLine(myServiceWithoutImplementation.Foo("Bar"));
        }
    }
}