using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;


namespace WAS.Application.Features.Template.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IBlobTransactionService _blobTransactionService;
        private readonly AzureStorageSettings _azureStorageSettings;
        private readonly IAzureStorageService _azureStorageService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IBlobTransactionService blobTransactionService,
            IOptions<AzureStorageSettings> azureStorageSettings,
            IAzureStorageService azureStorageService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _blobTransactionService = blobTransactionService;
            _azureStorageSettings = azureStorageSettings.Value;
            _azureStorageService = azureStorageService;

        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                //email attachment saving to blob
                request.EmailAttachmentsURL = await getEmailAttachmentsAsync(request.EmailAttachments, request.ExistingEmailAttachments);

                Uri TemplateUrl = null;
                request.EmailAttachments = new List<AttachmentData>();
                var JsonData = JsonConvert.SerializeObject(request);
                var blobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + ".json";

                var templateJSONBlob = await _blobTransactionService.UploadJSONToBlobAsync(new TemplateBlob()
                {
                    BlobContainerName = "was-notification-templates",
                    BlobTypeName = "application/json",
                    BlobFileName = blobFileName,
                    JsonData = JsonData,
                    StorageConnectionString = _azureStorageSettings.StorageConnectionString
                });

                TemplateUrl = templateJSONBlob != null ? new Uri(templateJSONBlob.BlobUri) : null;

                if (TemplateUrl != null)
                {
                     
                    if (request.Id == Guid.Empty)
                    {
                        var templateModel = _mapper.Map<Domain.Entities.Template>(request);
                        templateModel.Path = TemplateUrl.AbsoluteUri;
                        await _context.Templates.AddAsync(templateModel, cancellationToken);
                    }
                    else
                    {
                        var result = await _context.Templates
                                .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                        if (result == null)
                            throw new BadRequestException("Template not found");

                        var templateEntity = _mapper.Map(request, result);
                        templateEntity.Path= TemplateUrl.AbsoluteUri;
                        _context.Templates.Attach(templateEntity).State = EntityState.Modified;
                    }

                        await _context.SaveChangesAsync(cancellationToken);
                }

                return new Response { Success = true };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }

        private async Task<List<AttachmentDetails>> getEmailAttachmentsAsync(List<AttachmentData> EmailAttachments, List<string> ExistingEmailAttachments)
        {
            var attachments = new List<AttachmentDetails>();

            if (EmailAttachments.Any() || ExistingEmailAttachments.Any())
            {
                foreach (AttachmentData emailAttachment in EmailAttachments)
                {
                    var eaByteArray = Convert.FromBase64String(emailAttachment.Content);
                    var emailBlobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + Path.GetExtension(emailAttachment.FileName);

                    var result = await _azureStorageService.UploadFileToBlobStorage(
                                                    emailBlobFileName,
                                                    eaByteArray);

                    if (result != null && result.AbsoluteUri != null)
                    {
                        attachments.Add(new AttachmentDetails()
                        {

                            URL = result.AbsoluteUri,
                            FileName = emailAttachment.FileName,
                            ContentType = emailAttachment.ContentType

                        });
                    }
                }

                if (ExistingEmailAttachments.Any())
                {
                    foreach (var attachmentURL in ExistingEmailAttachments)
                    {
                        var content = attachmentURL.Split("|");
                        if (content.Length > 2)
                        {
                            attachments.Add(new AttachmentDetails()
                            {

                                URL = content[0],
                                FileName = content[1],
                                ContentType = content[2]

                            });
                        }
                    }

                }

            }

            return attachments;
        }
    }
}
