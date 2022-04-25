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
using WAS.Domain.Entities;

namespace WAS.Application.Features.Survey.Broadcast
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IBlobTransactionService _blobTransactionService;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IBlobTransactionService blobTransactionService,
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _blobTransactionService = blobTransactionService;
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var broadcast = _mapper.Map<Domain.Entities.SurveyBroadcast>(request);
                broadcast.ModifiedBy = broadcast.CreatedBy;
                //getting survey data
                var result = await _context.Surveys
                             .SingleOrDefaultAsync(b => b.Id == request.SurveyId, cancellationToken);
                if (result == null)
                    throw new BadRequestException("Survey not found");
                broadcast.Subject = result.Subject;
                broadcast.Description = result.Description;
                broadcast.NumberofQuestions = result.NumberofQuestions;
                broadcast.Path = await getJsonPath(result.Path);
                await _context.SurveyBroadcasts.AddAsync(broadcast, cancellationToken);

                if (request.FollowUpTime != null)
                {
                    var surveyFollowup = new Domain.Entities.SurveyBroadcastFollowup();
                    surveyFollowup.SurveyBroadcastId = broadcast.Id;
                    surveyFollowup.FollowUpDate = request.FollowUpTime ?? DateTime.UtcNow;
                    surveyFollowup.CreatedBy = surveyFollowup.ModifiedBy = broadcast.CreatedBy;
                    surveyFollowup.Status = Domain.Enum.SurveyStatus.Submitted;
                    await _context.SurveyBroadcastFollowups.AddAsync(surveyFollowup, cancellationToken);
                }

                var surveyGroups = request.GroupId.Select(gid => new Domain.Entities.SurveyBroadcastGroup { GroupId = gid, SurveyBroadcastId = broadcast.Id });
                var sureySubscriptions = request.SubscriptionId.Select(sid => new Domain.Entities.SurveyBroadcastSubscription { SubscriptionId = sid, SurveyBroadcastId = broadcast.Id });
                await _context.SurveyBroadcastGroups.AddRangeAsync(surveyGroups, cancellationToken);
                await _context.SurveyBroadcastSubscriptions.AddRangeAsync(sureySubscriptions, cancellationToken);

                if (request.DistributionGroups != null && request.DistributionGroups.Any())
                {
                    var surveyDGroups = request.DistributionGroups.Select(grp => new Domain.Entities.SurveyBroadcastDistributionGroup
                    {
                        DistributionGroup = grp.EmailId,
                        DistributionGroupId = grp.Id,
                        DistributionGroupName = grp.Name,
                        SurveyBroadcastId = broadcast.Id,
                        CreatedBy = broadcast.CreatedBy,
                        ModifiedBy = broadcast.CreatedBy
                    });
                    await _context.SurveyBroadcastDistributionGroup.AddRangeAsync(surveyDGroups, cancellationToken);
                }

                if (request.ADPeople != null && request.ADPeople.Any())
                {
                    var surveyADPeople = new List<SurveyBroadcastADUser>();
                    foreach (var ppl in request.ADPeople)
                    {
                        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Name.Equals(ppl.Department), cancellationToken);
                        var location = await _context.Locations.FirstOrDefaultAsync(d => d.Name.Equals(ppl.Location), cancellationToken);

                        var adUser = new SurveyBroadcastADUser()
                        {
                            FirstName = ppl.FirstName,
                            LastName = ppl.LastName,
                            EmailId = ppl.EmailId,
                            DepartmentId = department.Id,
                            LocationId = location.Id,
                            SurveyBroadcastId = broadcast.Id,
                            CreatedBy = broadcast.CreatedBy,
                            ModifiedBy = broadcast.CreatedBy,
                        };
                        surveyADPeople.Add(adUser);
                    }
                    await _context.SurveyBroadcastADUser.AddRangeAsync(surveyADPeople, cancellationToken);
                }

                if (request.IsText)
                {
                    var surveyText = new Domain.Entities.SurveyBroadcastText();
                    surveyText.SurveyBroadcastId = broadcast.Id;
                    surveyText.CreatedBy = surveyText.ModifiedBy = broadcast.CreatedBy;
                    await _context.SurveyBroadcastTexts.AddAsync(surveyText, cancellationToken);
                }

                if (request.IsTeams)
                {
                    var surveyTeams = new Domain.Entities.SurveyBroadcastTeams();
                    surveyTeams.SurveyBroadcastId = broadcast.Id;
                    surveyTeams.CreatedBy = surveyTeams.ModifiedBy = broadcast.CreatedBy;
                    await _context.SurveyBroadcastTeams.AddAsync(surveyTeams, cancellationToken);
                }

                if (request.IsEmail)
                {
                    var surveyEmail = new Domain.Entities.SurveyBroadcastEmail();
                    surveyEmail.SurveyBroadcastId = broadcast.Id;
                    surveyEmail.CreatedBy = surveyEmail.ModifiedBy = broadcast.CreatedBy;
                    await _context.SurveyBroadcastEmails.AddAsync(surveyEmail, cancellationToken);
                }

                if (request.IsWhatsApp)
                {
                    var surveyWhatsapp = new Domain.Entities.SurveyBroadcastWhatsApp();
                    surveyWhatsapp.SurveyBroadcastId = broadcast.Id;
                    surveyWhatsapp.CreatedBy = surveyWhatsapp.ModifiedBy = broadcast.CreatedBy;
                    await _context.SurveyBroadcastWhatsApps.AddAsync(surveyWhatsapp, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Success = true, Id = broadcast.Id };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new Response { Success = false };
            }
        }

        private async Task<string> getJsonPath(string jsonPath)
        {
            string path = "";
            Uri SurveyUrl = null;
            string JsonData = await _azureStorageService.DowloadTemplatejsonFromBlobStorage(jsonPath);
            var blobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + ".json";

            var surveyJSONBlob = await _blobTransactionService.UploadJSONToBlobAsync(new TemplateBlob()
            {
                BlobContainerName = "was-survey-questionnaire",
                BlobTypeName = "application/json",
                BlobFileName = blobFileName,
                JsonData = JsonData,
                StorageConnectionString = _azureStorageSettings.StorageConnectionString
            });

            SurveyUrl = surveyJSONBlob != null ? new Uri(surveyJSONBlob.BlobUri) : null;
            if (SurveyUrl != null)
                path = SurveyUrl.AbsoluteUri;

            return path;
        }
    }
}
