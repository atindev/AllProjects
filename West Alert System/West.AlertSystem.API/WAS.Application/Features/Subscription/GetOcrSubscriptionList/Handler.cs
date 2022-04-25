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
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using Locations = WAS.Application.Features.Location.GetAll;

namespace WAS.Application.Features.Subscription.GetOcrSubscriptionList
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGraphService _graphService;

        public Handler(
            IWasContextAdmin context,
            ILogger<Handler> logger,
            IMapper mapper,
            IMediator mediator,
            IGraphService graphService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _graphService = graphService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var locations = await _mediator.Send(new Locations.Request());
                locations.Locations.Add(new Locations.Location { Id = 0, Name = "Unknown" });
                if (request.LocationId == null)
                {
                    var subscription = await _context.Subscriptions.SingleOrDefaultAsync(s => s.OfficialEmail == request.AdminOfficialEmail, cancellationToken);
                    if(subscription != null)
                    {
                        request.LocationId = subscription.LocationId;
                    }
                    else
                    {
                        var userResponse = await _graphService.GetUser(request.AdminOfficialEmail);
                        request.LocationId = locations.Locations.Where(l => l.Name == userResponse.OfficeLocation).Select(l => l.Id).FirstOrDefault();
                    }
                }

                var ocrSubscription = await _context.OcrSubscriptions
                        .OrderByDescending(i => i.CreatedDate)
                        .ToListAsync(cancellationToken);

                var ocrSubscriptionList = _mapper.Map<List<Common.Models.OcrSubscription>>(ocrSubscription);
                ocrSubscriptionList.RemoveAll(o => o.LocationId != request.LocationId);

                return new Response{ OcrSubscriptionList = ocrSubscriptionList, Locations = locations.Locations, AdminLocationId = (int) request.LocationId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
    }

}
