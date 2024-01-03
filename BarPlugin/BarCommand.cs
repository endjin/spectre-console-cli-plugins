using Spectre.Console.Cli;

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BarPlugin;

public class BarCommand : AsyncCommand<BarCommand.Settings>
{
    /// <inheritdoc/>
    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        Console.WriteLine($"BarCommand says: {settings.Message}");

        return Task.FromResult(0);
    }

    /// <summary>
    /// The settings for the command.
    /// </summary>
    public class Settings : CommandSettings
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [CommandOption("-m|--message")]
        [Description("Message to Display")]
        public string  Message { get; init; }
    }
}