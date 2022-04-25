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

namespace WAS.Application.Features.Survey.CreateUpdate
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IBlobTransactionService _blobTransactionService;
        private readonly AzureStorageSettings _azureStorageSettings;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IBlobTransactionService blobTransactionService,
            IOptions<AzureStorageSettings> azureStorageSettings
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _blobTransactionService = blobTransactionService;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                Uri SurveyUrl = null;
                Guid surveyId = Guid.Empty;
                var JsonData = JsonConvert.SerializeObject(request);
                var blobFileName = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss") + ".json";

                var surveyJSONBlob = await _blobTransactionService.UploadJSONToBlobAsync(new TemplateBlob()
                {
                    BlobContainerName = "was-survey-questionnaire",
                    BlobTypeName = "application/json",
                    BlobFileName = blobFileName,
                    JsonData = JsonData,
                    StorageConnectionString = _azureStorageSettings.StorageConnectionString
                });

                SurveyUrl = surveyJSONBlob != null ? new Uri(surveyJSONBlob.BlobUri) : null;

                if (SurveyUrl != null)
                {
                    if (request.Id == null || request.Id == Guid.Empty)
                    {
                        var surveyModel = _mapper.Map<Domain.Entities.Survey>(request);
                        surveyModel.Path = SurveyUrl.AbsoluteUri;
                        surveyModel.NumberofQuestions = request.Questions.Count;
                        await _context.Surveys.AddAsync(surveyModel, cancellationToken);
                        surveyId = surveyModel.Id;
                    }
                    else
                    {
                        var result = await _context.Surveys
                                                        .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                        if (result == null)
                            throw new BadRequestException("Survey not found");

                        var surveyEntity = _mapper.Map(request, result);
                        surveyEntity.Path = SurveyUrl.AbsoluteUri;
                        surveyEntity.NumberofQuestions = request.Questions.Count;
                        _context.Surveys.Attach(surveyEntity).State = EntityState.Modified;
                        surveyId = request.Id;
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
                return new Response { Success = true, Id = surveyId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
