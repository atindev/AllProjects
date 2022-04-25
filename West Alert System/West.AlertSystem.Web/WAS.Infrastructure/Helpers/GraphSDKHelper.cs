﻿/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.Graph;

namespace WAS.Infrastructure.Helpers
{
    public class GraphSdkHelper : IGraphSdkHelper
    {
        private readonly IGraphAuthProvider _authProvider;

        public GraphSdkHelper(IGraphAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        // Get an authenticated Microsoft Graph Service client.
        public GraphServiceClient GetAuthenticatedClient(ClaimsIdentity userIdentity)
        {
                var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    // Get user's id for token cache.
                    var identifier = userIdentity.FindFirst("https://schemas.microsoft.com/identity/claims/objectidentifier")?.Value + "." + userIdentity.FindFirst("https://schemas.microsoft.com/identity/claims/tenantid")?.Value;

                    // Passing tenant ID to the sample auth provider to use as a cache key
                    var accessToken = await _authProvider.GetUserAccessTokenAsync(identifier);

                    // Append the access token to the request
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // This header identifies the sample in the Microsoft Graph service. If extracting this code for your project please remove.
                    requestMessage.Headers.Add("SampleID", "aspnetcore-connect-sample");
                }));

            return graphClient;
        }
    }
    public interface IGraphSdkHelper
    {
        GraphServiceClient GetAuthenticatedClient(ClaimsIdentity userIdentity);
    }
}
