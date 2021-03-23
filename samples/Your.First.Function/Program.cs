using System;
using FuncR;
using Microsoft.Extensions.DependencyInjection;

namespace Your.First.Function
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddScopedFunction<IServiceWithoutImplementation>
                // Implements                  string Foo(Bar bar)
                (nameof(IServiceWithoutImplementation.Foo)).Runs<Bar, string>(bar =>
                {
                    return $"From func '{bar}'";
                });

            var myServiceWithoutImplementation =
                services.BuildServiceProvider().GetRequiredService<IServiceWithoutImplementation>();

            // Prints: "From func 'BAR'"
            Console.WriteLine(myServiceWithoutImplementation.Foo(new Bar()));
        }
    }
}