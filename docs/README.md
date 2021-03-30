<p align="center">
  <a href="" rel="noopener">
    <img width=150px height=150px src="funcr.png" alt="Project logo">
  </a>
</p>

<h3 align="center">FuncR</h3>

<div align="center">

  [![Status](https://img.shields.io/badge/status-active-success.svg?style=flat-square)]() </br>
  [![GitHub Issues](https://img.shields.io/github/issues/merken/FuncR?style=flat-square)](https://github.com/merken/FuncR/issues) </br>
  [![GitHub Pull Requests](https://img.shields.io/github/issues-pr/merken/FuncR?style=flat-square)](https://github.com/merken/FuncR/pulls) </br>
  [![NuGet Badge](https://img.shields.io/nuget/v/FuncR?label=FuncR&style=flat-square)](https://www.nuget.org/packages/FuncR/) </br>
  [![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](https://github.com/merken/FuncR/blob/main/LICENSE)

</div>

---

<p align="center">From interfaces to functions, with FuncR!
    <br/> 
</p>

## 📝 Table of Contents
- [📝 Table of Contents](#-table-of-contents)
- [🧐 About](#-about)
- [🏁 Getting Started](#-getting-started)
  - [Scoped function](#scoped-function)
  - [Singleton function](#singleton-function)
  - [Async function](#async-function)
  - [Inject services into a function](#inject-services-into-a-function)
  - [Using a function within a function](#using-a-function-within-a-function)
  - [Local functions](#local-functions)
- [📜 Examples](#-examples)
- [✍️ Authors](#️-authors)

## 🧐 About
<a name="about"></a>

**FuncR**, skip the implementations, go for functions!
This library lets you register small **Func**'s for the methods of your Service interface, omitting the need for you to implement that interface in form of a class.
FuncR will generate a **DispatchProxy** and register all **Func**'s that you provide. It also allows you to leverage existing services via DependencyInjection by providing you with the **IServiceProvider**!

```csharp
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
```

## 🏁 Getting Started
<a name="getting-started"></a>
Add FuncR to your main application (.NET Web Application, Console app, ...)
```
dotnet add package FuncR
```

### Scoped function
Add a new ```scoped``` function to your service collection:
```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddScopedFunction<MyServiceInterface>
        ("name of the method you want to create a function for")
        .Runs<TParameter1Type, TReturnType>(
            (parameter1) =>
            {
                TReturnType value = default(TReturnType);
                return value;
            });
}
```

Example:
```csharp
public interface IFooService
{
    string Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddScopedFunction<IFooService>
            ("Foo")
            .Runs<int, string>(
                numberOfFoos =>
                {
                    if(numberOfFoos >= 5)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```

Best to use the actual name of the method instead of ```("Foo")```:
```csharp
public interface IFooService
{
    string Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddScopedFunction<IFooService>
            (nameof(IFooService.Foo)) // Better
            .Runs<int, string>(
                numberOfFoos =>
                {
                    if(numberOfFoos >= 5)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```

### Singleton function
Using a singleton function, as an alternative for a static class + method.
```csharp
public interface IFooService
{
    string Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddSingletonFunction<IFooService>
            (nameof(IFooService.Foo))
            .Runs<int, string>(
                numberOfFoos =>
                {
                    if(numberOfFoos >= 5)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```


### Async function
Using a singleton function, as an alternative for a static class + method.
```csharp
public interface IFooService
{
    Task<string> Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddSingletonFunction<IFooService>
            (nameof(IFooService.Foo))
            .Runs<int, string>(
                async numberOfFoos =>
                {
                    // async operation
                    await Task.Delay(1000);

                    if(numberOfFoos > 5)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```

### Inject services into a function
Make use of dependency injection to resolve services inside a function.
```csharp
public interface IFooLogic
{
    bool IsTooMany(int numberOfFoos);
}

public class FooLogic : IFooLogic
{
    public bool IsTooMany(int numberOfFoos)
    {
        return numberOfFoos >= 5;
    }
}

public interface IFooService
{
    Task<string> Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddScopedFunction<IFooService>
            (nameof(IFooService.Foo))
            .Runs<IServiceProvider, int, string>(
                async (serviceProvider, numberOfFoos) =>
                {
                    await Task.Delay(1000);
                    
                    // Resolve the IFooLogic via the IServiceProvider
                    var fooLogic = serviceProvider.GetRequiredService<IFooLogic>();
                    var isTooMany = fooLogic.IsTooMany(numberOfFoos);

                    if(isTooMany)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```

### Using a function within a function
The injected service can also be a registered function!

```csharp
public interface IFooLogic
{
    bool IsTooMany(int numberOfFoos);
}

public interface IFooService
{
    Task<string> Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    services.AddScopedFunction<IFooLogic>
            (nameof(IFooService.IsTooMany))
            .Runs<int, bool>(
                async (serviceProvider, numberOfFoos) =>
                {
                    return numberOfFoos >= 5;
                });

    services.AddScopedFunction<IFooService>
            (nameof(IFooService.Foo))
            .Runs<IServiceProvider, int, string>(
                async (serviceProvider, numberOfFoos) =>
                {
                    await Task.Delay(1000);
                    
                    var fooLogic = serviceProvider.GetRequiredService<IFooLogic>();
                    var isTooMany = fooLogic.IsTooMany(numberOfFoos);

                    if(isTooMany)
                        return "Too many foos!";

                    return $"This many foos: '{numberOfFoos}'";
                });
}
```


### Local functions
You can also make use of C# local functions.
```csharp
public interface IFooService
{
    string Foo(int numberOfFoos);
}

public void ConfigureServices(IServiceCollection services)
{
    string LocalFoo(int numberOfFoos)
    {
        return $"This many foos: '{numberOfFoos}'";
    }

    services.AddScopedFunction<IFooService>
            (nameof(IFooService.Foo))
            .Runs<int, string>(LocalFoo);
}
```

## 📜 Examples
<a name="examples"></a>

- [🌤️ Your first function](https://github.com/merken/FuncR/tree/main/samples/Your.First.Function)
- [🧮 Calculator app](https://github.com/merken/FuncR/tree/main/samples/Calculator)
- [♻️ Funception](https://github.com/merken/FuncR/tree/main/samples/Funception)

## ✍️ Authors
<a name="authors"></a>

- [@merken](https://github.com/merken) - Idea & Initial work