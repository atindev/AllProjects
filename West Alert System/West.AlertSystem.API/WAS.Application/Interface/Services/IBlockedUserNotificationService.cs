using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface IBlockedUserNotificationService
    {
        Task SendBlockedUserNotification(string officialEmail, string name,string attemptON);
    }
}
