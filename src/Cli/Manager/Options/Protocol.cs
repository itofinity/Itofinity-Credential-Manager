using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Options.Command
{
    public class Protocol : AbstractOptionDefinition
    {
        public override string Name => nameof(Protocol);

        public override string Template => $"--{nameof(Protocol).ToLower()}";

        public override string Description => $"The remote {nameof(Protocol)}";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}