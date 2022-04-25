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

namespace WAS.Application.Features.Template.GetById
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IAzureStorageService _azureStorageService;


        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IMediator mediator,
            IAzureStorageService azureStorageService

            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _azureStorageService = azureStorageService;

        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new Response();

                var Templates = await _context.Templates
                     .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

                if (Templates == null)
                    throw new NotFoundException($"Template not found with the id {request.Id}");

                response.TemplateContent = await _azureStorageService.DowloadTemplatejsonFromBlobStorage(Templates.Path);

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

