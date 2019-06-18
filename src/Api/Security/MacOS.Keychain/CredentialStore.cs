using System;
using Spi.Credentials;
using System.Threading.Tasks;

namespace MacOS.Keychain
{
    public class CredentialStore : ICredentialStore
    {
        public Task<bool> Delete(ICredentialKey key)
        {
            throw new Exception("duh delete key");
        }

        public Task<bool> Delete(ICredentials credentials)
        {
            throw new Exception("duh delete credentials");
        }

        public Task<ICredentials> Read(ICredentialKey key)
        {
            throw new Exception("duh read");
        }

        public Task<bool> Write(ICredentials credentials)

        {
            throw new Exception("duh write");
        }
    }
}