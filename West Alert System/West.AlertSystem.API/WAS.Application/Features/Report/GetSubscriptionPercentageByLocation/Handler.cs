using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Report.GetSubscriptionPercentageByLocation
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IGraphAuthProvider _graphAuthProvider;
        private readonly HttpClient _httpClient;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IGraphAuthProvider graphAuthProvider,
            IHttpClientFactory clientFactory
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _graphAuthProvider = graphAuthProvider;
            _httpClient = clientFactory != null ? clientFactory.CreateClient("GraphHttpClient") : throw new ArgumentNullException(nameof(clientFactory));
        }
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var graphAccessToken = await _graphAuthProvider.GetGraphAccessTokenAsync();
                _httpClient.DefaultRequestHeaders.Add("Authorization", graphAccessToken);
                _httpClient.DefaultRequestHeaders.Add("ConsistencyLevel", "eventual");

                var locationCount = await _context.Subscriptions.Where(x=>x.IsActive)
                                        .Select(x => x.Location)
                                        .ToListAsync(cancellationToken);
                locationCount.RemoveAll(i => i == null);
                var subscriptionlocationdictionary = locationCount.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
                List<SubscriptionPercentage> subscriptionPercentages = new List<SubscriptionPercentage>();
                List<RemainingSubscriptionPercentage> remainingSubscriptionPercentages = new List<RemainingSubscriptionPercentage>();
                int gobalsubscriptionCount=new int();
                foreach(var item in subscriptionlocationdictionary)
                {
                    var response = _httpClient.GetAsync($"https://graph.microsoft.com/v1.0/users/$count?$filter=officeLocation eq '{item.Key.Name}' and accountEnabled eq true and jobTitle ne null and endsWith(userPrincipalName,'westpharma.com')").Result;
                    var responseJsonString = await response.Content.ReadAsStringAsync();
                    var employeeCount = JsonConvert.DeserializeObject<int>(responseJsonString);
                    decimal subscriptionCount = item.Value;
                    decimal remainingSubscriptionCount = employeeCount - subscriptionCount;
                    decimal subscriptionPercent = Math.Round((subscriptionCount / employeeCount) * 100, 2);
                    decimal remainingPercent = 100 - subscriptionPercent;
                    gobalsubscriptionCount  += item.Value;
                    SubscriptionPercentage subscriptionPercentage = new SubscriptionPercentage
                    {
                        LocationName = item.Key.Name,
                        SubscribedCount = subscriptionCount,
                        SubscriptionPercent = subscriptionPercent
                    };
                    RemainingSubscriptionPercentage remainingSubscriptionPercentage = new RemainingSubscriptionPercentage
                    {
                        LocationName = item.Key.Name,
                        RemainingSubscribedCount = remainingSubscriptionCount,
                        RemainingPercent = remainingPercent
                    };
                    subscriptionPercentages.Add(subscriptionPercentage);
                    remainingSubscriptionPercentages.Add(remainingSubscriptionPercentage);
                }
                var globalResponse = _httpClient.GetAsync($"https://graph.microsoft.com/v1.0/users/$count?$filter= accountEnabled eq true and jobTitle ne null and endsWith(userPrincipalName,'westpharma.com')").Result;
                var globalresponseJsonString = await globalResponse.Content.ReadAsStringAsync();
                var globalemployeeCount = JsonConvert.DeserializeObject<int>(globalresponseJsonString);
                
                var globalSubscriptionPercenatge = Math.Round((Convert.ToDecimal(gobalsubscriptionCount) / Convert.ToDecimal(globalemployeeCount)) * 100, 2);

                LocationCountBySubscriptionPercentage locationCountBySubscriptionPercentage = new LocationCountBySubscriptionPercentage()
                {
                    RemainingSubscriptionPercentages = new List<RemainingSubscriptionPercentage>(),
                    SubscriptionPercentages = new List<SubscriptionPercentage>(),
                    GlobalSubscription = gobalsubscriptionCount,
                    GlobalEmployeeCount = globalemployeeCount,
                    GlobalSubscriptionPercentage = globalSubscriptionPercenatge.ToString()
                };
                locationCountBySubscriptionPercentage.RemainingSubscriptionPercentages.AddRange(remainingSubscriptionPercentages.OrderBy(x=>x.RemainingPercent).ToList());
                locationCountBySubscriptionPercentage.SubscriptionPercentages.AddRange(subscriptionPercentages.OrderByDescending(x => x.SubscriptionPercent).ToList());
                var responseSubscriptionPercentage = _mapper.Map<LocationCountBySubscriptionPercentage>(locationCountBySubscriptionPercentage);
                return new Response { SubscriptionLocationPercentage = responseSubscriptionPercentage };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
