
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
    public class Store : AbstractCommandDefinition
    {
        public override string Name { get; } = nameof(Store);

        public string Description => $"This is the description for {Name}.";

        public override Action<CommandLineApplication> GetConfiguration(CommandLineApplication app)
        {
            return (command) =>
            {
                command.Description = Description;

                SetHelpOption(command);
                //SetApiTokenOption(command);

                command.OnExecute(() =>
                {
                    return RunRequest(command,
                        app,
                        () =>
                        {
                            return $"you called {Name}";
                        }
                        );
                });
            };
        }
    }
}