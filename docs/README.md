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

<p align="center"> FuncR, small function runner
    <br/> 
</p>

## 📝 Table of Contents
- [📝 Table of Contents](#-table-of-contents)
- [🧐 About](#-about)
- [🏁 Getting Started](#-getting-started)
- [📜 Examples](#-examples)
- [✍️ Authors](#️-authors)

## 🧐 About
<a name="about"></a>

**FuncR**, skip the implementations, go for functions!
This library lets you register small **Func**'s for the methods of your Service, omitting the need for you to implement that interface in form of a class.
FuncR will generate a **DispatchProxy** and register all **Func**'s that you provide. It also allows you to leverage existing services via DependencyInjection by providing you with the **IServiceProvider**!

```csharp
// We don't need no, implementation
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
        Console.WriteLine(myServiceWithoutImplementation.Foo("Bar");
    }
}
```

## 🏁 Getting Started
<a name="getting-started"></a>
Add FuncR to your main application (.NET Web Application, Console app, ...)
```
dotnet add package FuncR
```

## 📜 Examples
<a name="examples"></a>

- [🌤️ Your first function](https://github.com/merken/FuncR/tree/main/samples/Your.First.Function)
- [🧮 Calculator app](https://github.com/merken/FuncR/tree/main/samples/Calculator)
- [♻️ Funception](https://github.com/merken/FuncR/tree/main/samples/Funception)

## ✍️ Authors
<a name="authors"></a>

- [@merken](https://github.com/merken) - Idea & Initial work