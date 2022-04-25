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
using WAS.Application.Common.Models;
using WAS.Application.Interface;

namespace WAS.Application.Features.Report.GetAllFeedbackChannelCount
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
                List<Domain.Entities.SubscriptionFeedback> feedbackChannel = new List<Domain.Entities.SubscriptionFeedback>();
                if (request.LocationId != 0)
                {
                     feedbackChannel = await _context.SubscriptionFeedbacks.Include(x => x.Subscription).Where(x => x.Subscription.LocationId == request.LocationId).IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                else
                {
                    feedbackChannel = await _context.SubscriptionFeedbacks.IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                var allfeedbackChannel = feedbackChannel.Select(x => x.FeedbackChannel).Distinct();
                var subscriptionfeedback = await _context.SubscriptionFeedbacks.IgnoreQueryFilters().ToListAsync(cancellationToken);
                List<FeedbackChannelCount> feedbackChannelCounts = new List<FeedbackChannelCount>();
                
                var values = Enum.GetValues(typeof(WAS.Domain.Enum.Feedback));
                foreach (var item in allfeedbackChannel)
                {
                    List<FeedbackRatings> feedbackRatings = new List<FeedbackRatings>();
                    foreach (var item1 in values)
                    {
                        FeedbackRatings differentfeedbackratings = new FeedbackRatings()
                        {
                            Count = subscriptionfeedback.Count(x=>x.FeedbackChannel == item &&  x.FeedbackRating == item1.ToString()),
                            Rating = item1.ToString()
                        };
                        feedbackRatings.Add(differentfeedbackratings);
                    }
                    FeedbackChannelCount feedbackChannelCount = new FeedbackChannelCount()
                    {
                        FeedbackChannel = item,
                        FeedbackRatings = feedbackRatings
                    };
                    feedbackChannelCounts.Add(feedbackChannelCount);
                }
                var responseFeedbackChannelCount = _mapper.Map<List<FeedbackChannelCount>>(feedbackChannelCounts);
                return new Response { FeedbackChannels = responseFeedbackChannelCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
