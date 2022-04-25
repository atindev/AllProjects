using AutoMapper;
using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Survey.GetAnswerwiseReport
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;

        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();
                var surveyResponse = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAnswerwiseReport}/{request.Id}"
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<Response>(cancellationToken);

                if (surveyResponse == null)
                    return null;

                surveyResponse = FormatOtherOptions(surveyResponse);
                return surveyResponse;
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

        private Response FormatOtherOptions(Response surveyResponse)
        {
            foreach (SurveyAnswer surveyAnswer in surveyResponse.Answers.Where(x => x.OtherOptions.Any()))
            {
                surveyAnswer.FormattedOtherOptions= surveyAnswer.OtherOptions.GroupBy(x => x).Select(x => new WordCloud
                {
                    Text=x.Key,
                    Frequency=x.Count()
                }).ToList();
            }
            return surveyResponse;
        }
    }
}
