using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication1.BL;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesmanController : ControllerBase
    {
        private readonly SalesmanBlv2 salesmanBl;

        public SalesmanController(SalesmanBlv2 _salesmanBl)
        {
            salesmanBl = _salesmanBl;
        }

        [HttpGet("GetSalesman")]
        public IActionResult GetAllSalesman()
        {
            try
            {
                return this.Ok(salesmanBl.GetAllSalesman());
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("GetSalesmanOrders")]
        public IActionResult GetAllSalesmanwithOrders()
        {
            try
            {
                return this.Ok(salesmanBl.GetAllSalesmanwithOrders());
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("GetSalesman/{Id}")]
        public IActionResult GetSpecificSalesman(int Id)
        {
            Salesman sal = salesmanBl.GetSpecificSalesman(Id);
            if (sal != null)
                return this.Ok(sal);
            else
                return this.NotFound(sal);
        }

        [HttpDelete("DeleteSalesman/{Id}")]
        public IActionResult DeleteSpecificSalesman(int Id)
        {
            bool sal = salesmanBl.DeleteSpecificSalesman(Id);
            if (sal)
            {
                return this.Ok("Deleted");
            }
            else
                return this.NotFound();
        }

        [HttpPost("CreateSalesman")]
        public IActionResult CreateSalesman([FromBody] Salesman sal)
        {
            Salesman sal1 = salesmanBl.CreateSalesman(sal);
            if (sal1 != null)
            {
                return this.Created("", sal);
            }
            else
                return this.NoContent();
        }

        [HttpPut("UpdateSalesman")]
        public IActionResult updateSalesman([FromBody] Salesman sal1)
        {
            Salesman sal = salesmanBl.updateSalesman(sal1);
            if (sal != null)
            {
                return this.Ok(sal1);
            }
            else
                return this.NoContent();
        }

        [HttpGet("Linq123")]
        public IActionResult Linq123()
        {
            var data = salesmanBl.Linq123();

            if (data?.Any() == true)
            {
                return this.Ok(data);
            }
            else
                return this.NoContent();
        }
    }
}
