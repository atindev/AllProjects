using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Survey.UpdateBroadcast
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly IWasContextAdmin _contextAdmin;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            IWasContext context,
            IWasContextAdmin contextAdmin,
            ILogger<Handler> logger,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _contextAdmin = contextAdmin;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                string modifiedBy = request.CreatedBy;

                //updating broadcast table
                var result = await _context.SurveyBroadcasts
                               .SingleOrDefaultAsync(b => b.Id == request.BroadcastId, cancellationToken);

                if (result == null)
                    throw new BadRequestException("Broadcasted survey not found");

                request.CreatedBy = result.CreatedBy;
                var broadcastEntity = _mapper.Map(request, result);
                broadcastEntity.ModifiedBy = modifiedBy;
                _context.SurveyBroadcasts.Attach(broadcastEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);

                //updating followup table
                var followUpResult = await _contextAdmin.SurveyBroadcastFollowups
                                     .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                                     .ToListAsync(cancellationToken);
                if (followUpResult != null && followUpResult.Any())
                {
                    _contextAdmin.SurveyBroadcastFollowups.RemoveRange(followUpResult);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }
                if (request.FollowUpTime != null)
                {
                    var surveyFollowup = new Domain.Entities.SurveyBroadcastFollowup();
                    surveyFollowup.SurveyBroadcastId = request.BroadcastId;
                    surveyFollowup.FollowUpDate = request.FollowUpTime ?? DateTime.UtcNow;
                    surveyFollowup.CreatedBy = surveyFollowup.CreatedBy = modifiedBy;
                    surveyFollowup.Status = Domain.Enum.SurveyStatus.Submitted;
                    await _context.SurveyBroadcastFollowups.AddAsync(surveyFollowup, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }


                //updating group table
                var surveyGropusforDelete = await _contextAdmin.SurveyBroadcastGroups
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
                if (surveyGropusforDelete != null && surveyGropusforDelete.Any())
                {
                    _contextAdmin.SurveyBroadcastGroups.RemoveRange(surveyGropusforDelete);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }
                var surveyGroups = request.GroupId.Select(gid => new Domain.Entities.SurveyBroadcastGroup { GroupId = gid, SurveyBroadcastId = request.BroadcastId });
                if (surveyGroups != null && surveyGroups.Any())
                {
                    await _contextAdmin.SurveyBroadcastGroups.AddRangeAsync(surveyGroups, cancellationToken);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }

                //updating the subscription table
                var peopleforDelete = await _contextAdmin.SurveyBroadcastSubscriptions
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
                if (peopleforDelete != null && peopleforDelete.Any())
                {
                    _contextAdmin.SurveyBroadcastSubscriptions.RemoveRange(peopleforDelete);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }
                var sureySubscriptions = request.SubscriptionId.Select(sid => new Domain.Entities.SurveyBroadcastSubscription { SubscriptionId = sid, SurveyBroadcastId = request.BroadcastId });
                if (sureySubscriptions != null && sureySubscriptions.Any())
                {
                    await _contextAdmin.SurveyBroadcastSubscriptions.AddRangeAsync(sureySubscriptions, cancellationToken);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }

                //updating distribution group table
                var surveydistributionGrpforDelete = await _contextAdmin.SurveyBroadcastDistributionGroup
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
                if (surveydistributionGrpforDelete != null && surveydistributionGrpforDelete.Any())
                {
                    _contextAdmin.SurveyBroadcastDistributionGroup.RemoveRange(surveydistributionGrpforDelete);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }

                if (request.DistributionGroups != null && request.DistributionGroups.Any())
                {
                    var surveyDGroups = request.DistributionGroups.Select(grp => new Domain.Entities.SurveyBroadcastDistributionGroup
                    {
                        DistributionGroup = grp.EmailId,
                        DistributionGroupId = grp.Id,
                        DistributionGroupName = grp.Name,
                        SurveyBroadcastId = request.BroadcastId,
                        ModifiedBy = modifiedBy
                    });
                    await _contextAdmin.SurveyBroadcastDistributionGroup.AddRangeAsync(surveyDGroups, cancellationToken);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }

                //updating the AD User table
                var adPeopleforDelete = await _contextAdmin.SurveyBroadcastADUser
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
                if (adPeopleforDelete != null && adPeopleforDelete.Any())
                {
                    _contextAdmin.SurveyBroadcastADUser.RemoveRange(adPeopleforDelete);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
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
                            SurveyBroadcastId = request.BroadcastId,
                            ModifiedBy = modifiedBy,
                        };
                        surveyADPeople.Add(adUser);
                    }
                    await _contextAdmin.SurveyBroadcastADUser.AddRangeAsync(surveyADPeople, cancellationToken);
                    await _contextAdmin.SaveChangesAsync(cancellationToken);
                }

                await UpdatePreference(request, modifiedBy, cancellationToken);

                return new Response { Success = true, Id = request.BroadcastId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new Response { Success = false };
            }
        }

        public async Task UpdatePreference(Request request, string modifieddBy, CancellationToken cancellationToken)
        {
            //for email
            var emailResult = await _contextAdmin.SurveyBroadcastEmails
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
            if (emailResult != null && emailResult.Any())
            {
                _contextAdmin.SurveyBroadcastEmails.RemoveRange(emailResult);
                await _contextAdmin.SaveChangesAsync(cancellationToken);
            }
            if (request.IsEmail)
            {
                var surveyEmail = new Domain.Entities.SurveyBroadcastEmail();
                surveyEmail.SurveyBroadcastId = request.BroadcastId;
                surveyEmail.CreatedBy = surveyEmail.ModifiedBy = modifieddBy;
                await _context.SurveyBroadcastEmails.AddAsync(surveyEmail, cancellationToken);
            }

            //for text
            var textResult = await _contextAdmin.SurveyBroadcastTexts
                                .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                                .ToListAsync(cancellationToken);
            if (textResult != null && textResult.Any())
            {
                _contextAdmin.SurveyBroadcastTexts.RemoveRange(textResult);
                await _contextAdmin.SaveChangesAsync(cancellationToken);
            }
            if (request.IsText)
            {
                var surveyText = new Domain.Entities.SurveyBroadcastText();
                surveyText.SurveyBroadcastId = request.BroadcastId;
                surveyText.CreatedBy = surveyText.ModifiedBy = modifieddBy;
                await _context.SurveyBroadcastTexts.AddAsync(surveyText, cancellationToken);
            }

            //for teams
            var teamResult = await _contextAdmin.SurveyBroadcastTeams
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
            if (teamResult != null && teamResult.Any())
            {
                _contextAdmin.SurveyBroadcastTeams.RemoveRange(teamResult);
                await _contextAdmin.SaveChangesAsync(cancellationToken);
            }
            if (request.IsTeams)
            {
                var surveyTeams = new Domain.Entities.SurveyBroadcastTeams();
                surveyTeams.SurveyBroadcastId = request.BroadcastId;
                surveyTeams.CreatedBy = surveyTeams.ModifiedBy = modifieddBy;
                await _context.SurveyBroadcastTeams.AddAsync(surveyTeams, cancellationToken);
            }

            //for whatsapp
            var whatsappResult = await _contextAdmin.SurveyBroadcastWhatsApps
                               .Where(b => b.SurveyBroadcastId == request.BroadcastId)
                               .ToListAsync(cancellationToken);
            if (whatsappResult != null && whatsappResult.Any())
            {
                _contextAdmin.SurveyBroadcastWhatsApps.RemoveRange(whatsappResult);
                await _contextAdmin.SaveChangesAsync(cancellationToken);
            }
            if (request.IsWhatsApp)
            {
                var surveyWhatsapp = new Domain.Entities.SurveyBroadcastWhatsApp();
                surveyWhatsapp.SurveyBroadcastId = request.BroadcastId;
                surveyWhatsapp.CreatedBy = surveyWhatsapp.ModifiedBy = modifieddBy;
                await _context.SurveyBroadcastWhatsApps.AddAsync(surveyWhatsapp, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
