using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Events.CreateUpdate
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
                var existingEvent = await _context.Events
                            .FirstOrDefaultAsync(b => b.Name.Trim().ToUpper() == request.Name.Trim().ToUpper(), cancellationToken);

                var existingEventById = await _context.Events
                            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);
                if (existingEventById == null)
                {
                    if (request.Id != Guid.Empty && existingEvent.Id == request.Id && existingEvent.Name == request.Name)
                    {
                        existingEvent = null;
                    }

                    if (existingEvent != null)
                        return new Response { Success = true, Id = Guid.Empty, Name = "", IsNameExist = true };

                }

                var newevent = _mapper.Map<Event>(request);

               await Events(request, newevent, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Success = true, Id=newevent.Id, Name = newevent.Name };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        public async Task  Events(Request request,Event newevent, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                if (request.TypeId == 0)
                {
                    var eventType = await _context.EventTypes
                    .FirstOrDefaultAsync(i => i.Name.ToUpper() == "GENERAL", cancellationToken);

                    if (eventType == null)
                        throw new BadRequestException("Event Type not found");

                    newevent.TypeId = eventType.Id;
                }
                if (request.UrgencyId == 0)
                {
                    var eventUrgency = await _context.EventUrgencies
                    .FirstOrDefaultAsync(i => i.Name.ToUpper() == "MEDIUM", cancellationToken);

                    if (eventUrgency == null)
                        throw new BadRequestException("Event Urgency not found");

                    newevent.UrgencyId = eventUrgency.Id;
                }

                newevent.ModifiedDate = DateTime.UtcNow;
                await _context.Events.AddAsync(newevent, cancellationToken);
            }
            else
            {
                var result = await _context.Events
               .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                if (result == null)
                    throw new BadRequestException("Event not found");

                var eventEntity = _mapper.Map(request, result);
                _context.Events.Attach(eventEntity).State = EntityState.Modified;
            }
        }
    }
}
