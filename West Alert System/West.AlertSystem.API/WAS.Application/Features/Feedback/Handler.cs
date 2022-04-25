using AutoMapper.Configuration;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Feedback
{
    public class Handler : IRequestHandler<Request, Response>
    {

        private readonly ILogger<Handler> _logger;
        public readonly IDevopsService _devopsService;      
        private readonly IBlobTransactionService _blobTransactionService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public Handler(           
            ILogger<Handler> logger,
            IDevopsService devopsService,
            IBlobTransactionService blobTransactionService,
            IOptions<AzureStorageSettings> azureStorageSettings
            )
        {
            _logger = logger;
            _devopsService = devopsService;
            _blobTransactionService = blobTransactionService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                Uri ScreenshotUrl = null;

                if (request.isScreenshotRequired && !string.IsNullOrEmpty(request.PictureFileName) && request.PictureBase64 != null)
                {
                    var feedbackPictureBlob = await _blobTransactionService.UploadToBlobAsync(new FeedbackBlob()
                    {
                        BlobContainerName = "feedbackscreenshots",
                        BlobTypeName = "image/jpg",
                        BlobFileName = request.PictureFileName.Replace(" ", ""),
                        FileB64StringData = request.PictureBase64,
                        StorageConnectionString = _azureStorageSettings.StorageConnectionString
                    });

                    ScreenshotUrl = feedbackPictureBlob != null ? new Uri($"{feedbackPictureBlob.BlobUri}{_azureStorageSettings.SasToken}") : null;
                }
                var feedbackResource = new FeedbackResource()
                {
                    SubmittedBy = request.Username,
                    SubmittedByMail = request.UserEmailID,
                    Title = request.Title,
                    Description = request.Description,
                    ScreenshotUri = ScreenshotUrl,
                    SubmittedOn = DateTime.Now
                };

                var response= await _devopsService.SubmitUserFeedback(feedbackResource); 


                return new Response{ 
                StatusCode=response.StatusCode,
                Message=response.Message,
                Success = true
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
