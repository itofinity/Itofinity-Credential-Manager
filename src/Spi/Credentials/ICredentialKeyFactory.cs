using System.Threading.Tasks;
using System.Collections.Generic;

namespace Spi.Credentials
{
    public interface ICredentialKeyFactory
    {
         Task<ICredentialKey> GenerateKey(ICredentials credentials);
         Task<ICredentialKey> GenerateKey(Dictionary<string, List<string>> options);
    }
}