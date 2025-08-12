using Spectre.Console.Cli;

using System;

namespace SpectreConsoleCliPlugins;

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    private readonly IServiceProvider provider;

    public TypeResolver(IServiceProvider provider)
    {
        this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public object? Resolve(Type? type)
    {
        return type == null ? null : this.provider.GetService(type);
    }

    public void Dispose()
    {
        if (this.provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}