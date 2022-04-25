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

namespace WAS.Application.Features.Survey.SubmitSurvey
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IWasContext _context;
        private readonly ICosmosProvider _cosmosProvider;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            ICosmosProvider cosmosProvider
            )
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _cosmosProvider = cosmosProvider;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var subscription = await _context.Subscriptions
                                 .SingleOrDefaultAsync(s => (s.OfficialEmail == request.Email || s.EmployeeId == request.EmployeeId), cancellationToken);

                if (subscription != null)
                    request.SubscriberId = subscription.Id;

                var answers = _mapper.Map<Common.Models.SubmitSurvey>(request);
                answers.id = request.EmployeeId;

                bool flag = false, IsAlreadySubmitted = false;
                Container cosmosContainer = await _cosmosProvider.GetSurveySubmissionProvider();
                try
                {
                    // Read the item to see if it exists.  
                    ItemResponse<Common.Models.SubmitSurvey> surveyResponse = await cosmosContainer.ReadItemAsync<Common.Models.SubmitSurvey>(answers.id.ToString(), new PartitionKey(answers.BroadcastId.ToString()));
                    _logger.LogInformation("Item in database with EmployeeId: {0} already exists", surveyResponse.Resource.id);
                    IsAlreadySubmitted = true;
                }
                catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    await cosmosContainer.CreateItemAsync<Common.Models.SubmitSurvey>(answers, new PartitionKey(answers.BroadcastId.ToString()));
                    flag = true;
                }

                return new Response { Success = flag, IsAlreadySubmitted = IsAlreadySubmitted };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
