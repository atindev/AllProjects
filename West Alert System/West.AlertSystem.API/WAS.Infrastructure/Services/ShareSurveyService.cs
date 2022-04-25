using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ObjectsComparer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class ShareSurveyService : IShareSurveyService
    {
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IWasContext _context;
        private readonly ILogger<ShareSurveyService> _logger;
        private readonly IEmailFormatService _emailFormatService;

        public ShareSurveyService(
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService,
            IWasContext context,
            ILogger<ShareSurveyService> logger,
            IEmailFormatService emailFormatService
            )
        {
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
            _context = context;
            _logger = logger;
        }

        public async Task ShareSurvey(ShareSurveyData shareSurvey)
        {
            try
            {
                var sharedPersonFirstName = _context.Subscriptions.Where(x => x.OfficialEmail == shareSurvey.CreatedBy).Select(x => x.FirstName).Single();
                var sharedPersonLastName = _context.Subscriptions.Where(x => x.OfficialEmail == shareSurvey.CreatedBy).Select(x => x.LastName).Single();
                var shredPersonName = sharedPersonFirstName + " " + sharedPersonLastName;
                var survey = _context.SurveyBroadcasts.IgnoreQueryFilters().FirstOrDefault(x => x.Id == shareSurvey.BroadcastId);
                var surveyName = "";
                if(survey!=null)
                {
                    surveyName = survey.Subject;
                }
                for (int i = 0; i < shareSurvey.PeopleMail.Count; i++)
                {
                    var emailBody = "";
                    emailBody = await _emailFormatService.FormatEmail(new
                    {
                        SharedPerson = shredPersonName,
                        EmployeeFirstName = _context.Subscriptions.Where(x => x.OfficialEmail == shareSurvey.CreatedBy).Select(x => x.FirstName).Single(),
                        SurveyName = surveyName,
                    }, _azureStorageSettings.ShareSurveyTemplate);

                    await _azureStorageService.AddMessageToStorageQueue(new
                    {
                        Subject = "WAS | Shared Survey Alert",
                        MailBody = emailBody,
                        To = shareSurvey.PeopleMail[i],
                    }, _azureStorageSettings.SendEmailSurveyQueue);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
