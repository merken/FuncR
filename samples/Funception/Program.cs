using System;
using FuncR;
using Microsoft.Extensions.DependencyInjection;

namespace Funception
{
    interface ILevel3
    {
        string Foo(string bar);
    }

    class Level3 : ILevel3
    {
        public string Foo(string bar)
        {
            return $"Level 3 says: '{bar}'";
        }
    }

    interface ILevel2
    {
        string Foo(string bar);
    }

    interface ILevel1
    {
        string Foo(string bar);
    }

    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services
                .AddScopedFunction<ILevel1>
                    // Implements                  string Foo(string bar)
                    (nameof(ILevel1.Foo)).Runs<IServiceProvider, string, string>((sp, bar) =>
                {
                    var level2 = sp.GetRequiredService<ILevel2>();
                    return level2.Foo(bar);
                })
                .AddScopedFunction<ILevel2>
                    // Implements                  string Foo(string bar)
                    (nameof(ILevel2.Foo)).Runs<IServiceProvider, string, string>((sp, bar) =>
                {
                    var level3 = sp.GetRequiredService<ILevel3>();
                    return level3.Foo(bar);
                });

            // Register third-level service
            services.AddScoped<ILevel3, Level3>();

            var myServiceWithoutImplementation =
                services.BuildServiceProvider().GetRequiredService<ILevel1>();

            // Prints: "Level 3 says: 'Bar'"
            Console.WriteLine(myServiceWithoutImplementation.Foo("Bar"));
        }
    }
}