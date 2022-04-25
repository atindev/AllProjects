using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WAS.Application.Interface.Services;
using WAS.Infrastructure.Settings;

namespace WAS.Infrastructure.Services
{
    /// <summary>
    /// Auth service.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IOptions<Settings.AzureAdSettings> _azureOptions;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DTDeviceConnectedPlatform.Core.Services.AuthService"/> class.
        /// </summary>
        public AuthService(IOptions<Settings.AzureAdSettings> options)
        {
            _azureOptions = options;
        }

        /// <summary>
        /// Generates the authentication token async.
        /// </summary>
        /// <returns>The authentication token async.</returns>
        public async Task<string> GenerateAuthenticationTokenAsync(string resourceUrl = "")
        {
            var clientCredentials = new ClientCredential(_azureOptions.Value.ClientId, _azureOptions.Value.ClientSecret);

            var authContext = new AuthenticationContext(_azureOptions.Value.TokenAuthority);

            var authResult = await authContext.AcquireTokenAsync(string.IsNullOrEmpty(resourceUrl) ? _azureOptions.Value.ApiResourceUrl : resourceUrl, clientCredentials);

            return authResult.AccessToken;
        }
    }
}
