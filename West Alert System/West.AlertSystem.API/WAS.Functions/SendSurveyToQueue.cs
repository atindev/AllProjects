using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WAS.Application.Common.Settings;
using Models = WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;
using System.Threading;
using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Functions
{
    public class SendSurveyToQueue
    {
        private readonly IWasContextAdmin _context;
        private readonly IAzureStorageService _azureStorageService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public SendSurveyToQueue(
            IWasContextAdmin context,
            IAzureStorageService azureStorageService,
            IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _context = context;
            _azureStorageService = azureStorageService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        [FunctionName("SendSurveyToQueue")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            log.LogInformation($"SendSurveyToQueue C# Timer trigger function executed at: {DateTime.UtcNow}");
            try
            {
                var surveys = await _context.SurveyBroadcasts
                    .Include(e => e.SurveyBroadcastEmail)
                    .Include(ng => ng.SurveyBroadcastGroups)
                    .Include(ns => ns.SurveyBroadcastSubscriptions)
                    .Include(ns => ns.SurveyBroadcastDistributionGroups)
                    .Include(ns => ns.SurveyBroadcastADUsers)
                    .IgnoreQueryFilters()
                    .Where(s => (s.Status != Domain.Enum.SurveyStatus.Sent)
                      && (s.StartTime <= DateTime.UtcNow)
                      && s.IsActive)
                    .ToListAsync(cancellationToken);

                if (surveys != null && surveys.Any())
                    await SendSurveyAsync(surveys, cancellationToken);

                //sending followup
                var surveyFollowup = await _context.SurveyBroadcastFollowups
                    .Include(x => x.SurveyBroadcast)
                      .ThenInclude(e => e.SurveyBroadcastEmail)
                    .Include(x => x.SurveyBroadcast)
                      .ThenInclude(e => e.SurveyBroadcastGroups)
                    .Include(x => x.SurveyBroadcast)
                      .ThenInclude(e => e.SurveyBroadcastSubscriptions)
                     .Include(x => x.SurveyBroadcast)
                      .ThenInclude(ns => ns.SurveyBroadcastDistributionGroups)
                     .Include(x => x.SurveyBroadcast)
                       .ThenInclude(ns => ns.SurveyBroadcastADUsers)

                    .IgnoreQueryFilters()
                    .Where(s => (s.Status == Domain.Enum.SurveyStatus.Submitted || s.Status == Domain.Enum.SurveyStatus.Failed)
                      && (s.FollowUpDate <= DateTime.UtcNow)
                      && s.IsActive)
                    .ToListAsync(cancellationToken);

                if (surveyFollowup != null && surveyFollowup.Any())
                    await SendSurveyFollowupAsync(surveyFollowup, cancellationToken);

                log.LogInformation($"SendSurveyToQueue C# Timer trigger function Completed at : {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }


        private async Task SendSurveyAsync(List<Domain.Entities.SurveyBroadcast> surveys, CancellationToken cancellationToken)
        {
            foreach (SurveyBroadcast survey in surveys)
            {
                if (survey.SurveyBroadcastEmail != null && survey.SurveyBroadcastEmail.IsActive)
                {
                    await _azureStorageService.AddMessageToStorageQueue(new Models.EmailNotification
                    {
                        Id = survey.Id,
                        GroupIds = survey.SurveyBroadcastGroups.Select(g => g.GroupId).ToList(),
                        SubscriptionIds = survey.SurveyBroadcastSubscriptions.Select(s => s.SubscriptionId).ToList(),
                        DistributionGroups = survey.SurveyBroadcastDistributionGroups.Select(s => new DistributionGroup { Id = s.DistributionGroupId, SurveyBroadcastDistributionGroupId = s.Id, CreatedBy = s.CreatedBy }).ToList(),
                        ADPeople = survey.SurveyBroadcastADUsers.Select(s => new ADPeople() { FirstName = s.FirstName, EmailId = s.EmailId }).ToList(),
                        EmailSendGridTemplateID = _azureStorageSettings.EmailSendGridTemplateID
                    }, _azureStorageSettings.EmailSurveyQueue);

                    survey.SurveyBroadcastEmail.IsActive = false;
                    await _context.SaveChangesAsync(cancellationToken);
                }
                survey.Status = Domain.Enum.SurveyStatus.Sent;
                survey.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        private async Task SendSurveyFollowupAsync(List<Domain.Entities.SurveyBroadcastFollowup> surveys, CancellationToken cancellationToken)
        {
            foreach (SurveyBroadcastFollowup survey in surveys)
            {
                await _azureStorageService.AddMessageToStorageQueue(new Models.EmailNotification
                {
                    Id = survey.Id,
                    GroupIds = survey.SurveyBroadcast.SurveyBroadcastGroups.Select(g => g.GroupId).ToList(),
                    SubscriptionIds = survey.SurveyBroadcast.SurveyBroadcastSubscriptions.Select(s => s.SubscriptionId).ToList(),
                    DistributionGroups = survey.SurveyBroadcast.SurveyBroadcastDistributionGroups.Select(s => new DistributionGroup { Id = s.DistributionGroupId, SurveyBroadcastDistributionGroupId = s.Id, CreatedBy = s.CreatedBy }).ToList(),
                    ADPeople = survey.SurveyBroadcast.SurveyBroadcastADUsers.Select(s => new ADPeople() { FirstName = s.FirstName, EmailId = s.EmailId }).ToList(),
                    EmailSendGridTemplateID = _azureStorageSettings.EmailSendGridTemplateID,
                    IsFollowUpEmail = true
                }, _azureStorageSettings.EmailSurveyQueue);

                survey.Status = Domain.Enum.SurveyStatus.Sent;
                survey.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
