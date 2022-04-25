using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.IncomingMessage.GetAudio
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IAzureStorageService _azureStorageService;

        public Handler(
            ILogger<Handler> logger,
            IAzureStorageService azureStorageService
            )
        {
            _logger = logger;
            _azureStorageService = azureStorageService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var audioContent = await _azureStorageService.DowloadFileFromBlobStorage(request.Path);
                var response = new Response
                {
                    Content = Convert.ToBase64String(audioContent),
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
