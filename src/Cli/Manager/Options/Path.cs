using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Options.Command
{
    public class Path : AbstractOptionDefinition
    {
        public override string Name => nameof(Path);

        public override string Template => $"--{nameof(Path).ToLower()}";

        public override string Description => $"The relative {nameof(Path)}";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}