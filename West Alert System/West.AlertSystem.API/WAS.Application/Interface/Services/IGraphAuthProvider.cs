using Microsoft.Graph.Auth;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface IGraphAuthProvider
    {
        ClientCredentialProvider GetGraphAuthProvider();
        Task<string> GetGraphAccessTokenAsync();
    }
}
