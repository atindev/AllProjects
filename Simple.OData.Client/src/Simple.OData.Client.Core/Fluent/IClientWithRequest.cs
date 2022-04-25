using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.OData.Client.WestInternal
{
    public interface IClientWithRequest<T>
    {
        ODataRequest GetRequest();
        IClientWithResponse<T> FromResponse(HttpResponseMessage responseMessage);

        Task<IClientWithResponse<T>> RunAsync();
        Task<IClientWithResponse<T>> RunAsync(CancellationToken cancellationToken);
    }
}
