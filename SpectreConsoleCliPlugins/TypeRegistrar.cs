using Microsoft.Extensions.DependencyInjection;

using Spectre.Console.Cli;

using System;

namespace SpectreConsoleCliPlugins;

public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection services;

    public TypeRegistrar()
    {
        this.services = new ServiceCollection();
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(this.services.BuildServiceProvider());
    }

    public void Register(Type service, Type implementation)
    {
        this.services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        this.services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        this.services.AddSingleton(service, _ => factory());
    }
}
