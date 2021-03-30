using System;
using FuncR;
using Microsoft.Extensions.DependencyInjection;

namespace Your.First.Function
{
    // We don't need no, implementation
    public interface IFooService
    {
        string Foo(int numberOfFoos);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.AddScopedFunction<IFooService>
                // Implements                  string Foo(int numberOfFoos)
                (nameof(IFooService.Foo)).Runs<int, string>(numberOfFoos =>
                {
                    if(numberOfFoos >= 5)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });

            var fooService =
                services.BuildServiceProvider().GetRequiredService<IFooService>();

            // Prints: "This many foos: '3'"
            Console.WriteLine(fooService.Foo(3));
            // Prints: "Too many foos!"
            Console.WriteLine(fooService.Foo(5));
        }
    }
}