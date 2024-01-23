using Spectre.Console.Cli;

namespace SpectreConsoleCliBase;

public interface ICommandPlugin
{
    void Configure(IConfigurator configurator);
}