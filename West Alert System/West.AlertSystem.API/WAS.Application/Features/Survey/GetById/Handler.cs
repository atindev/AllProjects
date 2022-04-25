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

namespace WAS.Application.Features.Survey.GetById
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IAzureStorageService _azureStorageService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IAzureStorageService azureStorageService
            )
        {
            _context = context;
            _logger = logger;
            _azureStorageService = azureStorageService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();

                var survey = await _context.Surveys
                     .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

                if (survey == null)
                    throw new NotFoundException($"survey not found with the id {request.Id}");

                response.SurveyContent = await _azureStorageService.DowloadTemplatejsonFromBlobStorage(survey.Path);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}

