using McMaster.NETCore.Plugins;

using Spectre.Console.Cli;
using Spectre.IO;

using SpectreConsoleCliBase;

using System;
using System.Collections.Generic;
using System.Linq;

using Environment = Spectre.IO.Environment;

namespace SpectreConsoleCliPlugins;

public static class ConfiguratorExtensions
{
    public static void ConfigurePlugins(this IConfigurator config)
    {
        IFileSystem fileSystem = new FileSystem();
        Environment environment = new();
        IGlobber globber = new Globber(fileSystem, environment);
        List<FilePath> files = [];
        List<PluginLoader> loaders = [];
        DirectoryPath baseDir = new(AppContext.BaseDirectory);
        List<DirectoryPath> pluginDirs =
        [
            baseDir.Combine(@"..\..\..\..\BarPlugin\bin\Debug\net8.0\").Collapse(),
            baseDir.Combine(@"..\..\..\..\FooPlugin\bin\Debug\net8.0\").Collapse()
        ];

        foreach (DirectoryPath directoryPath in pluginDirs)
        {
            files.AddRange(globber.Match("*.dll", new GlobberSettings { Root = directoryPath }).OfType<FilePath>());
        }

        foreach (FilePath file in files.DistinctBy(x => x.GetFilename()))
        {
            PluginLoader loader = PluginLoader.CreateFromAssemblyFile(file.FullPath, sharedTypes: [typeof(ICommandPlugin)]);

            loaders.Add(loader);
        }

        List<ICommandPlugin> plugins = [];

        foreach (PluginLoader loader in loaders)
        {
            foreach (Type pluginType in loader
                         .LoadDefaultAssembly()
                         .GetTypes()
                         .Where(t => typeof(ICommandPlugin).IsAssignableFrom(t) && !t.IsAbstract))
            {
                // This assumes the implementation of IPlugin has a parameterless constructor
                if (Activator.CreateInstance(pluginType) is ICommandPlugin plugin)
                {
                    plugins.Add(plugin);
                }
            }
        }

        foreach (ICommandPlugin plugin in plugins)
        {
            plugin.Configure(config);
        }
    }
}
