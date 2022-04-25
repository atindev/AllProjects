using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using Microsoft.EntityFrameworkCore;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Survey.GetJsonByBroadcastId
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

                var survey = await _context.SurveyBroadcasts
                     .IgnoreQueryFilters()
                     .SingleOrDefaultAsync(i => (i.Id == request.Id && i.EndTime >= DateTime.UtcNow), cancellationToken);

                if (survey == null)
                    response.SurveyContent = "";
                else
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

