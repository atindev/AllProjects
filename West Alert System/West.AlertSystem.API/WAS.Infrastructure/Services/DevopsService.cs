using AutoMapper.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Features.Feedback;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class DevopsService : IDevopsService
    {
        private readonly IAzureStorageService _azureStorageService;
        private readonly AzureStorageSettings _azureStorageSettings;
        public DevopsService(
            IAzureStorageService azureStorageService,
           IOptions<AzureStorageSettings> azureStorageSettings
            )
        {
            _azureStorageService = azureStorageService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        public async Task<Response> SubmitUserFeedback(FeedbackResource feedbackResource)
        {
            try
            {
                
                await _azureStorageService.AddMessageToStorageQueue(feedbackResource, _azureStorageSettings.UserFeedbackQueue);

                return new Response()
                {
                    StatusCode = 1,
                    Message = $"We appreciate your feedback, Thank You!"
                };
            }
            catch (Exception ex)
            {
                return new Response()
                {
                    StatusCode = 0,
                    Message = ex.Message
                };
            }
        }
    }
}

