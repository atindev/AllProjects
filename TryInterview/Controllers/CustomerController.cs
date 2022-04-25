using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TryInterview.BlLayer;

namespace TryInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomers customer;

        public CustomerController(ICustomers customer)
        {
            this.customer = customer;
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var data = customer.GetCustomers();
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
