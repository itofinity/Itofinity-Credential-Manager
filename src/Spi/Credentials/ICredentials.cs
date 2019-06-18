using Spi.Cli;

namespace Spi.Credentials
{
    public interface ICredentials : IResponse
    { 
        string User { get; }
        string Protocol { get; }
        string Host { get; }
        string Path { get; }
    }
}