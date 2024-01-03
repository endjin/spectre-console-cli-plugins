using Spectre.Console.Cli;
using SpectreConsoleCliBase;

namespace BarPlugin;

public class BarCommandProxy : IPluginCommand
{
    public string Name => "Bar";

    public string Description => "Runs the Bar Command";

    public ICommand Command => new BarCommand();
}