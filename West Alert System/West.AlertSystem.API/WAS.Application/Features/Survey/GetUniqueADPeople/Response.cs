using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetUniqueADPeople
{
    public class Response
    {
        public List<DistributionGroupMember> ADUser { get; set; }
    }
}
