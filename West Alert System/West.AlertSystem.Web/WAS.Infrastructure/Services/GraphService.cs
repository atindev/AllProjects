/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;
using WAS.Infrastructure.Helpers;
using Microsoft.Graph.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;

namespace WAS.Infrastructure.Services
{
    public class GraphService : IGraphService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GraphService> _logger;
        private GraphServiceClient _graphClient;
        public readonly IGraphSdkHelper _graphSdkHelper;
        private readonly IConfiguration _configuration;
        private readonly GraphSettings _graphOptions;
       
        public GraphService(IMapper mapper, ILogger<GraphService> logger, IGraphSdkHelper graphSdkHelper, IConfiguration configuration, IOptions<GraphSettings> options)
        {
            _mapper = mapper;
            _logger = logger;
            _graphSdkHelper = graphSdkHelper;
            _configuration = configuration;
            _graphOptions = options.Value;
        }

        public async Task<ADUser> GetUser(string email, ClaimsPrincipal claimsPrincipal)
        {
            if (email == null)
                return null;

            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                _graphClient.BaseUrl = "https://graph.microsoft.com/beta";
                var filterString = $"startswith(mail, '{email}') or startswith(employeeId, '{email}') or startswith(userPrincipalName, '{email}')";

                var user = await _graphClient.Users.Request().Filter(filterString).GetAsync();

                var adUser = _mapper.Map<ADUser>(user.FirstOrDefault());

                return adUser;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<ADUser> GetUserManagerJson(string email, ClaimsPrincipal claimsPrincipal)
        {
            if (email == null)
                return null;

            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                var user = await _graphClient.Users[email].Manager.Request().GetAsync();
                return _mapper.Map<ADUser>(user);
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<List<string>> GetAllUserJson(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                List<string> allUsers = new List<string>();
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                var pagedUserList = await _graphClient.Users.Request().Select("Mail").GetAsync();

                while (true)
                {
                    foreach (var user in pagedUserList)
                    {
                        if (user.Mail != null)
                        {
                            allUsers.Add(user.Mail);
                        }
                    }
                    if (pagedUserList.NextPageRequest != null)
                    {
                        pagedUserList = await pagedUserList.NextPageRequest.GetAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                return allUsers;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
        }

        public async Task<string> GetPictureBase64(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            if (email == null)
                return null;

            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                _graphClient.BaseUrl = "https://graph.microsoft.com/beta";
                // Load user's profile picture.
                var pictureStream = await GetPictureStream(email, httpContext, claimsPrincipal);

                if (pictureStream == null)
                {
                    return null;
                }

                // Copy stream to MemoryStream object so that it can be converted to byte array.
                var pictureMemoryStream = new MemoryStream();
                await pictureStream.CopyToAsync(pictureMemoryStream);

                // Convert stream to byte array.
                var pictureByteArray = pictureMemoryStream.ToArray();

                // Convert byte array to base64 string.
                var pictureBase64 = Convert.ToBase64String(pictureByteArray);

                if (string.IsNullOrEmpty(pictureBase64))
                {
                    return null;
                }

                return "data:image/jpeg;base64," + pictureBase64;
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "ResourceNotFound":
                        // If picture not found, return the default image.
                        return "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4NCjwhRE9DVFlQRSBzdmcgIFBVQkxJQyAnLS8vVzNDLy9EVEQgU1ZHIDEuMS8vRU4nICAnaHR0cDovL3d3dy53My5vcmcvR3JhcGhpY3MvU1ZHLzEuMS9EVEQvc3ZnMTEuZHRkJz4NCjxzdmcgd2lkdGg9IjQwMXB4IiBoZWlnaHQ9IjQwMXB4IiBlbmFibGUtYmFja2dyb3VuZD0ibmV3IDMxMi44MDkgMCA0MDEgNDAxIiB2ZXJzaW9uPSIxLjEiIHZpZXdCb3g9IjMxMi44MDkgMCA0MDEgNDAxIiB4bWw6c3BhY2U9InByZXNlcnZlIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPg0KPGcgdHJhbnNmb3JtPSJtYXRyaXgoMS4yMjMgMCAwIDEuMjIzIC00NjcuNSAtODQzLjQ0KSI+DQoJPHJlY3QgeD0iNjAxLjQ1IiB5PSI2NTMuMDciIHdpZHRoPSI0MDEiIGhlaWdodD0iNDAxIiBmaWxsPSIjRTRFNkU3Ii8+DQoJPHBhdGggZD0ibTgwMi4zOCA5MDguMDhjLTg0LjUxNSAwLTE1My41MiA0OC4xODUtMTU3LjM4IDEwOC42MmgzMTQuNzljLTMuODctNjAuNDQtNzIuOS0xMDguNjItMTU3LjQxLTEwOC42MnoiIGZpbGw9IiNBRUI0QjciLz4NCgk8cGF0aCBkPSJtODgxLjM3IDgxOC44NmMwIDQ2Ljc0Ni0zNS4xMDYgODQuNjQxLTc4LjQxIDg0LjY0MXMtNzguNDEtMzcuODk1LTc4LjQxLTg0LjY0MSAzNS4xMDYtODQuNjQxIDc4LjQxLTg0LjY0MWM0My4zMSAwIDc4LjQxIDM3LjkgNzguNDEgODQuNjR6IiBmaWxsPSIjQUVCNEI3Ii8+DQo8L2c+DQo8L3N2Zz4NCg==";
                    case "EmailIsNull":
                        return JsonConvert.SerializeObject(new { Message = "Email address cannot be null." }, Formatting.Indented);
                    default:
                        return null;
                }
            }
        }

        public async Task<Stream> GetPictureStream(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            if (email == null)
                return null;

            _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
            Stream pictureStream;
            try
            {
                // Load user's profile picture.
                pictureStream = await _graphClient.Users[email].Photo.Content.Request().GetAsync();
            }
            catch (ServiceException e)
            {
                switch (e.Error.Code)
                {
                    case "GetUserPhoto":
                        // Set Microsoft Graph endpoint to beta, to be able to get profile picture for MSAs 
                        _graphClient.BaseUrl = "https://graph.microsoft.com/beta";
                        // Get profile picture from Microsoft Graph
                        pictureStream = await _graphClient.Users[email].Photo.Content.Request().GetAsync();
                        // Reset Microsoft Graph endpoint to v1.0
                        _graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";
                        break;
                    case "Request_ResourceNotFound":
                    case "ResourceNotFound":
                    case "ErrorItemNotFound":
                    case "TokenNotFound":
                        await httpContext.ChallengeAsync();
                        pictureStream = null;
                        break;
                    default:
                        pictureStream = null;
                        break;
                }
            }

            return pictureStream;
        }

        public async Task<Stream> GetMyPictureStream(HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            Stream pictureStream = null;

            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                try
                {
                    // Load user's profile picture.
                    pictureStream = await _graphClient.Me.Photo.Content.Request().GetAsync();
                }
                catch (ServiceException e)
                {
                    if (e.Error.Code == "GetUserPhoto") // User is using MSA, we need to use beta endpoint
                    {
                        // Set Microsoft Graph endpoint to beta, to be able to get profile picture for MSAs 
                        _graphClient.BaseUrl = "https://graph.microsoft.com/beta";

                        // Get profile picture from Microsoft Graph
                        pictureStream = await _graphClient.Me.Photo.Content.Request().GetAsync();

                        // Reset Microsoft Graph endpoint to v1.0
                        _graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";
                    }
                }
            }
            catch (ServiceException e)
            {
                switch (e.Error.Code)
                {
                    case "Request_ResourceNotFound":
                    case "ResourceNotFound":
                    case "ErrorItemNotFound":
                    case "itemNotFound":
                    case "TokenNotFound":
                        await httpContext.ChallengeAsync();
                        return null;
                    default:
                        return null;
                }
            }

            return pictureStream;
        }

        public async Task<List<User>> GetReportees(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            List<User> reportees = new List<User>();
            _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
            var directoryRecports = await _graphClient.Users[email].DirectReports.Request().GetAsync();
            foreach (var reportee in directoryRecports)
            {
                var user = await _graphClient.Users[reportee.Id].Request().GetAsync();
                user.DirectReports = await _graphClient.Users[reportee.Id].DirectReports.Request().Select("Count").GetAsync();
                reportees.Add(user);
            }
            return reportees;
        }

        public async Task GetWorkingHours(string email, HttpContext httpContext, ClaimsPrincipal claimsPrincipal)
        {
            _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
            await _graphClient.Users[email].Request().Select(e => new
            {
                e.MailboxSettings
            })
            .GetAsync();
        }

        public async Task<List<string>> GetPeopleWorkingWith(string email)
        {
            if (email == null)
                return null;
            try
            {
                var azureOptions = new AzureAdOptions();
                _configuration.Bind("AzureAd", azureOptions);

                List<string> reportees = new List<string>();

                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(azureOptions.ClientId)
                .WithTenantId(azureOptions.TenantId)
                .WithClientSecret(azureOptions.ClientSecret)
                .Build();
                ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);
                _graphClient = new GraphServiceClient(authenticationProvider);
                _graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";

                var users = await _graphClient.Users[email].People.Request().GetAsync();

                if (users != null && users.CurrentPage.Count > 0)
                    reportees = users.CurrentPage.Where(i => i.UserPrincipalName != null && i.UserPrincipalName.ToUpper() != email.ToUpper()).Select(x => x.UserPrincipalName).ToList();

                return reportees;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<ADUserForValidation> GetUserDetails(string input, ClaimsPrincipal claimsPrincipal)
        {
            if (input == null)
                return null;
            try
            {
                var response = new ADUserForValidation();

                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                _graphClient.BaseUrl = "https://graph.microsoft.com/beta";

                var request = _graphClient.Users.Request()
                    .Header("ConsistencyLevel", "eventual")
                    .Filter($"(mail eq '{input}') or (employeeId eq '{input}') or (userPrincipalName eq '{input}') or (onPremisesSamAccountName eq '{input}')");
                request.QueryOptions.Add(new QueryOption("$count", "true"));
                var pagedUserList = await request.GetAsync();

                if (pagedUserList != null && pagedUserList.Any())
                {
                    response.Email = pagedUserList[0].Mail;
                    response.Location = pagedUserList[0].OfficeLocation;
                    response.Department = pagedUserList[0].Department;
                    response.EmployeeId = pagedUserList[0].EmployeeId;
                    response.Name = pagedUserList[0].DisplayName;
                }
                else
                    return null;

                return response;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<ADUserForValidation> GetUserDetailsWithManager(string input, ClaimsPrincipal claimsPrincipal)
        {
            if (input == null)
                return null;
            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);

                var response = await GetUserDetails(input, claimsPrincipal);

                if (response == null || response.Email == null)
                    return null;

                var user = await _graphClient.Users[response.Email].Manager.Request().GetAsync();
                if (user != null)
                    response.Manager = _mapper.Map<ADUser>(user).Email;

                return response;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<string> GetUserRole(string email)
        {
            if (email == null)
                return null;

            try
            {
                var azureOptions = new AzureAdOptions();
                _configuration.Bind("AzureAd", azureOptions);

                IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(azureOptions.ClientId)
                .WithTenantId(azureOptions.TenantId)
                .WithClientSecret(azureOptions.ClientSecret)
                .Build();
                ClientCredentialProvider authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);
                _graphClient = new GraphServiceClient(authenticationProvider);
                _graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";

                var userRoles = await _graphClient.Users[email].AppRoleAssignments.Request().GetAsync();
                var appRoleAssignments = await _graphClient.ServicePrincipals[_graphOptions.AppResourceId].Request().GetAsync();

                var roles = new List<string>();
                if (appRoleAssignments != null && appRoleAssignments.AppRoles.Any())
                {
                    var userRolesOfCurrentResource = userRoles.Where(role => role.ResourceId == Guid.Parse(_graphOptions.AppResourceId)).ToList();
                    if (userRolesOfCurrentResource != null)
                    {
                        userRolesOfCurrentResource.ForEach(x =>
                        {
                            var role = appRoleAssignments.AppRoles.FirstOrDefault(role => role.Id == x.AppRoleId);
                            if (role != null)
                            {
                                roles.Add(role.Value);
                            }
                        });
                    }
                }
                if (roles.Contains("GlobalAdministrator"))
                {
                    return "GlobalAdministrator";
                }
                else if (roles.Contains("WASAdmin"))
                {
                    return "WASAdmin";
                }
                else
                {
                    return roles.FirstOrDefault();
                }
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<List<ADUser>> GetMatchingUsers(string searchString, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                List<ADUser> allUsers = new List<ADUser>();
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                _graphClient.BaseUrl = "https://graph.microsoft.com/beta";
                var request = _graphClient.Users.Request()
                    .Header("ConsistencyLevel", "eventual")
                    .Filter($"endsWith(mail, '@westpharma.com') and (startswith(surname, '{searchString}') or startswith(givenName, '{searchString}') or startswith(mail, '{searchString}') or startswith(displayName, '{searchString}'))");
                request.QueryOptions.Add(new QueryOption("$count", "true"));
                var pagedUserList = await request.GetAsync();

                while (true)
                {
                    foreach (var user in pagedUserList)
                    {
                        if (user.Mail != null && !user.Mail.ToLower().Contains("office"))
                        {
                            var aduser = new ADUser();
                            aduser.FullName = user.DisplayName;
                            aduser.FirstName = user.GivenName;
                            aduser.LastName = user.Surname;
                            aduser.UserPrincipalName = user.UserPrincipalName;
                            aduser.Department = user.Department;
                            aduser.Location = user.OfficeLocation;
                            allUsers.Add(aduser);
                        }
                    }
                    if (pagedUserList.NextPageRequest != null)
                    {
                        pagedUserList = await pagedUserList.NextPageRequest.Header("ConsistencyLevel", "eventual").GetAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                return allUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<List<DistributionGroup>> GetDistributionLists(string searchString, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                List<DistributionGroup> distributionLists = new List<DistributionGroup>();
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                var filterString = $"mailEnabled eq true and startswith(mail, '{searchString}') or startswith(displayName, '{searchString}')";

                var pagedDistributionList = await _graphClient.Groups
                    .Request()
                    .Header("ConsistencyLevel", "eventual")
                    .Filter(filterString)
                    .GetAsync();

                while (true)
                {
                    foreach (var dl in pagedDistributionList)
                    {
                        if (dl.Mail != null)
                        {
                            var distributionGroup = new DistributionGroup()
                            {
                                Id = dl.Id,
                                Name = dl.DisplayName,
                                EmailId = dl.Mail,
                            };
                            distributionLists.Add(distributionGroup);
                        }
                    }
                    if (pagedDistributionList.NextPageRequest != null)
                    {
                        pagedDistributionList = await pagedDistributionList.NextPageRequest.GetAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                return distributionLists;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
        }

        public async Task<List<string>> GetDistributionListMembers(string distributionListId, ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                List<string> membersList = new List<string>();
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                var members = await _graphClient.Groups[distributionListId].Members
                    .Request()
                    .GetAsync();

                while (true)
                {
                    foreach (User member in members.OfType<User>())
                    {
                        if (member != null && member.Mail != null)
                        {
                            membersList.Add(member.Mail);
                        }
                    }
                    if (members.NextPageRequest != null)
                    {
                        members = await members.NextPageRequest.GetAsync();
                    }
                    else
                    {
                        break;
                    }
                }

                return membersList;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
        }


        public async Task<ADUser> GetUserWithUserId(string emailId, ClaimsPrincipal claimsPrincipal)
        {
            if (emailId == null)
                return null;
            try
            {
                _graphClient = _graphSdkHelper.GetAuthenticatedClient((ClaimsIdentity)claimsPrincipal.Identity);
                _graphClient.BaseUrl = "https://graph.microsoft.com/beta";

                var request = _graphClient.Users.Request()
                     .Header("ConsistencyLevel", "eventual")
                     .Filter($"(mail eq '{emailId}') or (employeeId eq '{emailId}') or (userPrincipalName eq '{emailId}') or (onPremisesSamAccountName eq '{emailId}')");
                request.QueryOptions.Add(new QueryOption("$count", "true"));
                var pagedUserList = await request.GetAsync();

                var adUser = _mapper.Map<ADUser>(pagedUserList.FirstOrDefault());
                return adUser;
            }
            catch (ServiceException se)
            {
                _logger.LogError(se.Message, se);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
