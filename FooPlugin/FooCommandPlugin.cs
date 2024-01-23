using Spectre.Console.Cli;

using SpectreConsoleCliBase;

namespace FooPlugin;

public class FooCommandPlugin : ICommandPlugin
{
    public void Configure(IConfigurator configurator)
    {
        configurator.AddBranch("foo", environment =>
        {
            environment.SetDescription("foo commands.");
            environment.AddCommand<FooBarCommand>("foobar")
                       .WithDescription("Runs the foobar command.");
            environment.AddCommand<FooBazCommand>("foobaz")
                       .WithDescription("Runs the foobaz command.");
        });
    }
}