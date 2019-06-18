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

        public Task<ICredentialKey> GenerateKey(Dictionary<string, List<string>> options)
        {
            // TODO magic boo
            var uri = new UriBuilder() {
                UserName = GetValue(options, "user"),
                Host = GetValue(options, "host"),
                Scheme = GetValue(options, "protocol"),
                Path = GetValue(options, "path")
            };

            return Task.FromResult(new GcmwCredentialKey(uri.Uri) as ICredentialKey);
        }

        public string GetValue(Dictionary<string, List<string>> options, string key, int index = 0)
        {
            if( options.TryGetValue(key, out List<string> values))
            {
                 if(index <= values.Count - 1)
                {
                    return values[index];
                }
            }

            return null;
        }
    }
}