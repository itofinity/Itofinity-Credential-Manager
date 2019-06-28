using Avalonia;
using System.Threading.Tasks;
using Spi.Credentials;

namespace Spi.Gui.Hosts
{
    public interface IHost : Spi.Hosts.IHost
    {
        /// <summary>
        /// Prompt the user for new credentials, when we know nothing
        /// </summary>
        /// <returns></returns>
        ICredentials PromptGui(Application app);

        /// <summary>
        /// Prompt the user for new credentials, when we know something
        /// </summary>
        /// <param name="knownCredentials"></param>
        /// <returns></returns>
        ICredentials PromptGui(Application app, ICredentials knownCredentials);
    }
}