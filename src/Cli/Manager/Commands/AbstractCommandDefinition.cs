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
using System.Threading.Tasks;
using System.Text;

namespace Manager.Commands
{
    public abstract class AbstractCommandDefinition : ICommandDefinition
    {
        private static ILogger Logger { get; } = ApplicationLogging.CreateLogger<AbstractCommandDefinition>();

        private static IEnumerable<IOptionDefinition> _globalOptionDefinitions = new List<IOptionDefinition>()
        {
            new Verbosity(),
            new LogFilePath(),
        };
        public static string _namespaceRoot = typeof(AbstractCommandDefinition).Namespace;
        public abstract string Name { get; }

        protected void SetHelpOption(CommandLineApplication command)
        {
            command.HelpOption("-?|-h|--help");
        }

        protected void SetGlobalOptions(CommandLineApplication command)
        {
            _globalOptionDefinitions.ToList().ForEach(o => command.Option(o.Template, o.Description, o.OptionType));
        }

        protected void SetLocalOptions(CommandLineApplication command, IEnumerable<IOptionDefinition> options)
        {
            options.ToList().ForEach(co => SetOption(command, co));
        }

        protected Spi.Input.Options GetRuntimeOptions(CommandLineApplication command, IEnumerable<IOptionDefinition> commandOptions)
        {
            var options = new Spi.Input.Options();
            // get env vars
            // - get all found
            GetEnvironmentVariableOptions().ToList().ForEach(x => options[x.Key] = x.Value);
            // get command line args
            // - get all found + overwrite/override env var
            var dave = command.Options.Distinct().ToDictionary(o => o.LongName, o => o.Values).ToList();
            dave.ForEach(x => options[x.Key] = x.Value);
            // do we have all we need?
            if (!options.Any(op =>
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

            foreach (var option in options)
            {
                StringBuilder buffer = new StringBuilder();
                buffer.Append($"{option.Key}=[");
                foreach (var value in option.Value)
                {
                    buffer.Append($"{value},");
                }
                buffer.Append($"]");
                buffer.Append(System.Environment.NewLine);

                Logger.LogDebug(buffer.ToString());
            }

            return options;
        }

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
                Console.WriteLine(ex.StackTrace);
            }

            return 0;
        }

        protected static void ConfigureLogging(CommandLineApplication command)
        {
            ConfigureLogging(command.Options.First(o => o.ShortName.Equals("l")), command.Options.First(o => o.ShortName.Equals("v")));
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
                Logger.LogError("help!");
                Console.WriteLine("help!!");
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
            var envvars = rawEnvVars.Keys.Cast<object>().ToDictionary(k => k.ToString(), v => rawEnvVars[v]);

            if (!envvars.Any(ev => ev.Key.StartsWith("icm", StringComparison.InvariantCultureIgnoreCase)))
            {
                return options;
            };

            return options;
        }

        public virtual string Run(Spi.Input.Options options)
        {
            Console.WriteLine($"Called [{Name}] with options:");

            foreach (var option in options)
            {
                Console.Write($"{option.Key}=[");
                foreach (var value in option.Value)
                {
                    Console.Write($"{value},");
                }
                Console.WriteLine($"]");
            }

            return null;
        }

        protected bool IsInteractive(Spi.Input.Options options)
        {
            var val = GetValue(new Manager.Options.Command.Interactive().Aliases.ToList(), options);
            return val == null || val.Equals("Always", StringComparison.InvariantCultureIgnoreCase) || val.Equals("Auto", StringComparison.InvariantCultureIgnoreCase);
        }
        protected bool UseModalPrompt(Spi.Input.Options options)
        {
            var val = GetValue(new Manager.Options.Command.ModalPrompt().Aliases.ToList(), options);
            return val == null || val.Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }

        protected string GetValue(List<string> aliases, Spi.Input.Options options)
        {
            return aliases
                .Select(a => options.ValueOrDefault(a))
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .FirstOrDefault();
        }
    }
}