using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;

namespace WAS.Application.Features.Report.GetAllNotifiactionPerMonth
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
                List<Domain.Entities.Notification> notificationData =new List<Domain.Entities.Notification>();
                if(request.LocationId != 0)
                {
                    var officeEmail = await _context.Subscriptions.IgnoreQueryFilters().Where(x => x.LocationId == request.LocationId && x.IsActive).Select(x=>x.OfficialEmail).ToListAsync(cancellationToken);
                    foreach (var item in officeEmail)
                    {
                        var notification = await _context.Notifications.IgnoreQueryFilters().Where(x => x.CreatedBy == item).ToListAsync(cancellationToken);
                        if (notification.Any())
                        {
                             notificationData.AddRange(notification);
                        }
                    }
                }
                else
                {
                    notificationData = await _context.Notifications.IgnoreQueryFilters().ToListAsync(cancellationToken);
                }
                    var NotifiactionPerMonth = notificationData.Select(x => x.CreatedDate.Month);
                var NotifiactionPerMonthdictionary = NotifiactionPerMonth.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                List<AllNotificationPerMonth> NotificationPerMonths = new List<AllNotificationPerMonth>();


                foreach (var item in NotifiactionPerMonthdictionary)
                {
                   
                    AllNotificationPerMonth allNotifications = new AllNotificationPerMonth()
                    {

                        NotifiactionModeSum = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key),
                        Count = item.Value
                    };

                    NotificationPerMonths.Add(allNotifications);
                }
                var responseNotificationPerMonth = _mapper.Map<List<AllNotificationPerMonth>>(NotificationPerMonths);
                return new Response { NotificationPerMonths = responseNotificationPerMonth };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
