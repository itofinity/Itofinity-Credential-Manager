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

        public Credentials(Spi.Input.Options options) : this(options.ValueOrDefault("user"), options.ValueOrDefault("protocol"), options.ValueOrDefault("host"), options.ValueOrDefault("path"), options.ValueOrDefault("password"))
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
            var buffer = new System.Text.StringBuilder();
            buffer.Append(GetResponseForProperty(nameof(User).ToLower(), User));
            buffer.Append(GetResponseForProperty(nameof(Protocol).ToLower(), Protocol));
            buffer.Append(GetResponseForProperty(nameof(Host).ToLower(), Host));
            buffer.Append(GetResponseForProperty(nameof(Path).ToLower(), Path));
            buffer.Append(GetResponseForProperty("password", Secret));
            return buffer.ToString();
        }

        private string GetResponseForProperty(string propertyName, string propertyValue)
        {
            if (!string.IsNullOrWhiteSpace(propertyValue))
            {
                return $"{propertyName}={propertyValue}{Environment.NewLine}";
            }

            return null;
        }
        
    }
}
