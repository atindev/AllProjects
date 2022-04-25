using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using GetDashboard = WAS.Application.Features.Dashboard.GetDashboard;

namespace WAS.API.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Create new DashboardController
        /// </summary>
        /// <param name="mediator"></param>
        public DashboardController(
            ILogger<DashboardController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetDashboard/{PageIndex}/{RowCount}/{PageType}/{Email}/{IsGlobalAdmin}", Name = "GetDashboard")]
        public async Task<ActionResult<GetDashboard.Response>> GetDashboard([FromRoute] GetDashboard.Request request)
        {
            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest();
            }
        }
        
         
    }
}
