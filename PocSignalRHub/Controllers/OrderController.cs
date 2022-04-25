using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace PocSignalRHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHubContext<OrderHub> orderContext;
        public OrderController(IHubContext<OrderHub> _orderContext)
        {
            orderContext = _orderContext;
        }

        [HttpGet("OrderReady/{equimentId}")]
        public async Task OrderReady(Guid equimentId)
        {
            await orderContext.Clients.Group(equimentId.ToString()).SendAsync(Constants.ReceiveMessage, Constants.OrderReady);
        }
    }
}