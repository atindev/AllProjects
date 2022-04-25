using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using UserFeedback = WAS.Application.Features.Feedback;

namespace WAS.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Create new Feedback Controller
        /// </summary>
        /// <param name="mediator"></param>
        public FeedbackController(
            ILogger<EventController> logger,
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
        [HttpPost("SubmitFeedback", Name = "SubmitFeedback")]
        public async Task<ActionResult<UserFeedback.Response>> SubmitFeedback([FromBody] UserFeedback.Request request)
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
