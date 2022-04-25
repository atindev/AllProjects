using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.IncomingMessage.Add
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly IWasContextAdmin _wasContextAdmin;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IAlertAdminService _alertAdminService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IHttpClientFactory clientFactory,
            IAzureStorageService azureStorageService,
            IAlertAdminService alertAdminService,
            IWasContextAdmin wasContextAdmin
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _httpClient = clientFactory != null ? clientFactory.CreateClient("RecordingHttpClient") : throw new ArgumentNullException(nameof(clientFactory));
            _azureStorageService = azureStorageService;
            _alertAdminService = alertAdminService;
            _wasContextAdmin = wasContextAdmin;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
           try
            {
                var twilioVoiceUrl = "";
                if (request.IsVoice != null)
                {
                    var recordByteArray = await _httpClient.GetByteArrayAsync(request.Message);
                    twilioVoiceUrl = request.Message;
                    var blobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + ".wav";
                    var result = await _azureStorageService.UploadTwilioRecordingsToBlobStorage(
                                                    blobFileName,
                                                    recordByteArray);
                    request.Message = result.AbsoluteUri;
                }
                else if(request.Message.ToUpper().StartsWith("IM-"))
                {
                    request.Message = request.Message[3..];
                }

                var incomingMessage = _mapper.Map<Domain.Entities.IncomingMessage>(request);

                incomingMessage.TwilioVoiceMailUrl = request.IsVoice != null ? twilioVoiceUrl : null;

                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.OfficeMobile.Equals(incomingMessage.FromPhone)
                                                                                   || s.OfficePhone.Equals(incomingMessage.FromPhone)
                                                                                   || s.PersonalMobile.Equals(incomingMessage.FromPhone)
                                                                                   || s.HomePhone.Equals(incomingMessage.FromPhone), cancellationToken);
                if (subscription != null)
                {
                    incomingMessage.SubscriberEmail = subscription.OfficialEmail;

                    if (request.Message.ToUpper().StartsWith("NR-"))
                    {
                        var notificationId = await GetNotificationId(request, subscription.Id, cancellationToken);
                        incomingMessage.NotificationId = notificationId;
                        incomingMessage.Message = request.Message[3..];
                    }
                    
                }
                
                await _context.IncomingMessages.AddAsync(incomingMessage, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await AlertAdmin(request, subscription, incomingMessage);

                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

        private async Task<Guid?> GetNotificationId(Request request, Guid subscriptionId, CancellationToken cancellationToken)
        {
            if (request.FromPhone.Contains("whatsapp:"))
            {
                var notificationWhatsApp = await _context.DeliveryReportWhatsApps
                                        .Include(i => i.NotificationWhatsApp)
                                        .OrderByDescending(i => i.CreatedDate)
                                        .IgnoreQueryFilters()
                                        .FirstOrDefaultAsync(dr => (dr.Status.Equals("delivered") || dr.Status.Equals("read")) && dr.SubscriptionId.Equals(subscriptionId), cancellationToken);
                return notificationWhatsApp?.NotificationWhatsApp.NotificationId;
            }
            else
            {
                var notificationText = await _context.DeliveryReportTexts
                                        .Include(i => i.NotificationText)
                                        .OrderByDescending(i => i.CreatedDate)
                                        .IgnoreQueryFilters()
                                        .FirstOrDefaultAsync(dr => dr.Status.Equals("delivered") && dr.SubscriptionId.Equals(subscriptionId), cancellationToken);
                return notificationText?.NotificationText.NotificationId;
            }
        }

        private async Task AlertAdmin(Request request,Domain.Entities.Subscription subscription, Domain.Entities.IncomingMessage incomingMessage)
        {
            if (subscription != null && !request.Message.ToUpper().StartsWith("NR-"))
            {
                    var adminSubscriptions = _wasContextAdmin.Subscriptions.Include(x => x.Location).Where(x => x.LocationId == subscription.LocationId && x.Role == "WASAdmin").ToList();
                    AlertAdmin alertAdmin = new AlertAdmin()
                    {
                        AdminSubscription = adminSubscriptions,
                        IncomingMessage = incomingMessage,
                        SenderFullName = subscription.FirstName + " " + subscription.LastName
                    };
                    await _alertAdminService.SendIncomingMessage(alertAdmin);
            }
        }
    }
}
