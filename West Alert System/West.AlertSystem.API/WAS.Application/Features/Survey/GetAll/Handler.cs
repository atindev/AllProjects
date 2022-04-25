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
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetAll
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;
        int Count = 0;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ITimeParser timeParser
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _timeParser = timeParser;
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

                var SurveyObj = _mapper.Map<List<Common.Models.Survey>>(Surveys);

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

                return new Response { Surveys = SurveyObj, Count = Count };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        public async Task<List<Domain.Entities.Survey>> getSurveyList(Request request, CancellationToken cancellationToken)
        {
            var Surveys = new List<Domain.Entities.Survey>();

            if (request.PageType == "Paged")
            {
                if (string.IsNullOrEmpty(request.NameFilter) && string.IsNullOrEmpty(request.CreatedByFilter))
                {
                    var getSurveyList = await GetSurveyList(request, cancellationToken);
                    Surveys.AddRange(getSurveyList.Surveys);
                    Count = getSurveyList.Count;
                }
                else if(request.NameFilter!=null && request.CreatedByFilter==null)
                {
                    var surveyListBySubject = await SurveyListBySubject(request, cancellationToken);

                        Count = surveyListBySubject.Count;
                        Surveys.AddRange(surveyListBySubject.Surveys);

                }
               else if (request.NameFilter == null && request.CreatedByFilter != null)
                {
                    var surveyListByCreatedBy = await SurveyListByCreatedBy(request, cancellationToken);

                        Count = surveyListByCreatedBy.Count;
                        Surveys.AddRange(surveyListByCreatedBy.Surveys);
                    
                }
                else
                {
                    var subjectAndCreatedByFilter = await SurveyListBySubjectAndCreatedBy(request, cancellationToken);

                    Count = subjectAndCreatedByFilter.Count;
                    Surveys.AddRange(subjectAndCreatedByFilter.Surveys);

                }
            }
            else
            {
                Surveys = await _context.Surveys
               .Include(s => s.SurveyBroadcasts)
               .IgnoreQueryFilters()
               .Where(i => (i.IsActive && (request.NameFilter == null || (i.Subject.Contains(request.NameFilter) || i.Description.Contains(request.NameFilter))))
                     )
               .OrderByDescending(i => i.CreatedDate)
               .ToListAsync(cancellationToken);
            }

            return Surveys;

        }

        public async Task<SurveyFilterResponse> SurveyListBySubject(Request request, CancellationToken cancellationToken)
        {
            SurveyFilterResponse surveyFilterResponse = new SurveyFilterResponse
            {
                Surveys = await _context.Surveys
              .Include(s => s.SurveyBroadcasts)
              .IgnoreQueryFilters()
              .OrderByDescending(i => i.CreatedDate)
              .Where(i => (i.IsActive && (i.Subject.Contains(request.NameFilter) || i.Description.Contains(request.NameFilter))))
              .Skip(request.PageIndex).Take(request.RowCount)
              .ToListAsync(cancellationToken),

                Count = await _context.Surveys.Where(i =>(i.Subject.Contains(request.NameFilter) || i.Description.Contains(request.NameFilter))).CountAsync(cancellationToken)
            };
            return surveyFilterResponse;

        }

        public async Task<SurveyFilterResponse> SurveyListByCreatedBy(Request request, CancellationToken cancellationToken)
        {
            SurveyFilterResponse surveyFilterResponse = new SurveyFilterResponse
            {
                Surveys = await _context.Surveys
               .Include(s => s.SurveyBroadcasts)
               .IgnoreQueryFilters()
               .OrderByDescending(i => i.CreatedDate)
               .Where(i => (i.IsActive && (i.CreatedBy == request.CreatedByFilter))
                     )
               .Skip(request.PageIndex).Take(request.RowCount)
               .ToListAsync(cancellationToken),
                Count = await _context.Surveys.Where(i =>(i.CreatedBy == request.CreatedByFilter)).CountAsync(cancellationToken)
            };

            return surveyFilterResponse;
        }
        public async Task<SurveyFilterResponse> GetSurveyList(Request request, CancellationToken cancellationToken)
        {
            SurveyFilterResponse surveyFilterResponse = new SurveyFilterResponse
            {
                Surveys = await _context.Surveys
               .Include(s => s.SurveyBroadcasts)
               .IgnoreQueryFilters()
               .OrderByDescending(i => i.CreatedDate)
               .Where(i => (i.IsActive))
               .Skip(request.PageIndex).Take(request.RowCount)
               .ToListAsync(cancellationToken),

                Count = await _context.Surveys.CountAsync(cancellationToken)
            };
            return surveyFilterResponse;
        }
        public async Task<SurveyFilterResponse> SurveyListBySubjectAndCreatedBy(Request request, CancellationToken cancellationToken)
        {
            SurveyFilterResponse surveyFilterResponse = new SurveyFilterResponse
            {
                Surveys = await _context.Surveys
              .Include(s => s.SurveyBroadcasts)
              .IgnoreQueryFilters()
              .OrderByDescending(i => i.CreatedDate)
              .Where(i => (i.IsActive && i.Subject.Contains(request.NameFilter) && (i.CreatedBy == request.CreatedByFilter)) || (i.Description.Contains(request.NameFilter) && (i.CreatedBy == request.CreatedByFilter)))
              .Skip(request.PageIndex).Take(request.RowCount)
              .ToListAsync(cancellationToken),

                Count = await _context.Surveys.Where(i => (i.Subject.Contains(request.NameFilter) && (i.CreatedBy == request.CreatedByFilter))||(i.Description.Contains(request.NameFilter) && (i.CreatedBy == request.CreatedByFilter))).CountAsync(cancellationToken)
            };
            return surveyFilterResponse;

        }
    }
}
