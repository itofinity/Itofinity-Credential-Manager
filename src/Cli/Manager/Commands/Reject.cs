
using Itofinity.Refit.Cli.Utils.Commands;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Composition;
using System.Linq;

namespace Manager.Commands
{
    [Export(typeof(ICommandDefinition))]
    public class Reject : Delete
    {
        public override string Name { get; } = nameof(Reject);

        public string Description => $"This is the description for {Name}.";
    }
}