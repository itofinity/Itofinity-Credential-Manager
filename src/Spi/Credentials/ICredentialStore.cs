using System.Threading.Tasks;

namespace Spi.Credentials
{
    public interface ICredentialStore
    {
         Task<bool> Delete(ICredentialKey key);
         Task<bool> Delete(ICredentials credentials);
         Task<ICredentials> Read(ICredentialKey key);
         Task<bool> Write(ICredentials credentials);
    }
}