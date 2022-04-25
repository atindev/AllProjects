using AutoMapper;
using Flurl.Http;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Template.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IServiceBase _serviceBase;
        private readonly WasApiSettings _wASApiSettings;
        private readonly IMapper _mapper;


        public Handler(
            ILogger<Handler> logger,
            IServiceBase serviceBase,
            IOptions<WasApiSettings> wASApiSettings,
            IMapper mapper
            )
        {
            _logger = logger;
            _serviceBase = serviceBase;
            _wASApiSettings = wASApiSettings.Value;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _serviceBase.GenerateAuthenticationTokenAsync();

                if (request.CategoryId==0)
                {
                    var newCategory = _mapper.Map<CreateTemplateCategory>(request);
                    var categoryResponse = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.CreateCategory}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(newCategory, cancellationToken);

                    if (!categoryResponse.IsSuccessStatusCode)
                        return null;

                    var catResult = await categoryResponse.Content.ReadAsStringAsync();
                    var categoryResponseContent = JsonConvert.DeserializeObject<CreateCategoryResponse>(catResult);
                    request.CategoryId = categoryResponseContent.Id;
                }


                var templateResponse = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.CreateTemplate}"
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(request, cancellationToken);
                 
                if (!templateResponse.IsSuccessStatusCode)
                    return null;

                var result = await templateResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<Response>(result);
                response.CategoryId = request.CategoryId;

                return response;
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
