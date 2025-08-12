using Spectre.Console.Cli;

using SpectreConsoleCliBase;

namespace FooPlugin;

public class FooCommandPlugin : ICommandPlugin
{
    public void Configure(IConfigurator configurator)
    {
        configurator.AddBranch("foo", config =>
        {
            config.SetDescription("foo commands.");
            config.AddCommand<FooBarCommand>("foobar")
                       .WithDescription("Runs the foobar command.");
            config.AddCommand<FooBazCommand>("foobaz")
                       .WithDescription("Runs the foobaz command.");
        });
    }
}