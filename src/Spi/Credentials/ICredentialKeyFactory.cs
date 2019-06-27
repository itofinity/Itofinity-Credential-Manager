using System.Threading.Tasks;
using System.Collections.Generic;

namespace Spi.Credentials
{
    public interface ICredentialKeyFactory
    {
         Task<ICredentialKey> GenerateKey(ICredentials credentials);
         Task<ICredentialKey> GenerateKey(Spi.Input.Options options);
    }
}