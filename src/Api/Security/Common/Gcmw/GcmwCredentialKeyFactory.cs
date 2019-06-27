using System;
using Spi.Credentials;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Common.Gcmw
{
    public class GcmwCredentialKeyFactory : ICredentialKeyFactory
    {
        private const string _prefix = "icm";
        public Task<ICredentialKey> GenerateKey(ICredentials credentials)
        {
            var uri = new UriBuilder() {
                UserName = credentials.User,
                Host = credentials.Host,
                Scheme = credentials.Protocol,
                Path = credentials.Path
            };

            return Task.FromResult(new GcmwCredentialKey(uri.Uri) as ICredentialKey);
        }

        public Task<ICredentialKey> GenerateKey(Spi.Input.Options options)
        {
            // TODO magic boo
            var uri = new UriBuilder() {
                UserName = options.ValueOrDefault("user"),
                Host = options.ValueOrDefault("host"),
                Scheme = options.ValueOrDefault("protocol"),
                Path = options.ValueOrDefault("path")
            };

            return Task.FromResult(new GcmwCredentialKey(uri.Uri) as ICredentialKey);
        }
    }
}