using System;
using Spi.Credentials;
using Spi.Cli;
using System.Collections;
using System.Collections.Generic;

namespace Security.Common
{
    public class Credentials : ICredentials, IResponse
    {
        public Credentials()
        {

        }

        public Credentials(Spi.Input.Options options) : this(options.ValueOrDefault("user"), options.ValueOrDefault("protocol"), options.ValueOrDefault("host"), options.ValueOrDefault("path"), null)
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

// TODO make these immutable? requires use of JsonPoroperty and direct dependnecy on json.net ...
        public string User { get; set; }

        public string Protocol { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string Secret { get; set; }

        public string GetResponse()
        {
            // TODO maybe this should not be OS specific newline?
            return $"user={User}{Environment.NewLine}protocol={Protocol}{Environment.NewLine}host={Host}{Environment.NewLine}path={Path}{Environment.NewLine}secret={Secret}";
        }

        
    }
}
