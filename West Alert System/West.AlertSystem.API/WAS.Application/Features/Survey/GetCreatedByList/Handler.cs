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
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetCreatedByList
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
                var survey = await _context.Surveys.Select(X => X.CreatedBy).Distinct().ToListAsync(cancellationToken);
                return new Response { CreatedByList = survey };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
    }
}
