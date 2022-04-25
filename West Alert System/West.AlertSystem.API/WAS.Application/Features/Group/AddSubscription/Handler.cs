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

namespace WAS.Application.Features.Group.AddSubscription
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
                List<Domain.Entities.SubscriptionGroup> subscriptionGroup = new List<Domain.Entities.SubscriptionGroup>();
                var existingSubscritionGroup = new WAS.Domain.Entities.SubscriptionGroup();

                for (int i = 0; i < request.GroupId.Count; i++)
                {
                    request.SubscriptionId.ForEach(subscription =>
                    {

                        existingSubscritionGroup = _context.SubscriptionGroups
                                                   .FirstOrDefault((item => (item.GroupId == request.GroupId[i] && item.SubscriptionId == subscription)));
                        if (existingSubscritionGroup == null)
                        {
                            subscriptionGroup.Add(new Domain.Entities.SubscriptionGroup
                            {
                                GroupId = request.GroupId[i],
                                SubscriptionId = subscription,
                                CreatedBy = request.CreatedBy,
                                ModifiedBy = request.CreatedBy,
                                ModifiedDate = DateTime.UtcNow
                            });

                        }

                    });
                }
                 
                await _context.SubscriptionGroups.AddRangeAsync(subscriptionGroup,cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                SendAdditionToGroupNotification(request);
                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
        public void SendAdditionToGroupNotification(Request request)
        {

            var subscriptionData = new Domain.Entities.Subscription();

            for (int i = 0; i < request.GroupId.Count; i++)
            {
                request.SubscriptionId.ForEach(async subscription =>
                {
                    subscriptionData = _wasContextAdmin.Subscriptions.Include(x => x.Location).Where(item => item.Id == subscription).FirstOrDefault();
                    var groupName = _context.Groups.Where(x => x.Id == request.GroupId[i]).Select(x => x.Name).FirstOrDefault();
                    var firstName = _context.Subscriptions.Where(x => x.OfficialEmail == request.CreatedBy).Select(x => x.FirstName).Single();
                    var lastName = _context.Subscriptions.Where(x => x.OfficialEmail == request.CreatedBy).Select(x => x.LastName).Single();
                    var adminName = firstName + " " + lastName;
                    await SendNotification(subscriptionData, groupName, adminName);
                });
            }

        }

        public async Task SendNotification(Domain.Entities.Subscription subscriptions,string groupName,string AdminName)
        {
                if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.OfficialEmail.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.OfficialEmail, "email", groupName, AdminName);
                }
                else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.PersonalEmail.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.PersonalEmail, "email", groupName, AdminName);
                }
                else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextOfficeMobile.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.OfficeMobile, "sms",groupName, AdminName);
                }   
                else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.TextPersonalMobile.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.PersonalMobile, "sms", groupName, AdminName);
                }
                else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppOfficeMobile.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.OfficeMobile, "whatsapp",groupName, AdminName);
                }
                else if (subscriptions.PreferredChannel == WAS.Application.Common.Enum.PreferredChannel.WhatsAppPersonalMobile.ToString())
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.PersonalMobile, "whatsapp",groupName, AdminName);
                }
                else
                {
                    await AdditionToGroupNotification(subscriptions, subscriptions.OfficialEmail, "email",groupName, AdminName);
                }
            
        }
        public async Task AdditionToGroupNotification(Domain.Entities.Subscription subscriptionData,string addresData,string channel,string groupName,string AdminName)
        {
            try
            {
                var employeeFirstName = subscriptionData.FirstName;
                var action = "add";
                var NameOfGroup = groupName;
                var NameOfAdmin = AdminName;
                var projectName = "West Alert System";
                var smsBody = $"Hello {employeeFirstName},\nYou have been added to Group - {NameOfGroup} on West Alert System by {NameOfAdmin}. You will be part of communication when this group is included in the targeted audience.";
                var whatsappBody = $"Hello {employeeFirstName},\n\nYou have been added to group - *{NameOfGroup}* on {projectName} by {NameOfAdmin}. You will be part of communication when this group is included in the targeted audience.";
                GroupChangeNotification groupChangeNotification = new GroupChangeNotification();
                groupChangeNotification.Subscription = subscriptionData;
                groupChangeNotification.Action = action;
                await  _groupChangeNotificationService.SendGroupChangeMessage(groupChangeNotification,addresData, channel, groupName, AdminName, whatsappBody, smsBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
