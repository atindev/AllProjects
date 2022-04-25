using InvestingCompany.BL;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace InvestingCompany.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IBLayer bLayer;

        public CompanyController(IBLayer _bLayer)
        {
            bLayer = _bLayer;
        }

        [HttpGet("GetCompanyLevelDetails")]
        public IActionResult GetCompanyLevelDetails()
        {
            var data = bLayer.GetCompanyLevelDetails();
            if (data?.Any() == true)
            {
                return this.Ok(data);
            }
            else
            {
                return this.NotFound("No Data");
            }
        }

        [HttpGet("GetCompanyUserLevelDetails/{CompanyId}")]
        public IActionResult GetCompanyUserLevelDetails(int CompanyId)
        {
            var data = bLayer.GetCompanyUserLevelDetails(CompanyId);
            if (data?.Any() == true)
            {
                return this.Ok(data);
            }
            else
            {
                return this.NotFound("No Data");
            }
        }
    }
}
