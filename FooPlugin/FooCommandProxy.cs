using Spectre.Console.Cli;

using SpectreConsoleCliBase;

namespace FooPlugin;

public class FooCommandProxy : IPluginCommand
{
    public string Name => "Foo";

    public string Description => "Runs the Foo Command";

    public ICommand Command => new FooCommand();
}