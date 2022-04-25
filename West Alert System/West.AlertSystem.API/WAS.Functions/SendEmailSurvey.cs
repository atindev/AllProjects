using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using getADUsers = WAS.Application.Features.Survey.GetUniqueADPeople;
using getAudience = WAS.Application.Features.Survey.GetUniqueSubscribers;
using submitted = WAS.Application.Features.Survey.GetAllSubmitted;

namespace WAS.Functions
{
    public class SendEmailSurvey
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<SendEmailSurvey> _logger;
        private readonly IMediator _mediator;
        private readonly IAzureStorageService _azureStorageService;
        private readonly IEmailFormatService _emailFormatService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public SendEmailSurvey(
            IWasContextAdmin context,
            ILogger<SendEmailSurvey> logger,
            IMediator mediator,
            IAzureStorageService azureStorageService,
            IEmailFormatService emailFormatService,
            IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _azureStorageService = azureStorageService;
            _emailFormatService = emailFormatService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        [FunctionName("SendEmailSurvey")]
        public async Task Run([QueueTrigger("was-email-survey", Connection = "AzureWebJobsStorage")] EmailNotification emailNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation($"C# Queue trigger SendEmailSurvey function started: {emailNotification.Id}");

                if (!emailNotification.IsFollowUpEmail)
                    await SendSurveyEmail(emailNotification, log, cancellationToken);
                else
                    await SendSurveyEmailFollowUp(emailNotification, log, cancellationToken);

                log.LogInformation($"C# Queue trigger SendEmailSurvey function processed: {emailNotification.Id}");
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private async Task SendSurveyEmail(EmailNotification emailNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var survey = await _context.SurveyBroadcasts
                      .Include(o => o.SurveyBroadcastEmail)
                      .Include(x => x.Survey)
                      .IgnoreQueryFilters()
                      .SingleOrDefaultAsync(s => s.Id == emailNotification.Id);

                var groupDetails = await _mediator.Send(new getAudience.Request { Ids = emailNotification.GroupIds, SubscriptionIds = emailNotification.SubscriptionIds });

                getADUsers.Response adUserDetails = new getADUsers.Response();
                var uniqueSubscribers = groupDetails.Audience.Select(s => s.SubscriberOfficialEmail).ToList();
                if ((emailNotification.DistributionGroups != null && emailNotification.DistributionGroups.Any()) || (emailNotification.ADPeople != null && emailNotification.ADPeople.Any()))
                    adUserDetails = await _mediator.Send(new getADUsers.Request { SurveyBroadcastId = emailNotification.Id, UniqueSubscribers = uniqueSubscribers, DistributionGroups = emailNotification.DistributionGroups, ADPeople = emailNotification.ADPeople, ShouldSaveToDB = true });

                var totalAudienceCount = uniqueSubscribers.Count + (adUserDetails.ADUser?.Count ?? 0);

                if (survey != null && survey.SurveyBroadcastEmail != null)
                {
                    await SendEmail(
                            groupDetails,
                            adUserDetails,
                            survey.Id,
                            survey.Survey.Subject,
                            survey.Survey.Description,
                            survey.EndTime,
                            false
                            );

                    survey.TotalAudienceCount = totalAudienceCount;
                    survey.SurveyBroadcastEmail.SentDate = DateTime.UtcNow;
                    survey.SentDate = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }
        }

        private async Task SendSurveyEmailFollowUp(EmailNotification emailNotification, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                var survey = await _context.SurveyBroadcastFollowups
                      .Include(o => o.SurveyBroadcast)
                         .ThenInclude(x => x.Survey)
                      .IgnoreQueryFilters()
                      .SingleOrDefaultAsync(s => s.Id == emailNotification.Id);

                var groupDetails = await _mediator.Send(new getAudience.Request { Ids = emailNotification.GroupIds, SubscriptionIds = emailNotification.SubscriptionIds });

                getADUsers.Response adUserDetails = new getADUsers.Response();
                var uniqueSubscribers = groupDetails.Audience.Select(s => s.SubscriberOfficialEmail).ToList();
                if ((emailNotification.DistributionGroups != null && emailNotification.DistributionGroups.Any()) || (emailNotification.ADPeople != null && emailNotification.ADPeople.Any()))
                    adUserDetails = await _mediator.Send(new getADUsers.Request { SurveyBroadcastId = emailNotification.Id, UniqueSubscribers = uniqueSubscribers, DistributionGroups = emailNotification.DistributionGroups, ADPeople = emailNotification.ADPeople, ShouldSaveToDB = false });

                await SendEmail(
                        groupDetails,
                        adUserDetails,
                        survey.SurveyBroadcast.Id,
                        (survey.SurveyBroadcast.Survey.Subject + " : Follow-up"),
                        survey.SurveyBroadcast.Survey.Description,
                        survey.SurveyBroadcast.EndTime,
                        true
                        );
                survey.SentDate = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
            }

        }


        private async Task SendEmail(
                        getAudience.Response groupDetails,
                        getADUsers.Response adUserDetails,
                        Guid surveyBroadcastId,
                        string surveyName,
                        string description,
                        DateTime surveyEndTime,
                        bool IsFollowUpEmail
                        )
        {
            //getting survey submitted list
            var submittedAudience = await _mediator.Send(new submitted.Request { Id = surveyBroadcastId, IsOnlySubscriberIds = true });

            //If work email is enabled for notification
            Parallel.ForEach(groupDetails?.Audience.Where(sg => sg.IsOfficialEmail), async item =>
            {
                try
                {
                    if (!IsFollowUpEmail || (IsFollowUpEmail && (!submittedAudience.Answers.Select(x => x.Email).Any(i => i == item.SubscriberOfficialEmail))))
                    {
                        var emailBody = await getEmailBoday(item.SubscriberFirstName, surveyBroadcastId, IsFollowUpEmail, surveyName, description, surveyEndTime);
                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = surveyName,
                            MailBody = emailBody,
                            To = item.SubscriberOfficialEmail,
                        }, _azureStorageSettings.SendEmailSurveyQueue);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            });

            //If personal email is enabled for notification
            Parallel.ForEach(groupDetails?.Audience.Where(sg => sg.IsPersonalEmail), async item =>
            {
                try
                {
                    if (!IsFollowUpEmail || (IsFollowUpEmail && (!submittedAudience.Answers.Select(x => x.Email).Any(i => i == item.SubscriberOfficialEmail))))
                    {
                        var emailBody = await getEmailBoday(item.SubscriberFirstName, surveyBroadcastId, IsFollowUpEmail, surveyName, description, surveyEndTime);
                        await _azureStorageService.AddMessageToStorageQueue(new
                        {
                            Subject = surveyName,
                            MailBody = emailBody,
                            To = item.SubscriberPersonalEmail,
                        }, _azureStorageSettings.SendEmailSurveyQueue);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
            });

            //AD People
            if (adUserDetails?.ADUser != null)
            {
                SendEmailToAdUsers(
                       adUserDetails,
                       surveyBroadcastId,
                       surveyName,
                       description,
                       surveyEndTime,
                       IsFollowUpEmail,
                       submittedAudience
                       );
            }
        }

        private void SendEmailToAdUsers(
                     getADUsers.Response adUserDetails,
                     Guid surveyBroadcastId,
                     string surveyName,
                     string description,
                     DateTime surveyEndTime,
                     bool IsFollowUpEmail,
                     submitted.Response submittedAudience
                     )
        {
            Parallel.ForEach(adUserDetails.ADUser, async item =>
                {
                    try
                    {
                        if (!IsFollowUpEmail || (IsFollowUpEmail && (!submittedAudience.Answers.Select(x => x.Email).Any(i => i == item.EmailId))))
                        {
                            var emailBody = await getEmailBoday(item.FirstName, surveyBroadcastId, IsFollowUpEmail, surveyName, description, surveyEndTime);
                            await _azureStorageService.AddMessageToStorageQueue(new
                            {
                                Subject = surveyName,
                                MailBody = emailBody,
                                To = item.EmailId,
                            }, _azureStorageSettings.SendEmailSurveyQueue);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                });
        }

        private async Task<string> getEmailBoday(string SubscriberFirstName, Guid surveyBroadcastId, bool IsFollowUpEmail, string surveyName, string description, DateTime surveyEndTime)
        {
            var wasWebBaseUrl = Environment.GetEnvironmentVariable("WasUrl");
            var signature = "<b>Admin</b> from <b>West Alert System</b>";
            var emailBody = "";
            if (!IsFollowUpEmail)
            {
                emailBody = await _emailFormatService.FormatEmail(new
                {
                    EmployeeName = SubscriberFirstName,
                    SurveyMessage = "Please take a moment to fill out the survey below",
                    SurveyTitle = surveyName,
                    SurveyDescription = description ?? "",
                    SurveyLink = $"{wasWebBaseUrl}/Survey/EmailSurveyLanding?id={surveyBroadcastId}",
                    SurveyEndTime = surveyEndTime.ToString("MMM dd, yyyy HH:mm:ss tt"),
                    Signature = signature
                }, _azureStorageSettings.SurveyWorkEmailTemplate);
            }
            else
            {
                emailBody = await _emailFormatService.FormatEmail(new
                {
                    EmployeeName = SubscriberFirstName,
                    SurveyMessage = "We recently sent you a survey invite which is awaiting your reply",
                    SurveyTitle = surveyName,
                    SurveyDescription = description ?? "",
                    SurveyLink = $"{wasWebBaseUrl}/Survey/EmailSurveyLanding?id={surveyBroadcastId}",
                    SurveyEndTime = surveyEndTime.ToString("MMM dd, yyyy HH:mm:ss tt"),
                    Signature = signature
                }, _azureStorageSettings.SurveyWorkEmailTemplate);
            }

            return emailBody;
        }

    }
}
