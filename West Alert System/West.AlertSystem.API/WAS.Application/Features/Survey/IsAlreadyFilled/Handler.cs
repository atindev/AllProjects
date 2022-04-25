using AutoMapper;
using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;

namespace WAS.Application.Features.Survey.IsAlreadyFilled
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IWasContext _context;
        private readonly ICosmosProvider _cosmosProvider;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            ICosmosProvider cosmosProvider
            )
        {
            _logger = logger;
            _context = context;
            _cosmosProvider = cosmosProvider;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            { 
                var subscription = await _context.Subscriptions
                                 .SingleOrDefaultAsync(s => (s.OfficialEmail == request.Email || s.EmployeeId == request.EmployeeId), cancellationToken);

                    var answers = new Common.Models.SubmitSurvey();
                    answers.SubscriberId = subscription != null ? subscription.Id : Guid.Empty;
                    answers.id = request.EmployeeId;
                    answers.Email = request.Email;
                    answers.BroadcastId = request.BroadcastId;
                    bool IsNotFilled = false;
                    Container cosmosContainer = await _cosmosProvider.GetSurveySubmissionProvider();
                    try
                    {
                        // Read the item to see if it exists.  
                        ItemResponse<Common.Models.SubmitSurvey> surveyResponse = await cosmosContainer.ReadItemAsync<Common.Models.SubmitSurvey>(answers.id, new PartitionKey(answers.BroadcastId.ToString()));
                        _logger.LogInformation("Item in database with EmployeeId: {0} already exists", surveyResponse.Resource.id);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        IsNotFilled = true;
                    }

                    return new Response { IsNotFilled = IsNotFilled };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
