
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

        public string Description => $"This is the description for {Name}.";

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
                var commandOptions = new List<IOptionDefinition>() {new Manager.Options.Command.Host(), new Manager.Options.Command.User(), new Manager.Options.Command.Protocol()};
                commandOptions.ForEach(co => SetOption(command, co));
//                var hostOption = SetOption(command, new Host());
  //              var userOption = SetOption(command, new User());
    //            var protocolOption = SetOption(command, new Protocol());
                

                command.OnExecute(() =>
                {
                    return RunRequest(command,
                        app,
                        () =>
                        {
                            ConfigureLogging(command.Options.First(o => o.ShortName.Equals("l")), command.Options.First(o => o.ShortName.Equals("v")));
                            /* if(command.Options.Any(o => o.HasValue()))
                            {
                                return $"you called {Name} with {hostOption.Value()}/{userOption.Value()}/{protocolOption.Value()}";
                            }

                            Console.WriteLine("enter some stuff");*/




                            var options = new Dictionary<string, List<string>>();
                            // get env vars
                            // - get all found
                            GetEnvironmentVariableOptions().ToList().ForEach(x => options[x.Key] = x.Value);
                            // get command line args
                            // - get all found + overwrite/override env var
                            var dave = command.Options.Distinct().ToDictionary(o => o.LongName, o => o.Values).ToList();
                            dave.ForEach(x => options[x.Key] = x.Value);
                            // do we have all we need?
                            if(!options.Any(op => 
                                op.Value.Any() && commandOptions.Any(co => co.Name.Equals(op.Key, StringComparison.InvariantCultureIgnoreCase))))
                            {
                                // - if not 
                                //  - get interactive values
                                var operationArguments = new Manager.Model.OperationArguments();

                                // Parse the operations arguments from stdin (this is how git sends commands)
                                // see: https://www.kernel.org/pub/software/scm/git/docs/technical/api-credentials.html
                                // see: https://www.kernel.org/pub/software/scm/git/docs/git-credential.html
                                using (var stdin = Console.OpenStandardInput())
                                {
                                    Task.Run(async () => await operationArguments.ReadInput(stdin)).Wait();
                                }

                                operationArguments.Options.ToList().ForEach(x => options[x.Key] = x.Value);
                            }

                            foreach(var option in options) 
                            {
                                Logger.LogDebug($"{option.Key}=[");
                                foreach(var value in option.Value) 
                                {
                                    Logger.LogDebug($"{value},");
                                }
                                Logger.LogDebug($"]");
                            }


                            var credentials = Task.Run(async () => await Run(options)).Result;

                            return credentials.GetResponse();
                        }
                        );
                });
            };
        }

        public async Task<ICredentials> Run(Dictionary<string, List<string>> options)
        {
            // get host
            var host = _hosts
                .Select(h => new { 
                    Handler = h, 
                    Weight = h.CanHandle(
                        GetOptionValue(options, Manager.Options.Command.Host.CanonicalName)
                        ).Result
                    }
                )
                .OrderBy(a => a.Weight).Where(a => a.Weight >= 1)
                .FirstOrDefault().Handler;
            //read from credential store
            var foundCredentials = await _credentialStore.Read(await _credentialKeyFactory.GenerateKey(options));
            if(foundCredentials != null)
            {
                return foundCredentials;
            }

            // TODO decide on GUI or not GUI
            foundCredentials = await host.PromptCli();
            return foundCredentials;
        }

        private string GetOptionValue(Dictionary<string, List<string>> options, string optionName)
        {
            var values = options[optionName];
            return values.FirstOrDefault();
        }
    }
}