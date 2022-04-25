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

namespace WAS.Application.Features.Report.GetAllGroupSize
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                List<Domain.Entities.SubscriptionGroup> subscriptionGroupData = new List<Domain.Entities.SubscriptionGroup>();
                if (request.LocationId != 0)
                {
                    var officeEmail = await _context.Subscriptions.IgnoreQueryFilters().Where(x => x.LocationId == request.LocationId && x.IsActive).Select(x => x.OfficialEmail).ToListAsync(cancellationToken);
                    foreach (var item in officeEmail)
                    {
                        var subscriptionGroups = await _context.SubscriptionGroups.Include(x => x.Group).Where(x => x.IsActive && x.Group.IsPrivate == false).IgnoreQueryFilters().Where(x => x.CreatedBy == item).ToListAsync(cancellationToken);
                        if (subscriptionGroups.Any())
                        {
                            subscriptionGroupData.AddRange(subscriptionGroups);
                        }
                    }
                }
                else
                {
                    subscriptionGroupData = await _context.SubscriptionGroups.Include(x => x.Group).Where(x => x.IsActive && x.Group.IsPrivate == false).IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                
                var groupdictionary = subscriptionGroupData.GroupBy(x => new { x.GroupId }).Select(x => new { x.Key.GroupId }).ToList();
                
                 
                List<GroupSize> AllGroups = new List<GroupSize>();


                foreach (var item in groupdictionary)
                {
                    GroupSize groupsize= new GroupSize()
                    {

                        GroupName = subscriptionGroupData.Where(x => x.GroupId == item.GroupId).Select(z => z.Group.Name).First(),
                        Count = subscriptionGroupData.Where(x => x.GroupId == item.GroupId).ToList().Count
                    };

                    AllGroups.Add(groupsize);
                }
                var responseAllGroup = _mapper.Map<List<GroupSize>>(AllGroups.OrderByDescending(x => x.Count));
                return new Response { AllGroups = responseAllGroup };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
