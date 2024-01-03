using McMaster.NETCore.Plugins;

using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

using Spectre.Console.Cli;
using Spectre.Console.Cli.Unsafe;
using Spectre.IO;

using SpectreConsoleCliBase;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SpectreConsoleCliPlugins;

public class Program
{
    public static Task<int> Main(string[] args)
    {
        List<FileInfo> files = [];
        List<PluginLoader> loaders = [];
        List<DirectoryPath> pluginDirs =
        [
            @".\FooPlugin\bin\Debug\net8.0\",
            @".\BarPlugin\bin\Debug\net8.0\"
        ];

        Matcher matcher = new();
        matcher.AddInclude("*.dll");

        DirectoryInfo commonDir = new(AppContext.BaseDirectory);

        foreach (DirectoryPath directoryPath in pluginDirs)
        {
            string result = System.IO.Path.GetRelativePath(directoryPath.FullPath, commonDir.FullName);
            string relPath = System.IO.Path.Combine(commonDir.FullName, result, directoryPath.FullPath);
            string fullPath = System.IO.Path.GetFullPath(relPath);
            
            PatternMatchingResult matches = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(fullPath)));

            foreach (FilePatternMatch file in matches.Files)
            {
                files.Add(new FileInfo(System.IO.Path.Join(fullPath, file.Path)));
            }
        }

        foreach (FileInfo file in files.DistinctBy(x => x.Name))
        {
            PluginLoader loader = PluginLoader.CreateFromAssemblyFile(file.FullName, sharedTypes: [typeof(IPluginCommand)]);

            loaders.Add(loader);
        }

        List<IPluginCommand> plugins = [];

        foreach (PluginLoader loader in loaders)
        {
            foreach (Type pluginType in loader
                         .LoadDefaultAssembly()
                         .GetTypes()
                         .Where(t => typeof(IPluginCommand).IsAssignableFrom(t) && !t.IsAbstract))
            {
                // This assumes the implementation of IPlugin has a parameterless constructor
                if (Activator.CreateInstance(pluginType) is IPluginCommand plugin)
                {
                    plugins.Add(plugin);
                }
            }
        }

        CommandApp app = new();

        app.Configure(config =>
        {
            config.Settings.PropagateExceptions = false;
            config.CaseSensitivity(CaseSensitivity.None);
            config.SetApplicationName("vellum");
            config.ValidateExamples();

            foreach (IPluginCommand plugin in plugins)
            {
                config.SafetyOff().AddCommand(plugin.Name, plugin.Command.GetType()).WithDescription(plugin.Description);
            }
        });

        return app.RunAsync(args);
    }
}