using Itofinity.Refit.Cli.Utils.Commands;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Itofinity.Refit.Cli.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Itofinity.Refit.Cli.Utils.Options;
using System.Threading.Tasks;
using Spi.Credentials;
using Manager.Model;
using Security.Common;
using Common.Gcmw;
using Spi.Hosts;

namespace Manager.Commands
{
    [Export(typeof(ICommandDefinition))]
    public class Erase : AbstractCommandDefinition
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<Erase>();

        public override string Name { get; } = nameof(Erase);

        public string Description => @"Remove a matching credential, if any, from the helper’s storage.
        see https://mirrors.edge.kernel.org/pub/software/scm/git/docs/technical/api-credentials.html";

        private IEnumerable<IOptionDefinition> _localOptions = new List<IOptionDefinition>() {
            new Manager.Options.Command.Host(),
            new Manager.Options.Command.User(),
            new Manager.Options.Command.Protocol() };

        #region api
        private ICredentialStore _credentialStore = new Common.FileSystem.FileSystemCredentialStore(new Common.Gcmw.GcmwCredentialKeyFactory());
        private ICredentialKeyFactory _credentialKeyFactory = new GcmwCredentialKeyFactory();
        private IEnumerable<IHost> _hosts = new List<IHost>() { new Host.Generic.GenericHost() };
        #endregion

        public override Action<CommandLineApplication> GetConfiguration(CommandLineApplication app)
        {
            return (command) =>
            {
                command.Description = Description;

                SetHelpOption(command);

                SetGlobalOptions(command);

                SetLocalOptions(command, _localOptions);

                command.OnExecute(() =>
                {
                    Logger.LogDebug($"Running {Name}");

                    return RunRequest(command,
                        app,
                        () =>
                        {
                            ConfigureLogging(command);

                            var saved = Run(GetRuntimeOptions(command, _localOptions));

                            return saved.ToString();
                        }
                        );
                });
            };
        }

        public override string Run(Spi.Input.Options options)
        {
            var credentials = new Security.Common.Credentials(options);

            Logger.LogDebug($"Attempting to delete {credentials}");

            var result = Task.Run(() => _credentialStore.Delete(credentials)).Result;
            if (result)
            {
                return "true";
            }

            return "false";
        }
    }
}