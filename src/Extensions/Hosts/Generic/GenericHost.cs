using System;
using System.Threading.Tasks;
using Spi.Hosts;
using Spi.Credentials;
using Security.Common;
using Avalonia;
using Host.Generic.Views;
using Host.Generic.ViewModels;

namespace Host.Generic
{
    public class GenericHost : Spi.Gui.Hosts.IHost
    {
        /// <inheritdoc/>
        public async Task<int> CanHandle(string url)
        {
            // can handle anything but should be the last choice
            return await Task.FromResult(1);

        }

        /// <inheritdoc/>
        public ICredentials PromptGui(Application app)
        {
            return PromptGui(app, new Credentials());
        }

        private Avalonia.Controls.Window _window;
        /// <inheritdoc/>
        public ICredentials PromptGui(Application app, ICredentials knownCredentials)
        {
            var data = new MainWindowViewModel();
            data.CredentialsCollected += CredentialsCollected;
            _window = new MainWindow
            {
                DataContext = data,
            };

            app.Run(_window);

            if(!data.Success)
            {
                return knownCredentials;
            }
            
            // do nothing rely on Git to prompt
            return new Credentials(data.Username, knownCredentials.Protocol, knownCredentials.Host, knownCredentials.Path, data.Secret);
        }

        private void CredentialsCollected(object sender, EventArgs e)
        {
            _window.Close();
        }

        /// <inheritdoc/>
        public async Task<ICredentials> PromptCli()
        {
            return await PromptCli(new Credentials());
        }

        /// <inheritdoc/>
        public async Task<ICredentials> PromptCli(ICredentials knownCredentials)
        {
            // do nothing rely on Git to prompt
            return await Task.FromResult(new Credentials());
        }

        public async Task<ICredentials> DoNothing(ICredentials knownCredentials)
        {
            Console.WriteLine("Password Masking Console Application");
            Console.WriteLine("------------------------------------");
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            string password = "";
            Console.Write("Enter password: ");
            ConsoleKeyInfo keyInfo;

            do
            {
                keyInfo = Console.ReadKey(true);
                // Skip if Backspace or Enter is Pressed
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        // Remove last charcter if Backspace is Pressed
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Getting Password Once Enter is Pressed
            while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            Console.WriteLine("---------------------------");
            Console.WriteLine("Welcome " + username + ",");
            Console.WriteLine("Your Password is : " + password);

            return await Task.FromResult(new Security.Common.Credentials(username, knownCredentials.Protocol, knownCredentials.Host, knownCredentials.Path,password));
        }
    }
}
