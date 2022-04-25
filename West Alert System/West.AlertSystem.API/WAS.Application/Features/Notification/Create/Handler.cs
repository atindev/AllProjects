using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Notification.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly IWasContextAdmin _wasContextAdmin;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IAzureStorageService _azureStorageService;
        private readonly INotificationApprovalAlert _notificationApprovalAlert;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IAzureStorageService azureStorageService,
            IWasContextAdmin wasContextAdmin,
            INotificationApprovalAlert notificationApprovalAlert
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _azureStorageService = azureStorageService;
            _notificationApprovalAlert = notificationApprovalAlert;
            _wasContextAdmin = wasContextAdmin;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var notificationType = await _context.NotificationTypes
                .FirstOrDefaultAsync(i => i.Name.ToUpper() == request.Name.ToUpper(), cancellationToken);

                if (notificationType != null)
                    request.NotificationTypeId = notificationType.Id;

                var notification = _mapper.Map<Domain.Entities.Notification>(request);
                if(notification.IsPrivateNotification && (notification.IsApprovalRequired?? false))
                    notification.Status = Domain.Enum.Status.FirstLevelApproved;

                await _context.Notifications.AddAsync(notification, cancellationToken);

                var notificationGroups = request.GroupId.Select(gid => new Domain.Entities.NotificationGroup { GroupId = gid, NotificationId = notification.Id });
                var notificationSubscriptions = request.SubscriptionId.Select(sid => new Domain.Entities.NotificationSubscription { SubscriptionId = sid, NotificationId = notification.Id });
                await _context.NotificationGroups.AddRangeAsync(notificationGroups, cancellationToken);
                await _context.NotificationSubscriptions.AddRangeAsync(notificationSubscriptions, cancellationToken);

                if (request.IsText)
                {
                    var notificationText = _mapper.Map<Domain.Entities.NotificationText>(request);
                    notificationText.NotificationId = notification.Id;

                    await _context.NotificationTexts.AddAsync(notificationText, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                if (request.IsVoice)
                {
                    var notificationVoice = _mapper.Map<Domain.Entities.NotificationVoice>(request);
                    notificationVoice.NotificationId = notification.Id;

                    await _context.NotificationVoices.AddAsync(notificationVoice, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                if (request.IsEmail)
                {
                    var notificationEmail = _mapper.Map<Domain.Entities.NotificationEmail>(request);
                    notificationEmail.NotificationId = notification.Id;

                    await _context.NotificationEmails.AddAsync(notificationEmail, cancellationToken);

                    List<Domain.Entities.NotificationEmailAttachment> notificationEmailAttachments = new List<Domain.Entities.NotificationEmailAttachment>();

                    notificationEmailAttachments = await getEmailAttachmentsAsync(notificationEmail.Id, request.EmailAttachments, request.ExistingEmailAttachments);

                    if (notificationEmailAttachments.Any())
                    {
                        await _context.NotificationEmailAttachments.AddRangeAsync(notificationEmailAttachments, cancellationToken);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }

                if (request.IsWhatsApp)
                {
                    var notificationWhatsApp = _mapper.Map<Domain.Entities.NotificationWhatsApp>(request);
                    notificationWhatsApp.NotificationId = notification.Id;

                    await _context.NotificationWhatsApps.AddAsync(notificationWhatsApp, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                if (request.ApproverForPrivate != null && request.IsApprovalRequired && request.IsPrivateNotification)
                {
                 var approverSubscription = _wasContextAdmin.Subscriptions.Where(x => x.Upn == request.ApproverForPrivate).ToList();
                    var senderFirstName = _wasContextAdmin.Subscriptions.FirstOrDefault(x => x.Upn == request.CreatedBy).FirstName;
                    var senderLastName = _wasContextAdmin.Subscriptions.FirstOrDefault(x => x.Upn == request.CreatedBy).LastName;
                    var senderName = senderFirstName +" "+senderLastName;
                    NotificationApproval notificationApproval = new NotificationApproval()
                    {
                        SenderFullName = senderName,
                        ApproverSubscription = approverSubscription
                    };
                 await _notificationApprovalAlert.NotificationApproval(notificationApproval);
                }
                if(request.IsApprovalRequired && !request.IsPrivateNotification)
                {   
                    var senderLocation = _wasContextAdmin.Subscriptions.Include(x=>x.Location).FirstOrDefault(x => x.Upn == request.CreatedBy).Location;
                    var approverSubscription = _wasContextAdmin.Subscriptions.Where(x => x.Role == "Approver" && x.Location == senderLocation).ToList();
                    var senderFirstName = _wasContextAdmin.Subscriptions.FirstOrDefault(x => x.Upn == request.CreatedBy).FirstName;
                    var senderLastName = _wasContextAdmin.Subscriptions.FirstOrDefault(x => x.Upn == request.CreatedBy).LastName;
                    var senderName = senderFirstName +" "+senderLastName;
                        NotificationApproval notificationApproval = new NotificationApproval()
                        {
                            SenderFullName = senderName,
                            ApproverSubscription = approverSubscription
                        };
                        await _notificationApprovalAlert.NotificationApproval(notificationApproval);
                }
                return new Response { Success = true, Id = notification.Id };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }


        private async Task<List<Domain.Entities.NotificationEmailAttachment>> getEmailAttachmentsAsync(Guid NotificationEmailId, List<AttachmentData> EmailAttachments, List<string> ExistingEmailAttachments)
        {
            List<Domain.Entities.NotificationEmailAttachment> notificationEmailAttachments = new List<Domain.Entities.NotificationEmailAttachment>();

            if (EmailAttachments.Any() || ExistingEmailAttachments.Any())
            {
                foreach (AttachmentData emailAttachment in EmailAttachments)
                {
                    var eaByteArray = Convert.FromBase64String(emailAttachment.Content);
                    var blobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + Path.GetExtension(emailAttachment.FileName);

                    var result = await _azureStorageService.UploadFileToBlobStorage(
                                                    blobFileName,
                                                    eaByteArray);
                    notificationEmailAttachments.Add(new Domain.Entities.NotificationEmailAttachment
                    {
                        NotificationEmailId = NotificationEmailId,
                        FileName = emailAttachment.FileName,
                        Attachment = result.AbsoluteUri,
                        ContentType = emailAttachment.ContentType
                    });
                }

                //if any attachment from template
                if (ExistingEmailAttachments.Any())
                {
                    foreach (var attachmentURL in ExistingEmailAttachments)
                    {
                        var content = attachmentURL.Split("|");
                        if (content.Length > 2)
                        {
                            notificationEmailAttachments.Add(new Domain.Entities.NotificationEmailAttachment
                            {
                                NotificationEmailId = NotificationEmailId,
                                Attachment = content[0],
                                FileName = content[1],
                                ContentType = content[2]
                            });

                        }
                    }

                }

            }

            return notificationEmailAttachments;

        }


    }
}
