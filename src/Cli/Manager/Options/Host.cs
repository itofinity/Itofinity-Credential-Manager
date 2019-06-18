using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;

namespace Manager.Options.Command
{
    public class Host : AbstractOptionDefinition
    {
        public static string CanonicalName = nameof(Host).ToLower();

        public override string Name => nameof(Host);
        
        public override string Template => $"--{nameof(Host).ToLower()}";

        public override string Description => $"The remote {nameof(Host)}";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}