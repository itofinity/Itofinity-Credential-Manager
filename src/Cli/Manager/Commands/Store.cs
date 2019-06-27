using Itofinity.Refit.Cli.Utils.Commands;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Itofinity.Refit.Cli.Utils.Options;
using Itofinity.Refit.Cli.Utils;
using System.Threading.Tasks;
using Spi.Credentials;
using Manager.Model;
using Security.Common;
using Common.Gcmw;
using Spi.Hosts;

namespace Manager.Commands
{
    [Export(typeof(ICommandDefinition))]
    public class Store : AbstractCommandDefinition
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<Store>();

        public override string Name { get; } = nameof(Store);

        public string Description => @"Store the credential, if applicable to the helper.
        see https://mirrors.edge.kernel.org/pub/software/scm/git/docs/technical/api-credentials.html";

        private IEnumerable<IOptionDefinition> _localOptions = new List<IOptionDefinition>() { 
            new Manager.Options.Command.Host(), 
            new Manager.Options.Command.User(), 
            new Manager.Options.Command.Protocol(),
            new Manager.Options.Command.Password()  };

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
                    return RunRequest(command,
                        app,
                        () =>
                        {
                            ConfigureLogging(command);

                            var saved = Task.Run(async () => await Run(GetRuntimeOptions(command, _localOptions))).Result;

                            return saved.ToString();
                        }
                        );
                });
            };
        }

        public override async Task<string> Run(Spi.Input.Options options)
        {
            var credentials = new Security.Common.Credentials(options);
            var result = await _credentialStore.Write(credentials);
            if (result)
            {
                Logger.LogError($"Failed to save {credentials}");
            }

            return string.Empty;
        }
    }
}