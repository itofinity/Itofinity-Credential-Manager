using Itofinity.Refit.Cli.Utils.Options;
using Microsoft.Extensions.CommandLineUtils;
using System.Collections.Generic;

namespace Manager.Options.Command
{
    public class ModalPrompt : AbstractOptionDefinition
    {
        public static string CanonicalName = nameof(ModalPrompt).ToLower();

        public override string Name => nameof(ModalPrompt);

        public static IEnumerable<string> CanonicalNameAliases = new List<string>() {
            nameof(ModalPrompt),
            "GCM_MODAL_PROMPT",
            "ICM_MODAL_PROMPT"
        };

        public override string Template => $"--{nameof(ModalPrompt).ToLower()}";

        public override string Description => @"Forces authentication to use a modal dialog instead of asking for credentials at the command prompt.

Supports `true` or `false`. Defaults to `true`.";

        public override CommandOptionType OptionType => CommandOptionType.SingleValue;
    }
}