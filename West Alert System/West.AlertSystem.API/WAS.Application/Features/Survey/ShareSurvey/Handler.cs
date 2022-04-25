using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Survey.ShareSurvey
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IShareSurveyService _shareSurveyService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IShareSurveyService shareSurveyService
            )
        {
            _context = context;
            _logger = logger;
            _shareSurveyService = shareSurveyService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                List<Domain.Entities.SurveyDetailShare> surveyDetailShare = new List<Domain.Entities.SurveyDetailShare>();
                for (int i = 0; i < request.PeopleMail.Count; i++)
                {
                    surveyDetailShare.Add(new Domain.Entities.SurveyDetailShare
                    {
                        Id = Guid.NewGuid(),
                        BroadcastId = request.BroadcastId,
                        OfficialMail = request.PeopleMail[i],
                        CreatedBy = request.CreatedBy
                    });
                }

                await _context.SurveyDetailShare.AddRangeAsync(surveyDetailShare, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                ShareSurveyData shareSurvey = new ShareSurveyData()
                {
                    BroadcastId = request.BroadcastId,
                    PeopleMail = request.PeopleMail,
                    CreatedBy = request.CreatedBy
                };
                await _shareSurveyService.ShareSurvey(shareSurvey);
                return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
    }
}
