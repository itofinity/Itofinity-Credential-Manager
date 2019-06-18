using System;
using Spi.Credentials;
using Spi.Cli;

namespace Security.Common
{
    public class Credentials : ICredentials, IResponse
    {
        public Credentials()
        {

        }
        
        public Credentials(string user, string protocol, string host, string path, string secret)
        {
            User = user;
            Protocol = protocol;
            Host = host;
            Path = path;
            Secret = secret;
        }

        public string User { get; }
        public string Protocol { get; }
        public string Host { get; }
        public string Path { get; }
        public string Secret { get; }

        public string GetResponse()
        {
            // TODO maybe this should not be OS specific newline?
            return $"user={User}{Environment.NewLine}protocol={Protocol}{Environment.NewLine}host={Host}{Environment.NewLine}path={Path}{Environment.NewLine}secret={Secret}";
        }
    }
}
