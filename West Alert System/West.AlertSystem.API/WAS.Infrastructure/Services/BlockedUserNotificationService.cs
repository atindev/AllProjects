using Microsoft.Extensions.Options;
using ObjectsComparer;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class BlockedUserNotificationService : IBlockedUserNotificationService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;

        public BlockedUserNotificationService(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
        }

        public async Task SendBlockedUserNotification(string officialEmail,string name, string attemptON)
        {
            await _azureStorageService.AddMessageToStorageQueue(new Application.Common.Models.SubscriptionConfirmation
            {
                Name = name,
                To = officialEmail,
                Channel = "email",
                AttemptON=attemptON
            }, _azureStorageSettings.BlockedUserNotificationQueue);
        }
    }
}
