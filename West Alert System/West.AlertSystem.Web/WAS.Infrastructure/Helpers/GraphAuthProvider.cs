﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using Microsoft.AspNetCore.Authentication;

namespace WAS.Infrastructure.Helpers
{
    public class GraphAuthProvider : IGraphAuthProvider
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string[] _scopes;

        public GraphAuthProvider(IConfiguration configuration)
        {
            var azureOptions = new AzureAdOptions();
            configuration.Bind("AzureAd", azureOptions);

            // More info about MSAL Client Applications: https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/Client-Applications
            _app = ConfidentialClientApplicationBuilder.Create(azureOptions.ClientId)
                    .WithClientSecret(azureOptions.ClientSecret)
                    .WithAuthority(AzureCloudInstance.AzurePublic, AadAuthorityAudience.AzureAdMyOrg)
                    .WithTenantId(azureOptions.TenantId)
                    .WithRedirectUri(azureOptions.BaseUrl + azureOptions.CallbackPath)
                    .Build();

            Authority = _app.Authority;

            _scopes = azureOptions.GraphScopes.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string Authority { get; }

        // Gets an access token. First tries to get the access token from the token cache.
        // Using password (secret) to authenticate. Production apps should use a certificate.
        public async Task<string> GetUserAccessTokenAsync(string userId)
        {
            var account = await _app.GetAccountAsync(userId);
            try
            {
                if (account == null)
                {
                    var result1 = await _app.AcquireTokenForClient(_scopes).ExecuteAsync();
                    return result1.AccessToken;
                }
            
                var result = await _app.AcquireTokenSilent(_scopes, account).ExecuteAsync();
                return result.AccessToken;
            }

            // Unable to retrieve the access token silently.
            catch
            {
                throw new ServiceException(new Error
                {
                    Code = GraphErrorCode.AuthenticationFailure.ToString(),
                    Message = "Caller needs to authenticate. Unable to retrieve the access token silently."
                });
            }
        }

        public async Task<AuthenticationResult> GetUserAccessTokenByAuthorizationCode(string authorizationCode)
        {
            return await _app.AcquireTokenByAuthorizationCode(_scopes, authorizationCode).ExecuteAsync();
        }
    }

    public interface IGraphAuthProvider
    {
        string Authority { get; }

        Task<string> GetUserAccessTokenAsync(string userId);

        Task<AuthenticationResult> GetUserAccessTokenByAuthorizationCode(string authorizationCode);
    }
}
