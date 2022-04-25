using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Subscription.ConversationSubscription
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly ISubscriptionConfirmationService _subscriptionConfirmationService;

        public Handler(
            IWasContextAdmin context,
            ILogger<Handler> logger,
            IMapper mapper,
            ISubscriptionConfirmationService subscriptionConfirmationService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _subscriptionConfirmationService = subscriptionConfirmationService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await CreateSubscription(request, cancellationToken);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }

        private async Task<Response> CreateSubscription(Request request, CancellationToken cancellationToken)
        {
            var response = new Response();
            var subscriptionEntity = await _context.Subscriptions
                      .FirstOrDefaultAsync(o => o.Upn == request.Upn, cancellationToken);
            var Location = await _context.Locations
                      .FirstOrDefaultAsync(o => o.Name.Equals(request.OfficeLocation), cancellationToken);
            var Department = await _context.Departments
                      .FirstOrDefaultAsync(o => o.Name.Equals(request.DepartmentName), cancellationToken);

            if (subscriptionEntity == null)
            {
                var subscription = _mapper.Map<Domain.Entities.Subscription>(request);
                subscription.PreferredLanguage ??= "en-US";
                subscription.LocationId = Location != null ? Location.Id : 0;
                subscription.DepartmentId = Department?.Id;
                await _context.Subscriptions.AddAsync(subscription, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await _subscriptionConfirmationService.SendSubscriptionConfirmation(subscription);
                response.ResponseMessage = "Save Successful";
                response.Success = true;
            }
            else
            {
                if (!request.SubscriptionMode.Contains("Web"))
                {
                    request = GetUpdateRequest(request, subscriptionEntity);
                }
                request.SubscriptionMode = subscriptionEntity.SubscriptionMode;
                _mapper.Map(request, subscriptionEntity);
                subscriptionEntity.PreferredLanguage ??= "en-US";
                subscriptionEntity.LocationId = Location != null ? Location.Id : 0;
                subscriptionEntity.DepartmentId = Department?.Id;
                
                await _context.SaveChangesAsync(cancellationToken);

                await _subscriptionConfirmationService.SendSubscriptionConfirmation(subscriptionEntity);

                response.ResponseMessage = "Update Successful";
                response.SubscriptionId = subscriptionEntity.Id;
                response.Success = true;
            }

            if (request.SubscriptionReviewId != null)
            {
                var ocrSubscriptionEntity = await _context.OcrSubscriptions
                .SingleOrDefaultAsync(o => o.Id == request.SubscriptionReviewId, cancellationToken);

                if (ocrSubscriptionEntity != null)
                {
                    ocrSubscriptionEntity.DeletedDate = DateTime.UtcNow;
                    ocrSubscriptionEntity.ModifiedDate = DateTime.UtcNow;
                    ocrSubscriptionEntity.IsActive = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            return response;
        }

        private Request GetUpdateRequest(Request request, Domain.Entities.Subscription subscriptionEntity)
        {
            if (request.OfficeMobile == null && subscriptionEntity.OfficeMobile != null)
            {
                request.OfficeMobile = subscriptionEntity.OfficeMobile;
                request.IsTextOfficeMobile = subscriptionEntity.IsTextOfficeMobile;
                request.IsVoiceOfficeMobile = subscriptionEntity.IsVoiceOfficeMobile;
                request.IsWhatsAppOfficeMobile = subscriptionEntity.IsWhatsAppOfficeMobile;
            }
            if (request.PersonalMobile == null && subscriptionEntity.PersonalMobile != null)
            {
                request.PersonalMobile = subscriptionEntity.PersonalMobile;
                request.IsTextPersonalMobile = subscriptionEntity.IsTextPersonalMobile;
                request.IsVoicePersonalMobile = subscriptionEntity.IsVoicePersonalMobile;
                request.IsWhatsAppPersonalMobile = subscriptionEntity.IsWhatsAppPersonalMobile;
            }
            request.PersonalEmail = subscriptionEntity.PersonalEmail;
            request.IsPersonalEmail = subscriptionEntity.IsPersonalEmail;
            request.HomePhone = subscriptionEntity.HomePhone;
            request.IsVoiceHomePhone = subscriptionEntity.IsVoiceHomePhone;

            return request;
        }
    }
}
