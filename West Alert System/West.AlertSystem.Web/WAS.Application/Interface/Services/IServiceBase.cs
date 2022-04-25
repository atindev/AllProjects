using System;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    /// <summary>
    /// Service base interface.
    /// </summary>
    public interface IServiceBase
    {
        /// <summary>
        /// Generates the authentication token async.
        /// </summary>
        /// <returns>The authentication token async.</returns>
        Task<string> GenerateAuthenticationTokenAsync(string resourceUrl = "");
    }
}
