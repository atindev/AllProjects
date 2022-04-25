using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Create = WAS.Application.Features.Notification.Create;
using GetByStatus = WAS.Application.Features.Notification.GetByStatus;
using GetById = WAS.Application.Features.Notification.GetById;
using ViewAttachment = WAS.Application.Features.Notification.ViewAttachment;
using ApproveReject = WAS.Application.Features.Notification.ApproveReject;
using GetCount = WAS.Application.Features.Notification.GetCount;
using GetPaged = WAS.Application.Features.Notification.GetAll;
using GetDeliveryStatus = WAS.Application.Features.Notification.DeliveryReport;
using GetWhatAppTemplates = WAS.Application.Features.Notification.GetWhatsAppTemplate;
using GetFailedNotification = WAS.Application.Features.Notification.GetFailedDetails;
using WAS.Domain.Enum;

namespace WAS.API.Controllers
{
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Notification")]
    public class NotificationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            IMediator mediator,
            ILogger<NotificationController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("", Name = "CreateNotification")]
        public async Task<ActionResult<Create.Response>> CreateNotification([FromBody] Create.Request request)
        {
            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                    return NotFound();

                return Created("", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest();
            }
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetByStatus/{Status}/{Email}/{IsOnlyPrivateApprover}/{IsGlobalAdmin}/{HavingBothApprovalLevel}", Name = "GetNotificationByStatus")]
        public async Task<ActionResult<GetByStatus.Response>> GetByStatus([FromRoute] GetByStatus.Request request)
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

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetById/{Id}", Name = "GetNotificationById")]
        public async Task<ActionResult<GetById.Response>> GetById([FromRoute] GetById.Request request)
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

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("ApproveReject", Name = "ApproveReject")]
        public async Task<ActionResult<ApproveReject.Response>> ApproveReject([FromBody] ApproveReject.Request request)
        {
            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                    return NotFound();

                return Created("", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest();
            }
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetNotificationCount", Name = "GetNotificationCount")]
        public async Task<ActionResult<GetCount.Response>> GetCount()
        {
            try
            {
                var response = await _mediator.Send(new GetCount.Request());

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

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("GetPagedNotification", Name = "GetPagedNotification")]
        public async Task<ActionResult<GetPaged.Response>> GetPagedNotification([FromBody] GetPaged.Request request)
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


        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetAttachment", Name = "GetAttachment")]
        public async Task<ActionResult<ViewAttachment.Response>> GetAttachment([FromQuery] ViewAttachment.Request request)
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

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetLatestDeliveryStatus/{Id}", Name = "GetLatestDeliveryStatus")]
        public async Task<ActionResult<GetDeliveryStatus.Response>> GetLatestDeliveryStatus([FromRoute] GetDeliveryStatus.Request request)
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

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetWhatsAppTemplates", Name = "GetWhatsAppTemplates")]
        public async Task<ActionResult<GetWhatAppTemplates.Response>> GetAllTemplates()
        {
            try
            {
                var response = await _mediator.Send(new GetWhatAppTemplates.Request());

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


        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetFailedNotificationDetails/{Id}/{PublishingType}/{Status}", Name = "GetFailedNotificationDetails")]
        public async Task<ActionResult<GetFailedNotification.Response>> GetFailedNotificationDetails([FromRoute] GetFailedNotification.Request request)
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
