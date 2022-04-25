using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Notification.ViewAttachment
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
                var attachmentEntity = await _context.NotificationEmailAttachments
                    .IgnoreQueryFilters()
                    .SingleOrDefaultAsync(ea => ea.NotificationEmailId.Equals(request.NotificationEmailId) && ea.FileName.Equals(request.FileName));

                if (attachmentEntity == null)
                    throw new NotFoundException($"Attachment not found with the Email Id {request.NotificationEmailId} & {request.FileName}");

                var fileContent = await _azureStorageService.DowloadFileFromBlobStorage(attachmentEntity.Attachment);
                var response = new Response
                {
                    FileName = attachmentEntity.FileName,
                    Content = Convert.ToBase64String(fileContent),
                    ContentType = attachmentEntity.ContentType
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
