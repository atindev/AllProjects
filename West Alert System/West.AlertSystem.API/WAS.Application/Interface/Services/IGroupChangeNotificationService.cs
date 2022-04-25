using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface IGroupChangeNotificationService
    {
        Task SendGroupChangeMessage(GroupChangeNotification groupChangeNotification, string addresData, string channel, string groupName, string AdminName, string whatsappBody, string smsBody);
    }
}
