using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using OdataRetry.Middleware.Routing;
using System.Linq;
using System.Threading.Tasks;
using West.Manufacturing.Common.Enterprise.Models;
using West.Manufacturing.Repository.Interfaces;

namespace OdataRetry.Controllers
{
    [Produces("application/json")]
    [EnableQuery]
    [GenericControllerName]
    public class ManufacturingController<T> : ODataController where T : EnterpriseModel
    {
        protected IEnterpriseRepository<T> Repository { get; set; }
        public ManufacturingController(IEnterpriseRepository<T> Repo)
        {
            Repository = Repo;
        }

        [HttpGet]
        public async Task<IQueryable<T>> Get()
        {
            return await Repository.GetItemsAsync(c => c.Id != null);
        }
    }
}