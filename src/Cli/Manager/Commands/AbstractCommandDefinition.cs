using Itofinity.Refit.Cli.Utils;
using Itofinity.Refit.Cli.Utils.Commands;
using Itofinity.Refit.Cli.Utils.Options;
using Itofinity.Refit.Cli.Utils.Options.Global;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Composition;
using System.Linq;

namespace Manager.Commands
{
    public abstract class AbstractCommandDefinition : ICommandDefinition
    {
//        [Import]
//        public IClientFactory<ApiClient> ClientFactory { get; private set; }

        public static string _namespaceRoot = typeof(AbstractCommandDefinition).Namespace;
        public abstract string Name { get; }

  //      public IOptionDefinition ApiTokenOptionDefinition => new Token();

  //      public CommandOption ApiTokenOption { get; private set; }

        protected void SetHelpOption(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
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
            ConfigureLogging(logLocationOption, logVerbosityOption);

            try
            {
                var results = request.Invoke();

                if (results is IList)
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
                }

                Console.WriteLine();
                Console.WriteLine($"{command.Name} has finished.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        private static void ConfigureLogging(CommandOption logLocationOption, CommandOption logVerbosityOption)
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

        private static void OutputResults(bool porcelain, bool raw, IList results)
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
        }

        public abstract Action<CommandLineApplication> GetConfiguration(CommandLineApplication app);

        protected static string GetCommandName(Type type)
        {
            var context = type.Namespace.Replace(_namespaceRoot + ".", string.Empty).Replace(".", "-").ToLower();
            var command = type.Name.ToLower();
            return $"{context}-{command}";// type.AssemblyQualifiedName.Replace("Itofinity.Appveyor.Cli.Commands.", string.Empty).Replace(".", "-").ToLower();
        }
    }
}