using System.Threading.Tasks;

namespace Spi.Credentials
{
    public interface ICredentialKey
    {
        Task<string> Key{ get; }
    }
}