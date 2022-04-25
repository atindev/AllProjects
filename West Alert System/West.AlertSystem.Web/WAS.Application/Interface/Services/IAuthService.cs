using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    /// <summary>
    /// Auth service interface.
    /// </summary>
    public interface IAuthService
    {
        Task<string> GenerateAuthenticationTokenAsync(string resourceUrl = "");
    }
}
