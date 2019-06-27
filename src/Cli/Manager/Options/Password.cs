using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Options.Command
{
    public class Password : AbstractOptionDefinition
    {
        public override string Name => nameof(Password);

        public override string Template => $"--{nameof(Password).ToLower()}";

        public override string Description => $"The relative {nameof(Password)}";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}