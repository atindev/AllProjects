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

namespace WAS.Application.Features.Groups.QueryView
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
            var token = await _serviceBase.GenerateAuthenticationTokenAsync();
            try
            {
                  var locationResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllLocations}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);
                       
                    
                  var shiftResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllShifts}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                  var departmentResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllDepartments}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                var  cityResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllCity}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                 var stateResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllState}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                var countryResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllCountry}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                var employeeTypeResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllEmployeeType}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                var employeeGroupResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllEmployeeGroup}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                var jobTitleResult = await $"{_wASApiSettings.ApiBaseUrl}/{EndPointSettings.GetAllJobTitle}"
                                        .WithOAuthBearerToken(token)
                                        .GetJsonAsync<Response>(cancellationToken);

                return new Response
                {
                    Locations = locationResult.Locations,
                    Shifts = shiftResult.Shifts,
                    Departments =departmentResult.Departments,
                    Cities =cityResult.Cities,
                    States = stateResult.States,
                    Countries = countryResult.Countries,
                    EmployeeTypes =employeeTypeResult.EmployeeTypes,
                    EmployeeGroups = employeeGroupResult.EmployeeGroups,
                    JobTitles =jobTitleResult.JobTitles,
                };
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
