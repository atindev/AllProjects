using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface.Services;
using Microsoft.Azure.Cosmos;

namespace WAS.Application.Features.Survey.GetAllSubmitted
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly ILogger<Handler> _logger;
        private readonly ICosmosProvider _cosmosProvider;

        public Handler(
            ILogger<Handler> logger,
            ICosmosProvider cosmosProvider
            )
        {
            _logger = logger;
            _cosmosProvider = cosmosProvider;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                string sqlQueryText = "";

                if(request.IsOnlySubscriberIds)
                    sqlQueryText = "SELECT c.Email FROM c WHERE c.BroadcastId = '" + request.Id + "'";
                else
                    sqlQueryText = "SELECT * FROM c WHERE c.BroadcastId = '" + request.Id +"'";

                Container cosmosContainer = await _cosmosProvider.GetSurveySubmissionProvider();

                QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
                FeedIterator<Common.Models.SubmitSurvey> queryResultSetIterator = cosmosContainer.GetItemQueryIterator<Common.Models.SubmitSurvey>(queryDefinition);

                List<Common.Models.SubmitSurvey> surveys = new List<Common.Models.SubmitSurvey>();

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Common.Models.SubmitSurvey> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Common.Models.SubmitSurvey survey in currentResultSet)
                    {
                        surveys.Add(survey);
                    }
                }

                return new Response{ Answers = surveys };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}

