using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Constants;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;


namespace WAS.Application.Features.Training.GetVideo
{
    public class Handler : IRequestHandler<Request, Response>
    {

        private readonly ILogger<Handler> _logger;
        private readonly WasApiSettings _wASApiSettings;
        private readonly IServiceBase _serviceBase;
        private readonly AzureStorageSettings _azureStorageSettings;

        public Handler(
            ILogger<Handler> logger,
            IOptions<WasApiSettings> wASApiSettings,
            IOptions<AzureStorageSettings> azurestoragesettings,
            IServiceBase serviceBase
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _azureStorageSettings = azurestoragesettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();

                var GetVideoDetails = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetVideo}/{request.Language ?? DefaultLanguage.Language}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                string sastoken = _azureStorageSettings.SasToken;
                StringBuilder temp = new StringBuilder();
                foreach (var videoCategory in GetVideoDetails.VideoCategories)
                {
                    temp.Clear();
                    videoCategory.CategoryThumbnail= temp.Append(videoCategory.CategoryThumbnail).Append(sastoken).ToString();
                    foreach (var trainingvideo in videoCategory.TrainingVideos)
                    {
                        temp.Clear();
                        trainingvideo.URL = temp.Append(trainingvideo.URL).Append(sastoken).ToString();
                        temp.Clear();
                        trainingvideo.VideoThumbnail=temp.Append(trainingvideo.VideoThumbnail).Append(sastoken).ToString();
                    }
                }
                return GetVideoDetails;
            }
            catch (FlurlHttpException ex)
            {
                _logger.LogError(ex.Message, ex);
                var problemDetails = await ex.GetResponseJsonAsync<ProblemDetailsResponse>();

                if (problemDetails == null)
                    throw ProblemDetailsResponseExtensions.Exception(ex.Message);

                throw problemDetails.Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}
