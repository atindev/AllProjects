using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using EventGetCount = WAS.Application.Features.Events.GetCount;
using NotificationGetCount = WAS.Application.Features.Notification.GetCount;
using GroupGetCount = WAS.Application.Features.Group.GetCount;
using PeopleGetCount = WAS.Application.Features.Subscription.GetCount;
using recentNotification = WAS.Application.Features.Notification.GetAll;
using incomingMessage = WAS.Application.Features.IncomingMessage.GetAll;
using AutoMapper;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;

namespace WAS.Application.Features.Survey.GetBroadcastView
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IWasContext _context;

        public Handler(
            ILogger<Handler> logger,
            IMediator mediator,
            IMapper mapper,
            IWasContext context
            )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _context = context;

        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            { 
                var groupRequest = _mapper.Map<Group.GetAll.Request>(request);
                var groups = await _mediator.Send(groupRequest);

                var subscriptionRequest = _mapper.Map<Subscription.GetAll.Request>(request);
                var subscriptions = await _mediator.Send(subscriptionRequest);

                var survey = await _context.Surveys
                      .SingleOrDefaultAsync(i => i.Id == request.SurveyId, cancellationToken);

                if (survey == null)
                    throw new NotFoundException($"survey not found with the id {request.SurveyId}");

                return new Response {
                    Subscriptions=subscriptions.Subscriptions,
                    Groups=groups.Groups,
                    SurveyName= survey.Subject,
                    SurveyId = survey.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
