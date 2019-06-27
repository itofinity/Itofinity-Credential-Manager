
using Itofinity.Refit.Cli.Utils.Commands;
using Itofinity.Refit.Cli.Utils;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Manager.Options.Command;
using System.Collections.Generic;
using Itofinity.Refit.Cli.Utils.Options;
using Spi.Credentials;
using Manager.Model;
using Security.Common;
using Common.Gcmw;
using Spi.Hosts;

namespace Manager.Commands
{
    [Export(typeof(ICommandDefinition))]
    public class Get : AbstractCommandDefinition
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<Get>();

        public override string Name { get; } = nameof(Get);

        public string Description => @"Return a matching credential, if any exists.
        see https://mirrors.edge.kernel.org/pub/software/scm/git/docs/technical/api-credentials.html";

        #region api
        private ICredentialStore _credentialStore = new Common.FileSystem.FileSystemCredentialStore(new Common.Gcmw.GcmwCredentialKeyFactory());
        private ICredentialKeyFactory _credentialKeyFactory = new GcmwCredentialKeyFactory();
        private IEnumerable<IHost> _hosts = new List<IHost>() { new Host.Generic.GenericHost() };
        #endregion

        private IEnumerable<IOptionDefinition> _localOptions = new List<IOptionDefinition>() {
            new Manager.Options.Command.Host(),
            new Manager.Options.Command.User(),
            new Manager.Options.Command.Protocol(),
            new Manager.Options.Command.Path() };

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
                            ConfigureLogging(command.Options.First(o => o.ShortName.Equals("l")), command.Options.First(o => o.ShortName.Equals("v")));

                            return Task.Run(async () => await Run(GetRuntimeOptions(command, _localOptions))).Result;
                        }
                        );
                });
            };
        }

        public override async Task<string> Run(Spi.Input.Options options)
        {
            // get host
            var host = _hosts
                .Select(h => new
                {
                    Handler = h,
                    Weight = h.CanHandle(
                        options.ValueOrDefault(Manager.Options.Command.Host.CanonicalName)
                        ).Result
                }
                )
                .OrderBy(a => a.Weight).Where(a => a.Weight >= 1)
                .FirstOrDefault().Handler;
            //read from credential store
            var foundCredentials = await _credentialStore.Read(await _credentialKeyFactory.GenerateKey(options));
            if (foundCredentials != null)
            {
                return foundCredentials.GetResponse();
            }

            if(!IsInteractive(options))
            {
                return null;
            }
            
            if(UseModalPrompt(options))
            {
                foundCredentials = await host.PromptGui();
                return foundCredentials.GetResponse();
            }
            else
            {
                foundCredentials = await host.PromptCli();
                return foundCredentials.GetResponse();
            }
        }
    }
}