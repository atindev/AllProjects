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
using GetAllShareSurvey = WAS.Application.Features.Survey.GetAllSharedSurvey;


namespace WAS.Application.Features.Survey.GetAllBroadcast
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;
        private readonly IMediator _mediator;
        int Count = 0;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
             IMediator mediator,
            ITimeParser timeParser
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _timeParser = timeParser;
            _mediator = mediator;
            Count = 0;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                Count = 0;
                var Surveys = await getSurveyList(request, cancellationToken);

                if (Surveys == null || Surveys.Count == 0)
                    return new Response();

                var SurveyObj = _mapper.Map<List<Common.Models.BroadcastedSurvey>>(Surveys);

                if (SurveyObj != null && SurveyObj.Any())
                {
                    var userList = SurveyObj.Select(n => n.CreatedBy).Where(i => i != null).Distinct().ToList()
                                .ConvertAll(d => d.ToLower());

                    var subscriptionLocation = await _context.Subscriptions
                                .IgnoreQueryFilters()
                                .Include(i => i.Location)
                                .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                                .ToListAsync(cancellationToken);

                    SurveyObj.ForEach(x =>
                    {
                        x.Updated = _timeParser.RelativeTime(x.CreatedDate);

                        x.OwnerWithoutSpecialCharacter = (x.CreatedBy != null) ? x.CreatedBy.ToLower().Replace("@", "") : "";

                        if (x.CreatedBy != null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()))
                            x.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.CreaterLocation = "";
                    });
                }

                return new Response { BroadcastedSurveys = SurveyObj, Count = Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        public async Task<List<Domain.Entities.SurveyBroadcast>> getSurveyList(Request request, CancellationToken cancellationToken)
        {
            var Surveys = new List<Domain.Entities.SurveyBroadcast>();
            string startDate = ((request.StartTime == null) ? null : Convert.ToString(request.StartTime));
            string endDate = ((request.EndTime == null) ? null : Convert.ToString(request.EndTime));

            var sharedSurveyResponse = await _mediator.Send(new GetAllShareSurvey.Request() { OfficialMail = request.UserMailId });
            if (request.PageType == "Paged")
            {
                if (startDate != null && endDate != null)
                    Surveys = await getSurveyListBetweenDates(request, startDate, endDate, sharedSurveyResponse.BroadcastedSurveyIds, cancellationToken);
                else
                    Surveys = await getPagedList(request, startDate, endDate, sharedSurveyResponse.BroadcastedSurveyIds, cancellationToken);
            }
            else
                Surveys = await getAllBroadcastedList(request, startDate, endDate, sharedSurveyResponse.BroadcastedSurveyIds, cancellationToken);

            return Surveys;
        }

        public async Task<List<Domain.Entities.SurveyBroadcast>> getPagedList(Request request, string startDate, string endDate, List<Guid> sharedSurvey, CancellationToken cancellationToken)
        {
            var Surveys = new List<Domain.Entities.SurveyBroadcast>();
            if (request.IsGlobalAdmin)
            {
                Count = await _context.SurveyBroadcasts
                                 .Include(s => s.Survey)
                                 .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                                  (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                                  (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                                  (startDate == null || i.StartTime.Date == (Convert.ToDateTime(startDate).Date)) &&
                                  (endDate == null || i.EndTime.Date == Convert.ToDateTime(endDate).Date) &&
                                              i.DeletedDate == null)
                                  .IgnoreQueryFilters().CountAsync(cancellationToken);

                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                          .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          (startDate == null || i.StartTime.Date == (Convert.ToDateTime(startDate).Date)) &&
                          (endDate == null || i.EndTime.Date == Convert.ToDateTime(endDate).Date) &&
                                       i.DeletedDate == null)
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .Skip(request.PageIndex).Take(request.RowCount)
                          .ToListAsync(cancellationToken);
            }
            else
            {
                Count = await _context.SurveyBroadcasts
                                 .Include(s => s.Survey)
                                 .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                                  (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                                  (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                                  (startDate == null || i.StartTime.Date == (Convert.ToDateTime(startDate).Date)) &&
                                  (endDate == null || i.EndTime.Date == Convert.ToDateTime(endDate).Date) &&
                                              i.DeletedDate == null && (i.CreatedBy == request.UserMailId || sharedSurvey.Contains(i.Id)))
                                  .IgnoreQueryFilters().CountAsync(cancellationToken);

                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                          .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          (startDate == null || i.StartTime.Date == (Convert.ToDateTime(startDate).Date)) &&
                          (endDate == null || i.EndTime.Date == Convert.ToDateTime(endDate).Date) &&
                                       i.DeletedDate == null && (i.CreatedBy == request.UserMailId || sharedSurvey.Contains(i.Id)))
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .Skip(request.PageIndex).Take(request.RowCount)
                          .ToListAsync(cancellationToken);
            }
            return Surveys;
        }

        public async Task<List<Domain.Entities.SurveyBroadcast>> getSurveyListBetweenDates(Request request, string start, string end, List<Guid> sharedSurvey, CancellationToken cancellationToken)
        {
            var Surveys = new List<Domain.Entities.SurveyBroadcast>();
            if (request.IsGlobalAdmin)
            {
                Count = await _context.SurveyBroadcasts
                             .Include(s => s.Survey)
                             .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                              (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                              (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                              ((i.StartTime.Date >= (Convert.ToDateTime(start).Date)) && (i.EndTime.Date <= Convert.ToDateTime(end).Date)) &&
                                         i.DeletedDate == null)
                              .IgnoreQueryFilters().CountAsync(cancellationToken);

                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                          .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          ((i.StartTime.Date >= (Convert.ToDateTime(start).Date)) && (i.EndTime.Date <= Convert.ToDateTime(end).Date)) &&
                                  i.DeletedDate == null)
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .Skip(request.PageIndex).Take(request.RowCount)
                          .ToListAsync(cancellationToken);

            }
            else
            {
                Count = await _context.SurveyBroadcasts
                             .Include(s => s.Survey)
                             .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                              (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                              (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                              ((i.StartTime.Date >= (Convert.ToDateTime(start).Date)) && (i.EndTime.Date <= Convert.ToDateTime(end).Date)) &&
                                         i.DeletedDate == null && (i.CreatedBy == request.UserMailId || sharedSurvey.Contains(i.Id)))
                              .IgnoreQueryFilters().CountAsync(cancellationToken);

                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                          .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                           .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          ((i.StartTime.Date >= (Convert.ToDateTime(start).Date)) && (i.EndTime.Date <= Convert.ToDateTime(end).Date)) &&
                                  i.DeletedDate == null && (i.CreatedBy == request.UserMailId || sharedSurvey.Contains(i.Id)))
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .Skip(request.PageIndex).Take(request.RowCount)
                          .ToListAsync(cancellationToken);
            }

            return Surveys;
        }

        public async Task<List<Domain.Entities.SurveyBroadcast>> getAllBroadcastedList(Request request, string startDate, string endDate, List<Guid> sharedSurvey, CancellationToken cancellationToken)
        {
            var Surveys = new List<Domain.Entities.SurveyBroadcast>();
            if (request.IsGlobalAdmin)
            {
                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                         .Include(i => i.SurveyBroadcastADUsers)
                            .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                            .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          (startDate == null || i.StartTime.Date >= (Convert.ToDateTime(startDate).Date)) &&
                          (endDate == null || i.EndTime.Date <= Convert.ToDateTime(endDate).Date) &&
                                      i.DeletedDate == null)
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .ToListAsync(cancellationToken);
            }
            else
            {
                Surveys = await _context.SurveyBroadcasts
                         .Include(s => s.Survey)
                         .Include(i => i.SurveyBroadcastGroups)
                           .ThenInclude(x => x.Group)
                         .Include(i => i.SurveyBroadcastSubscriptions)
                           .ThenInclude(i => i.Subscription)
                         .Include(i => i.SurveyBroadcastDistributionGroups)
                         .Include(i => i.SurveyBroadcastADUsers)
                            .ThenInclude(i => i.Location)
                         .Include(i => i.SurveyBroadcastADUsers)
                            .ThenInclude(i => i.Department)
                         .Where(i => (request.NameFilter == null || i.Survey.Subject.Contains(request.NameFilter)) &&
                          (request.StatusFilter == 0 || i.Status == request.StatusFilter) &&
                          (request.CreatedByFilter == null || i.CreatedBy == request.CreatedByFilter) &&
                          (startDate == null || i.StartTime.Date >= (Convert.ToDateTime(startDate).Date)) &&
                          (endDate == null || i.EndTime.Date <= Convert.ToDateTime(endDate).Date) &&
                                      i.DeletedDate == null && (i.CreatedBy == request.UserMailId || sharedSurvey.Contains(i.Id)))
                          .OrderByDescending(i => i.CreatedDate)
                          .IgnoreQueryFilters()
                          .ToListAsync(cancellationToken);
            }

            return Surveys;
        }

    }
}
