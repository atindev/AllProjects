using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using System.Linq;
using WAS.Application.Common.Exceptions;

namespace WAS.Application.Features.IncomingMessage.GetAll
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
                var response = new Response();
                var incomingMessage = await _context.IncomingMessages
                                        .OrderByDescending(i => i.CreatedDate)
                                        .Where(i => (request.MessageFilter == null ||
                                                    i.Message.Contains(request.MessageFilter)) && i.NotificationId == null && !i.IsEmail)
                                        .ToListAsync(cancellationToken);

                var responseIncomingMessage = _mapper.Map<List<Common.Models.IncomingMessage>>(incomingMessage);

                response.Count = responseIncomingMessage.Count;

                if (request.PageType == "Paged" && responseIncomingMessage.Any())
                {
                    request.RowCount = (request.RowCount == 0) ? 7 : request.RowCount;
                    responseIncomingMessage = responseIncomingMessage.Skip(request.PageIndex).Take(request.RowCount).ToList();
                }

                if (responseIncomingMessage.Any())
                {
                    var userList = responseIncomingMessage.Select(n => n.SubscriberEmail).Where(i => i != null).Distinct().ToList()
                            .ConvertAll(d => d.ToLower());
                    var subscriptionLocation = await _context.Subscriptions
                            .IgnoreQueryFilters()
                            .Include(i => i.Location)
                            .Where(s => userList.Contains(s.OfficialEmail.ToLower()))
                            .ToListAsync(cancellationToken);

                    responseIncomingMessage.ForEach(x =>
                    {
                        x.MessageDate = _timeParser.RelativeTime(x.CreatedDate);

                        if (x.SubscriberEmail!=null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == x.SubscriberEmail.ToLower()))
                            x.SubscriberLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == x.SubscriberEmail.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                        else
                            x.SubscriberLocation = "";
                    });
                }

                response.IncomingMessages = responseIncomingMessage;

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
