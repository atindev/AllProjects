using System.Collections.Generic;
using System.Threading.Tasks;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface IGraphService
    {
        Task<UserDetails> GetUserByUPN(string upn);

        Task<ADUser> GetUser(string emailId);

        Task<List<DistributionGroupMember>> GetDistributionListMembers(string distributionListId);
    }
}
