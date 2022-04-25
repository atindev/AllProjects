using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface IGraphService
    {
        Task<ADUser> GetUser(string email, ClaimsPrincipal claimsPrincipal);

        Task<ADUser> GetUserManagerJson(string email, ClaimsPrincipal claimsPrincipal);

        Task<List<string>> GetAllUserJson(ClaimsPrincipal claimsPrincipal);

        Task<string> GetPictureBase64(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal);

        Task<Stream> GetPictureStream(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal);

        Task<Stream> GetMyPictureStream(HttpContext httpContext, ClaimsPrincipal claimsPrincipal);

        Task<List<User>> GetReportees(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal);

        Task GetWorkingHours(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal);

        Task<List<string>> GetPeopleWorkingWith(string email);

        Task<ADUserForValidation> GetUserDetails(string input, ClaimsPrincipal claimsPrincipal);

        Task<ADUserForValidation> GetUserDetailsWithManager(string input, ClaimsPrincipal claimsPrincipal);

        Task<string> GetUserRole(string email);

        Task<List<ADUser>> GetMatchingUsers(string searchString, ClaimsPrincipal claimsPrincipal);

        Task<List<DistributionGroup>> GetDistributionLists(string searchString, ClaimsPrincipal claimsPrincipal);

        Task<List<string>> GetDistributionListMembers(string distributionListId, ClaimsPrincipal claimsPrincipal);

        Task<ADUser> GetUserWithUserId(string emailId, ClaimsPrincipal claimsPrincipal);
    }
}
