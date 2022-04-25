using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class GraphService : IGraphService
    {
        private readonly IGraphAuthProvider graphAuthProvider;
        private readonly ILogger<GraphService> _logger;
        private readonly IMapper _mapper;
        private readonly IOptions<GraphSettings> _graphOptions;


        public GraphService(IGraphAuthProvider graphAuthProvider, ILogger<GraphService> logger, IMapper mapper, IOptions<GraphSettings> options)
        {
            this.graphAuthProvider = graphAuthProvider;
            _logger = logger;
            _mapper = mapper;
            _graphOptions = options;
        }

        public async Task<UserDetails> GetUserByUPN(string upn)
        {
            UserDetails userDetails = new UserDetails();
            try
            {
                GraphServiceClient graphClient = new GraphServiceClient(graphAuthProvider.GetGraphAuthProvider());
                graphClient.BaseUrl = _graphOptions.Value.GraphBaseURL;

                var filterData = await graphClient.Users.Request().Filter($"startsWith(UserPrincipalName, '{upn}')").GetAsync();

                if (filterData != null && filterData.Count > 0)
                {
                    userDetails.IsSubscriberPresent = true;
                    userDetails.User = new UserDataFromAd()
                    {
                        EmployeeType = filterData.CurrentPage[0].EmployeeType,
                        DepartmentName = filterData.CurrentPage[0].Department,
                        JobTitle = filterData.CurrentPage[0].JobTitle,
                        PostalCodeFromAd = filterData.CurrentPage[0].PostalCode != null ? filterData.CurrentPage[0].PostalCode.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") : filterData.CurrentPage[0].PostalCode,
                        OfficeLocation = filterData.CurrentPage[0].OfficeLocation,
                        EmployeeGroup = filterData.CurrentPage[0].OnPremisesExtensionAttributes.ExtensionAttribute4,
                        CostCenter = filterData.CurrentPage[0].OnPremisesExtensionAttributes.ExtensionAttribute10,
                        EmployeeId = filterData.CurrentPage[0].EmployeeId,
                        Role = GetUserRole(filterData.CurrentPage[0].Mail).Result
                    };
                    return userDetails;
                }
                return userDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public async Task<ADUser> GetUser(string emailId)
        {
            if (emailId == null)
                return null;
            try
            {
                GraphServiceClient graphClient = new GraphServiceClient(graphAuthProvider.GetGraphAuthProvider());
                graphClient.BaseUrl = _graphOptions.Value.GraphBaseURL;

                var request = graphClient.Users.Request()
                            .Header("ConsistencyLevel", "eventual")
                            .Filter($"(mail eq '{emailId}') or (employeeId eq '{emailId}') or (userPrincipalName eq '{emailId}') or (onPremisesSamAccountName eq '{emailId}')");
                request.QueryOptions.Add(new QueryOption("$count", "true"));
                var user = await request.GetAsync();

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

        public async Task<string> GetUserRole(string email)
        {
            if (email == null)
                return null;

            try
            {
                GraphServiceClient _graphClient = new GraphServiceClient(graphAuthProvider.GetGraphAuthProvider());
                _graphClient.BaseUrl = "https://graph.microsoft.com/v1.0";

                var userRoles = await _graphClient.Users[email].AppRoleAssignments.Request().GetAsync();
                var appRoleAssignments = await _graphClient.ServicePrincipals[_graphOptions.Value.AppResourceId].Request().GetAsync();

                var roles = new List<string>();
                if (appRoleAssignments != null && appRoleAssignments.AppRoles.Any())
                {
                    var userRolesOfCurrentResource = userRoles.Where(role => role.ResourceId == Guid.Parse(_graphOptions.Value.AppResourceId)).ToList();
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

        public async Task<List<DistributionGroupMember>> GetDistributionListMembers(string distributionListId)
        {
            try
            {
                List<DistributionGroupMember> membersList = new List<DistributionGroupMember>();
                GraphServiceClient graphClient = new GraphServiceClient(graphAuthProvider.GetGraphAuthProvider());
                graphClient.BaseUrl = _graphOptions.Value.GraphBaseURL;

                var members = await graphClient.Groups[distributionListId].TransitiveMembers
                    .Request()
                    .GetAsync();

                while (true)
                {
                    foreach (var member in members.OfType<Microsoft.Graph.User>())
                    {
                        if (member != null && member.Mail != null)
                        {
                            var dGmember = new DistributionGroupMember()
                            {
                                FirstName = member.GivenName,
                                LastName = member.Surname,
                                EmailId = member.Mail,
                                Department = member.Department,
                                Location = member.OfficeLocation
                            };
                            membersList.Add(dGmember);
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

    }
}
