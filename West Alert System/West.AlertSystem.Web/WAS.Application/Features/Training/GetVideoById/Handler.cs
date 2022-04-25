using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Training.GetVideoById
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

                var getVideo = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetVideoById}/{request.videoId}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                string sastoken = _azureStorageSettings.SasToken;
                StringBuilder temp = new StringBuilder();
                getVideo.TrainingVideo.URL=temp.Append(getVideo.TrainingVideo.URL).Append(sastoken).ToString();
                getVideo.Videos.ForEach(video =>
                {
                    temp.Clear();
                    video.VideoThumbnail= temp.Append(video.VideoThumbnail).Append(sastoken).ToString();
                });
                return getVideo;
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
