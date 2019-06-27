using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace Manager.Options.Command
{
    public class Interactive : AbstractOptionDefinition
    {
        public static string CanonicalName = nameof(Interactive).ToLower();

        public override string Name => nameof(Interactive);

        public IEnumerable<string> Aliases = new List<string>() {
            nameof(Interactive),
            "GCM_INTERACTIVE",
            "ICM_INTERACTIVE"
        };

        public override string Template => $"--{nameof(Interactive).ToLower()}";

        public override string Description => @"Specifies if user can be prompted for credentials or not.

Supports `Auto`, `Always`, or `Never`. Defaults to `Auto`.";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}