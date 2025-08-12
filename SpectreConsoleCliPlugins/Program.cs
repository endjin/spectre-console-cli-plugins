using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace SpectreConsoleCliPlugins;

public class Program
{
    public static Task<int> Main(string[] args)
    {
        CommandApp app = new(new TypeRegistrar());

        app.Configure(config =>
        {
            config.SetApplicationName("vellum");
            config.CaseSensitivity(CaseSensitivity.None);
            config.ValidateExamples();
            config.PropagateExceptions();
            config.ConfigurePlugins();
        });

        return app.RunAsync(args);
    }
}