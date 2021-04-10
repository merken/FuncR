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
                (nameof(IFooService.Foo))
                // string Foo(int numberOfFoos)
                .Runs<int, string>(n =>
                {
                    if(n >= 5)
                        return "Too many foos!";

                    return $"This many foos: '{n}'";
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