using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SignalRTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet("Test")]
        public async Task<IActionResult> OrderReady()
        {
            OrderHub orderHub = new OrderHub();
            await orderHub.SendMessage("ATIN", "HAHA");
            return this.Ok();
        }
    }
}