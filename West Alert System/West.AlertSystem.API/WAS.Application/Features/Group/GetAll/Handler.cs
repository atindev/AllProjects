using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using GetByMail = WAS.Application.Features.Subscription.GetByMail;
using GetByIds = WAS.Application.Features.Group.GetByIds;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;
using AutoMapper.Configuration;

namespace WAS.Application.Features.Group.GetAll
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;
        private readonly IMediator _mediator;
        private readonly IOptions<GroupRestorationCount> _groupOptions;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ITimeParser timeParser,
            IMediator mediator,
            IOptions<GroupRestorationCount> options
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _timeParser = timeParser;
            _mediator = mediator;
            _groupOptions = options;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                int Count = 0;
                var groups = await getGroupListAsync(request,cancellationToken);

                if (!request.IsGlobalAdmin && request.IsAccessRequired && request.Email!=null && groups.Any())
                    groups = await getAccessableGroupsAsync(groups,request.Email, cancellationToken);

               var responseGroups = new Response().Groups;
                groups.ForEach(x =>
               {
                   var group = _mapper.Map<Group>(x);

                   group.EmailSubscriptionCount = x.SubscriptionGroups.Count(i => (i.Subscription.IsOfficialEmail || i.Subscription.IsPersonalEmail));
                   group.TextSubscriptionCount = x.SubscriptionGroups.Count(i => (i.Subscription.IsTextOfficeMobile || i.Subscription.IsTextPersonalMobile));
                   group.VoiceSubscriptionCount = x.SubscriptionGroups.Count(i => (i.Subscription.IsVoiceHomePhone || i.Subscription.IsVoiceOfficeMobile || i.Subscription.IsVoiceOfficePhone || i.Subscription.IsVoicePersonalMobile));
                   group.WhatsAppSubscriptionCount = x.SubscriptionGroups.Count(i => (i.Subscription.IsWhatsAppOfficeMobile || i.Subscription.IsWhatsAppPersonalMobile));

                   responseGroups.Add(group);

               });

                if (request.PageType == "Paged" && responseGroups != null && responseGroups.Count > 0)
                {
                    Count = responseGroups.Count;
                    request.RowCount = (request.RowCount == 0) ? 7 : request.RowCount;
                    responseGroups = responseGroups.Skip(request.PageIndex).Take(request.RowCount).ToList();
                }
                if (responseGroups != null && responseGroups.Count > 0)
                {
                    responseGroups = await getUpdatedGroupsAsync(responseGroups, cancellationToken);
                }
                var response = new Response()
                {
                    Groups = responseGroups.OrderByDescending(o => o.ModifiedDate).ToList(),
                    Count = Count
                };

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }

        private async Task<List<Domain.Entities.Group>> getAccessableGroupsAsync(List<Domain.Entities.Group> groups,string email, CancellationToken cancellationToken)
        {
            List<Domain.Entities.Group> groupList = new List<Domain.Entities.Group>();

            for (int i = 0; i < groups.Count; i++)
            {
                var x = groups[i];
                
                if (x.CreatedBy.ToUpper().Trim() == email.ToUpper().Trim() || (!(x.IsAccessToAdmins ?? false) && !(x.IsPrivate ?? false)))
                    groupList.Add(x);
                
                else
                {
                    if ((x.IsPrivate ?? false) && x.CreatedBy.ToUpper().Trim() == email)
                        groupList.Add(x);
                    
                    else if ((x.IsAccessToAdmins ?? false))
                    {
                        var isPartoftheGroup = await isPartoftheGroupAsync(x.Id, email, cancellationToken);
                        if (isPartoftheGroup)
                            groupList.Add(x);
                    }
                }
            }

            return groupList;
        }

        private async Task<bool> isPartoftheGroupAsync(int groupId,string email, CancellationToken cancellationToken)
        {
            bool flag = false;
            var response = await _mediator.Send(new GetByMail.Request() { Email = email });

            if (response != null)
            {
                var subscriptionGroups = await _context.SubscriptionGroups
                   .Where(sg => (sg.GroupId==groupId && sg.SubscriptionId== response.Id))
                   .ToListAsync(cancellationToken);
                if (subscriptionGroups != null && subscriptionGroups.Any())
                    flag = true;
            }
            
            return flag;
        }

        private async Task<List<Domain.Entities.Group>> getGroupListAsync(Request request,CancellationToken cancellationToken)
        {
            var groups = new List<Domain.Entities.Group>();

            if (request.IsArchiveGroupRequired)
            {
                groups = await _context.Groups
                                      .IgnoreQueryFilters()
                                      .Include(i => i.SubscriptionGroups)
                                          .ThenInclude(i => i.Subscription)
                                      .Where(i => (request.GroupFilter == null
                                         || (i.Name.Contains(request.GroupFilter) || i.CreatedBy.Contains(request.GroupFilter)))
                                         && (i.IsActive || (i.DeletedDate != null && i.DeletedDate.Value.AddDays(_groupOptions.Value.DeletedGroupRententionDays) > DateTime.UtcNow))
                                         )
                                      .ToListAsync(cancellationToken);
                groups.ForEach(x => x.SubscriptionGroups = (x.SubscriptionGroups.Where(i => (i.IsActive && i.Subscription.IsActive)).ToList()));
            }
            else
            {
                groups = await _context.Groups
                  .Include(i => i.SubscriptionGroups)
                      .ThenInclude(i => i.Subscription)
                  .Where(i => (request.GroupFilter == null
                     || (i.Name.Contains(request.GroupFilter) || i.CreatedBy.Contains(request.GroupFilter))))
                  .ToListAsync(cancellationToken);
            }


            return groups;
        }

        private async Task<List<Group>> getUpdatedGroupsAsync(List<Group> responseGroups, CancellationToken cancellationToken)
        {
            var userList = responseGroups.Select(n => n.CreatedBy).Where(i => i != null).Distinct().ToList()
                                .ConvertAll(d => d.ToLower());

            var subscriptionLocation = await _context.Subscriptions
                                .IgnoreQueryFilters()
                                .Include(i => i.Location)
                                .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                                .ToListAsync(cancellationToken);

            responseGroups.ForEach(x =>
            {
                x.Updated = _timeParser.RelativeTime(x.ModifiedDate);

                if (x.CreatedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()))
                    x.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                else
                    x.CreaterLocation = "";
            });

            return responseGroups;
        }

    }
}
