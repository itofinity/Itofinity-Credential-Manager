using System.Threading.Tasks;
using System;
using Spi.Credentials;

namespace Spi.Hosts
{
    /// <summary>
    /// Definition of a Host type.
    /// Assumes the controlling program will make decisions e.g. to GUI or not to GUI
    /// </summary>
    public interface IHost
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>a 'weight'.async > 1 implies this can handle the host, <=0 implies if can't.!-- The higher the 'weight' the better the match</returns>
         Task<int> CanHandle(string url);

        /// <summary>
        /// Prompt the user for new credentials, when we know nothing
        /// </summary>
        /// <returns></returns>
        Task<ICredentials> PromptCli();

        /// <summary>
        /// Prompt the user for new credentials, when we know something
        /// </summary>
        /// <param name="knownCredentials"></param>
        /// <returns></returns>
        Task<ICredentials> PromptCli(ICredentials knownCredentials);

        /// <summary>
        /// Prompt the user for new credentials, when we know nothing
        /// </summary>
        /// <returns></returns>
        Task<ICredentials> PromptGui();

        /// <summary>
        /// Prompt the user for new credentials, when we know something
        /// </summary>
        /// <param name="knownCredentials"></param>
        /// <returns></returns>
        Task<ICredentials> PromptGui(ICredentials knownCredentials);
    }
}