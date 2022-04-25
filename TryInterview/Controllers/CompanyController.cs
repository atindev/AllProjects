using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TryInterview.BlLayer;

namespace TryInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanies company;

        public CompanyController(ICompanies company)
        {
            this.company = company;
        }

        [HttpGet("GetCompanies")]
        public IActionResult GetCompanies()
        {
            var data = company.GetCompanies();
            if (data?.Any() == true)
            {
                return this.Ok(data);
            }
            else
            {
                return this.NoContent();
            }
        }

    }
}
