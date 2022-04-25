using Microsoft.Azure.Cosmos;
using Microsoft.Graph.Auth;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface ICosmosProvider
    {
        Task<Container> GetSurveySubmissionProvider();
    }
}
