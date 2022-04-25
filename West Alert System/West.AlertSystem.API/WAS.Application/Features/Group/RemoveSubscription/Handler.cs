using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Group.RemoveSubscription
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IWasContextAdmin _wasContextAdmin;
        private readonly IGroupChangeNotificationService _groupChangeNotificationService;


        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IWasContextAdmin wasContextAdmin,
            IGroupChangeNotificationService groupChangeNotificationService
            )
        {
            _context = context;
            _logger = logger;
            _wasContextAdmin = wasContextAdmin;
            _groupChangeNotificationService = groupChangeNotificationService;
        }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
            try
            {
                for (int i = 0; i < request.SubscriptionGroupId.Count; i++)
                {
                    var result = await _context.SubscriptionGroups
                    .SingleOrDefaultAsync(b => b.Id == request.SubscriptionGroupId[i], cancellationToken);

                    var subscriptionData = _wasContextAdmin.Subscriptions.Include(x => x.Location).FirstOrDefault(x => x.Id == result.SubscriptionId);
                    
                    if (result != null)
                    {
                        result.ModifiedBy = request.ModifiedBy;
                        _context.SubscriptionGroups.Remove(result);
                        await SendRemovalFromGroupNotification(request, cancellationToken, subscriptionData);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                
                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
        public async Task SendRemovalFromGroupNotification(Request request, CancellationToken cancellationToken, Domain.Entities.Subscription subscriptionData)
        {

                var groupName = _context.Groups.Where(x => x.Id == request.GroupId).Select(x => x.Name).Single();
                var firstName = _context.Subscriptions.Where(x => x.OfficialEmail == request.ModifiedBy).Select(x => x.FirstName).Single();
                var lastName = _context.Subscriptions.Where(x => x.OfficialEmail == request.ModifiedBy).Select(x => x.LastName).Single();
                var adminName = firstName +" "+ lastName;
                await  SendNotification(subscriptionData, groupName, adminName);
            
        }

        public async Task SendNotification(Domain.Entities.Subscription subscriptions, string groupName, string adminName)
        {
            if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.OfficialEmail.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.OfficialEmail, "email", groupName, adminName);
            }
            else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.PersonalEmail.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.PersonalEmail, "email", groupName, adminName);
            }
            else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextOfficeMobile.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.OfficeMobile, "sms", groupName, adminName);
            }
            else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextPersonalMobile.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.PersonalMobile, "sms", groupName, adminName);
            }
            else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppOfficeMobile.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.OfficeMobile, "whatsapp", groupName, adminName);
            }
            else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppPersonalMobile.ToString())
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.PersonalMobile, "whatsapp", groupName, adminName);
            }
            else
            {
                await RemovalFromGroupNotification(subscriptions, subscriptions.OfficialEmail, "email", groupName, adminName);
            }

        }
        public async Task RemovalFromGroupNotification(Domain.Entities.Subscription subscriptionData, string addresData, string channel, string groupName, string AdminName)
        {
            try
            {
                var employeeFirstName = subscriptionData.FirstName;
                var NameOfGroup = groupName;
                var action = "remove";
                var NameOfAdmin = AdminName;
                var projectName = "West Alert System";
                var smsBody = $"Hello {employeeFirstName},\nYou are removed from the Group - {NameOfGroup} on West Alert System by {NameOfAdmin}.";
                var whatsappBody = $"Hello {employeeFirstName},\n\nYou are removed from the group - *{NameOfGroup}* on {projectName} by {NameOfAdmin}.";
                GroupChangeNotification groupChangeNotification = new GroupChangeNotification();
                groupChangeNotification.Subscription = subscriptionData;
                groupChangeNotification.Action = action;
                await _groupChangeNotificationService.SendGroupChangeMessage(groupChangeNotification, addresData, channel, groupName, AdminName, whatsappBody, smsBody);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
