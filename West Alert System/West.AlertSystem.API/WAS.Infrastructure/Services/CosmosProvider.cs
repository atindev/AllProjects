using Microsoft.Extensions.Options;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Cosmos;
using WAS.Application.Common.Settings;
using System.Threading.Tasks;

namespace WAS.Infrastructure.Services
{
    public class CosmosProvider : ICosmosProvider
    {
        private readonly IOptions<CosmosSettings> _CosmosSettings;
            
        public CosmosProvider(IOptions<CosmosSettings> options)
        {
            _CosmosSettings = options;
        }
        public async Task<Container> GetSurveySubmissionProvider()
        {
            // The Azure Cosmos DB endpoint for running this sample.
             string EndpointUri = _CosmosSettings.Value.EndpointUri;
            // The primary key for the Azure Cosmos account.
            string PrimaryKey = _CosmosSettings.Value.PrimaryKey;
            // The name of the database and container we will create
            string databaseId = _CosmosSettings.Value.SurveyDatabaseId;
            string containerId = _CosmosSettings.Value.SurveySubmissionContainer;

            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey, new CosmosClientOptions() { ApplicationName = _CosmosSettings.Value.ApplicationName });

            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);

            Container container = await database.CreateContainerIfNotExistsAsync(containerId, "/BroadcastId");

            return container;
        }

    }
}
