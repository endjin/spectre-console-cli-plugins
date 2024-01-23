using Spectre.Console.Cli;
using SpectreConsoleCliBase;

namespace BarPlugin;

public class BarCommandPlugin : ICommandPlugin
{
    public void Configure(IConfigurator configurator)
    {
        configurator.AddCommand<BarCommand>("bar")
                    .WithDescription("Run the bar command");
    }
}