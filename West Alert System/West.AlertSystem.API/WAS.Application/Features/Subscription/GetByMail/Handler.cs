using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using System.Collections.Generic;

namespace WAS.Application.Features.Subscription.GetByMail
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
                var subscriptionEntity = await _context.Subscriptions
                    .Include(i=>i.Location)
                    .Include(i=>i.Shift)
                    .Include(i=>i.SubscriptionGroups)
                        .ThenInclude(i=>i.Group)
                    .SingleOrDefaultAsync(s => s.OfficialEmail == request.Email || s.PersonalEmail == request.Email);

                if (subscriptionEntity == null)
                    return null;

                var response = _mapper.Map<Response>(subscriptionEntity);
                response.Groups = _mapper.Map<List<Common.Models.Group>>(subscriptionEntity.SubscriptionGroups);

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
