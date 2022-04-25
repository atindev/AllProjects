using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    /// <summary>
    /// Service base class for holding common services
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        /// <summary>
        /// The auth service.
        /// </summary>
        private readonly IAuthService authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DTDeviceConnectedPlatform.Core.Services.ServiceBase"/> class.
        /// </summary>
        /// <param name="authService">Auth service.</param>
        public ServiceBase(IAuthService authService)
        {   
            this.authService = authService;
        }

        /// <summary>
        /// Generates the authentication token.
        /// </summary>
        /// <returns>The authentication token.</returns>
        public async Task<string> GenerateAuthenticationTokenAsync(string resourceUrl = "")
        {
            var token = await this.authService.GenerateAuthenticationTokenAsync(resourceUrl);
            return token;
        }
    }
}
