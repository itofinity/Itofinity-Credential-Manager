using Itofinity.Refit.Cli.Utils;
using Itofinity.Refit.Cli.Utils.Commands;
using Itofinity.Refit.Cli.Utils.Options;
using Itofinity.Refit.Cli.Utils.Options.Global;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using Itofinity.Refit.Cli.Utils.Options.Global;

namespace Manager.Commands
{
    public abstract class AbstractCommandDefinition : ICommandDefinition
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<AbstractCommandDefinition>();
//        [Import]
//        public IClientFactory<ApiClient> ClientFactory { get; private set; }

        private static IEnumerable<IOptionDefinition> _globalOptionDefinitions = new List<IOptionDefinition>()
        {
            new Verbosity(),
            new LogFilePath(),
        };
        public static string _namespaceRoot = typeof(AbstractCommandDefinition).Namespace;
        public abstract string Name { get; }

  //      public IOptionDefinition ApiTokenOptionDefinition => new Token();

  //      public CommandOption ApiTokenOption { get; private set; }

        protected void SetHelpOption(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
        }

        protected void SetGlobalOptions(CommandLineApplication command)
        {
            _globalOptionDefinitions.ToList().ForEach(o => command.Option(o.Template, o.Description, o.OptionType));
        }

//        protected void SetApiTokenOption(CommandLineApplication command)
//        {
//            ApiTokenOption = command.Option(ApiTokenOptionDefinition.Template,
//                ApiTokenOptionDefinition.Description,
//                ApiTokenOptionDefinition.OptionType);
//        }

        protected CommandOption SetOption(CommandLineApplication command, IOptionDefinition optionDefinition)
        {
            return command.Option(optionDefinition.Template,
                optionDefinition.Description,
                optionDefinition.OptionType);
        }

        public static int RunRequest(CommandLineApplication command, CommandLineApplication app, Func<object> request)
        {
            return RunRequest(command,
                        app.Options.FirstOrDefault(o => o.ShortName.Equals("l")),
                        app.Options.FirstOrDefault(o => o.ShortName.Equals("v")),
                        app.Options.FirstOrDefault(o => o.ShortName.Equals("p")),
                        app.Options.FirstOrDefault(o => o.ShortName.Equals("r")),
                        request
                        );
        }

        public static int RunRequest(CommandLineApplication command, CommandOption logLocationOption, CommandOption logVerbosityOption, CommandOption porcelainOption, CommandOption rawOption,
            Func<object> request)
        {

            try
            {
                var results = request.Invoke();

                /* if (results is IList)
                {
                    OutputResults(porcelainOption.HasValue(), rawOption.HasValue(), results as IList);
                }
                else if (porcelainOption != null && rawOption != null )
                {
                    OutputResult(porcelainOption.HasValue(), rawOption.HasValue(), results);
                }
                else
                {
                    OutputResult(results);
                }*/
                OutputResult(results as string);

                Logger.LogDebug($"{command.Name} has finished.");
            }
            catch (Exception ex)
            {
                Logger.LogError("oops!", ex);
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        protected static void ConfigureLogging(CommandOption logLocationOption, CommandOption logVerbosityOption)
        {
            var logLevel = NLog.LogLevel.Off;
            if (logVerbosityOption.HasValue())
            {
                var requestedLevel = logVerbosityOption.Value();
                logLevel = NLogFactory.GetLogLevel(requestedLevel);
            }

            if (logLocationOption.HasValue())
            {
                NLogFactory.Configure(logLocationOption.Value(), logLevel);
            }
            else if (logLevel != NLog.LogLevel.Off)
            {
                // no file specified by a verbosity level so log to console
                ApplicationLogging.LoggerFactory.AddConsole(true);
            }
        }

        /* private static void OutputResults(bool porcelain, bool raw, IList results)
        {
            if (!porcelain)
                Console.WriteLine();
            if (raw)
            {
                OutputResult(results);
            }
            else
            {
                foreach (var result in results)
                {
                    OutputResult(result);
                }
            }
        }

        private static void OutputResult(bool porcelain, bool raw, object result)
        {
            if (!porcelain)
                Console.WriteLine();

            OutputResult(result);
        }

        private static void OutputResult(object result)
        {
            var line = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.None);
            Console.WriteLine($"{line}");
        }*/

        private static void OutputResult(string result)
        {
            Console.WriteLine(result);
        }

        public abstract Action<CommandLineApplication> GetConfiguration(CommandLineApplication app);

        protected static string GetCommandName(Type type)
        {
            var context = type.Namespace.Replace(_namespaceRoot + ".", string.Empty).Replace(".", "-").ToLower();
            var command = type.Name.ToLower();
            return $"{context}-{command}";// type.AssemblyQualifiedName.Replace("Itofinity.Appveyor.Cli.Commands.", string.Empty).Replace(".", "-").ToLower();
        }

        protected static Dictionary<string, List<string>> GetEnvironmentVariableOptions()
        {
            var options = new Dictionary<string, List<string>>();

            var rawEnvVars = System.Environment.GetEnvironmentVariables();
            var envvars = rawEnvVars.Keys.Cast<object>().ToDictionary(k=> k.ToString(), v=> rawEnvVars[v]);

            if(!envvars.Any(ev => ev.Key.StartsWith("icm", StringComparison.InvariantCultureIgnoreCase)))
            {
                return options;
            };

            return options;
        }
    }
}