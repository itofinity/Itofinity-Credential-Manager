using Spi.Credentials;
using System.Threading.Tasks;
using System;

namespace Common.Gcmw
{
    public class GcmwCredentialKey : ICredentialKey
    {
        private const string _prefix = "git";
        private Uri _uri;

        public GcmwCredentialKey(Uri uri)
        {
            _uri = uri;
        }

        public Task<string> Key{ get {
            return Task.FromResult($"{_prefix}:{_uri.ToString()}");
        } }
    }
}