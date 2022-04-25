using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.IncomingMessage.GetById
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
                var incomingMessageEntity = await _context.IncomingMessages.SingleOrDefaultAsync(i => i.Id.Equals(request.Id), cancellationToken);

                if (incomingMessageEntity == null)
                    return null;

                var IncomingMessage = _mapper.Map<Common.Models.IncomingMessage>(incomingMessageEntity);
                if (IncomingMessage.SubscriberEmail != null)
                {
                    var subscriptionLocation = await _context.Subscriptions
                              .IgnoreQueryFilters()
                              .Include(i => i.Location)
                              .Where(s => s.OfficialEmail.ToLower() == IncomingMessage.SubscriberEmail.ToLower())
                              .ToListAsync(cancellationToken);

                    if (IncomingMessage.SubscriberEmail != null && subscriptionLocation.Any(i => i.OfficialEmail.ToLower() == IncomingMessage.SubscriberEmail.ToLower()))
                        IncomingMessage.SubscriberLocation = subscriptionLocation.Where(i => i.OfficialEmail.ToLower() == IncomingMessage.SubscriberEmail.ToLower()).Select(o => o.Location.Name).ElementAt(0);
                    else
                        IncomingMessage.SubscriberLocation = "";
                }
                else
                    IncomingMessage.SubscriberLocation = "";

                var response = new Response { IncomingMessage= IncomingMessage };

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
