using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using Microsoft.Identity.Client;

namespace WAS.Application.Features.Survey.GetAllSharedSurvey
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
                List<Guid> broadcastedIds = new List<Guid>();
                broadcastedIds.AddRange(await _context.SurveyDetailShare.Where(x => x.OfficialMail == request.OfficialMail).Select(x => x.BroadcastId).ToListAsync(cancellationToken));
                var distinctbroadcastedIds = broadcastedIds.Distinct().ToList();
                return new Response
                {
                    BroadcastedSurveyIds = distinctbroadcastedIds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
