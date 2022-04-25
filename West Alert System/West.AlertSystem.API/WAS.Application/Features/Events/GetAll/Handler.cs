using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Domain.Entities;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Events.GetAll
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ITimeParser _timeParser;

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
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var events = await _context.Events
                    .Include(i => i.EventType)
                    .Include(i => i.EventUrgency)
                    .Include(i => i.Notifications)
                    .IgnoreQueryFilters()
                    .OrderByDescending(o => o.ModifiedDate)
                    .ToListAsync(cancellationToken);

                var eventsTypes = await _context.EventTypes
                    .ToListAsync(cancellationToken);

                var eventUrgencies = await _context.EventUrgencies
                    .ToListAsync(cancellationToken);

                var responseEvents = _mapper.Map<List<Common.Models.Event>>(events);

                if (responseEvents != null && responseEvents.Any())
                {
                    var userList = responseEvents.Select(n => n.CreatedBy).Where(i => i != null).Distinct().ToList()
                                .ConvertAll(d => d.ToLower());

                    var subscriptionLocation = await _context.Subscriptions
                                .IgnoreQueryFilters()
                                .Include(i => i.Location)
                                .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                                .ToListAsync(cancellationToken);

                    responseEvents.ForEach(x =>
                    {
                        x.Updated = _timeParser.RelativeTime(x.ModifiedDate);

                        if (x.CreatedBy!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()))
                            x.CreaterLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.CreatedBy.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.CreaterLocation = "";
                    });
                }

                var response = new Response()
                {
                    Events = responseEvents,
                    EventTypes = _mapper.Map<List<Common.Models.EventType>>(eventsTypes),
                    EventUrgencies = _mapper.Map<List<Common.Models.EventUrgency>>(eventUrgencies),
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
