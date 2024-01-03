using Spectre.Console.Cli;

namespace SpectreConsoleCliBase;

public interface IPluginCommand
{
    string Name { get; }

    string Description { get; }

    ICommand Command  { get; }
}