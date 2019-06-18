using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Options.Command
{
    public class User : AbstractOptionDefinition
    {
        public override string Name => nameof(User);

        public override string Template => $"--{nameof(User).ToLower()}";

        public override string Description => $"The remote {nameof(User)}";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}