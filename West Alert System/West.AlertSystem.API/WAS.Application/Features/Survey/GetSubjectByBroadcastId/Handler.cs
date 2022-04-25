using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using WAS.Application.Interface.Services;
using Entity = WAS.Domain.Entities;

namespace WAS.Application.Features.Survey.GetSubjectByBroadcastId
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
       
        public Handler(
            IWasContext context,
            ILogger<Handler> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var surveyEntity = await _context.SurveyBroadcasts
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
                 
                if (surveyEntity == null)
                    throw new NotFoundException($"Broadcasted survey not found with the id {request.Id}");
                 
                return new Response() { 
                   Subject = surveyEntity.Subject
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }

      
    }
}

