using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TryInterview.BlLayer;

namespace TryInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjects projects;

        public ProjectsController(IProjects projects)
        {
            this.projects = projects;
        }

        [HttpGet("GetProjects")]
        public IActionResult GetProjects()
        {
            var data = projects.GetProjects();
            if (data?.Any() == true)
            {
                return this.Ok(data);
            }
            else
            {
                return this.NoContent();
            }
        }

        [HttpGet("GetProjectsBillingData")]
        public IActionResult GetProjectsBillingData()
        {
            var data = projects.GetProjectsBillingData();
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
