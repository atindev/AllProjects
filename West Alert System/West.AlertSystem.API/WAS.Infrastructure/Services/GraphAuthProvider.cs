using Microsoft.Extensions.Options;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;
using System.Threading.Tasks;

namespace WAS.Infrastructure.Services
{
    public class GraphAuthProvider : IGraphAuthProvider
    {
        private readonly IOptions<GraphSettings> _graphOptions;

        public GraphAuthProvider(IOptions<GraphSettings> options)
        {
            _graphOptions = options;

        }
        public ClientCredentialProvider GetGraphAuthProvider()
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(_graphOptions.Value.ClientId)
                .WithTenantId(_graphOptions.Value.TenantId)
                .WithClientSecret(_graphOptions.Value.ClientSecret)
                .Build();
            ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);
            return authenticationProvider;
        }

        public async Task<string> GetGraphAccessTokenAsync()
        {
            var clientCredentials = new ClientCredential(_graphOptions.Value.ClientId, _graphOptions.Value.ClientSecret);

            var authContext = new AuthenticationContext(_graphOptions.Value.TokenAuthority);

            var authResult = await authContext.AcquireTokenAsync(_graphOptions.Value.GraphResourceURL, clientCredentials);

            return authResult.AccessToken;
        }
    }
}
