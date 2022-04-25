using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Survey.GetUniqueADPeople
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;
        private readonly IGraphService _graphService;
        private readonly IMapper _mapper;

        public Handler(
             IWasContextAdmin context,
             ILogger<Handler> logger,
             IGraphService graphService,
             IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _graphService = graphService;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                List<DistributionGroupMember> distinctRecords = null;
                List<DistributionGroupMember> distributionGroupMembers = await GetDistributionMembers(request, cancellationToken);

                if (request.ADPeople != null && request.ADPeople.Any())
                {
                    var admember = _mapper.Map<List<DistributionGroupMember>>(request.ADPeople);

                    if (distributionGroupMembers != null && request.ADPeople.Any())
                    {
                        var adPeople = distributionGroupMembers.Union(admember).GroupBy(g => g.EmailId.ToLower()).Select(x => x.First()).ToList();
                        distinctRecords = adPeople.Where(x => !request.UniqueSubscribers.Any(y => y.ToLower() == x.EmailId.ToLower())).ToList();
                    }
                    else if (distributionGroupMembers == null)
                    {
                        distinctRecords = admember.Where(x => !request.UniqueSubscribers.Any(y => y.ToLower() == x.EmailId.ToLower())).ToList();
                    }
                }
                else if (distributionGroupMembers != null)
                {
                    var distinctDG = distributionGroupMembers.GroupBy(g => g.EmailId.ToLower()).Select(x => x.First()).ToList();
                    distinctRecords = distinctDG.Where(x => !request.UniqueSubscribers.Any(y => y.ToLower() == x.EmailId.ToLower())).ToList();
                }

                return new Response { ADUser = distinctRecords };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Distribution Group memebers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<DistributionGroupMember>> GetDistributionMembers(Request request, CancellationToken cancellationToken)
        {
            List<DistributionGroupMember> distributionGroupMembers = null;

            if (request.DistributionGroups != null && request.DistributionGroups.Any())
            {
                distributionGroupMembers = new List<DistributionGroupMember>();
                foreach (var disGrp in request.DistributionGroups)
                {
                    var memberList = await _graphService.GetDistributionListMembers(disGrp.Id);
                    distributionGroupMembers.AddRange(memberList);

                    if (request.ShouldSaveToDB && memberList != null && memberList.Any())
                    {
                        await SaveDistributionGroupMembersToDB(request.SurveyBroadcastId, disGrp, memberList, cancellationToken);
                    }
                }
            }
            return distributionGroupMembers;
        }

        /// <summary>
        /// Save Distribution Group Members to DB
        /// </summary>
        /// <param name="surveyBroadcastId"></param>
        /// <param name="disGrp"></param>
        /// <param name="memberList"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task SaveDistributionGroupMembersToDB(Guid surveyBroadcastId, DistributionGroup disGrp, List<DistributionGroupMember> memberList, CancellationToken cancellationToken)
        {
            List<SurveyBroadcastADUser> surveyADPeople = new List<SurveyBroadcastADUser>();

            foreach (var member in memberList)
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name.Equals(member.Department), cancellationToken);
                var location = await _context.Locations.FirstOrDefaultAsync(d => d.Name.Equals(member.Location), cancellationToken);
                var adUser = new SurveyBroadcastADUser()
                {
                    FirstName = member.FirstName,
                    LastName = member.LastName,
                    EmailId = member.EmailId,
                    DepartmentId = department.Id,
                    LocationId = location.Id,
                    SurveyBroadcastId = surveyBroadcastId,
                    SurveyBroadcastDistributionGroupId = disGrp.SurveyBroadcastDistributionGroupId,
                    CreatedBy = disGrp.CreatedBy,
                    ModifiedBy = disGrp.CreatedBy,
                };
                surveyADPeople.Add(adUser);
            }

            if (surveyADPeople.Any())
            {
                await _context.SurveyBroadcastADUser.AddRangeAsync(surveyADPeople, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
