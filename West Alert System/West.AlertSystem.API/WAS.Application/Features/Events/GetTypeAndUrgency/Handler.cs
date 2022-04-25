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

namespace WAS.Application.Features.Events.GetTypeAndUrgency
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
                var eventsTypes = await _context.EventTypes
                    .ToListAsync(cancellationToken);

                var eventUrgencies = await _context.EventUrgencies
                    .ToListAsync(cancellationToken);

                var response = new Response()
                {
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
