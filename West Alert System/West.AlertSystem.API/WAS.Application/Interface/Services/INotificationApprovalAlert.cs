using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface INotificationApprovalAlert
    {
        Task NotificationApproval(NotificationApproval notificationApproval);
        Task SendNotificationApproval(Domain.Entities.Subscription subscription, string addresData, string channel, string senderName);
    }
}
