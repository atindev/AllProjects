using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Create = WAS.Application.Features.Subscription.Create;
using ConversationSubscription = WAS.Application.Features.Subscription.ConversationSubscription;
using OcrSubscriptionData = WAS.Application.Features.Subscription.OcrSubscriptionData;
using WAS.Application.Common.Models;
using Unsubscribe = WAS.Application.Features.Subscription.Unsubscribe;
using GetByMail = WAS.Application.Features.Subscription.GetByMail;
using GetMaskedByMail = WAS.Application.Features.Subscription.GetByMailUnMasked;
using Microsoft.AspNetCore.Authorization;
using GetAll = WAS.Application.Features.Subscription.GetAll;
using GetAllEmployeeType = WAS.Application.Features.Subscription.GetAllEmployeeType;
using GetAllEmployeeGroup = WAS.Application.Features.Subscription.GetAllEmployeeGroup;
using GetAllSubscribedOn = WAS.Application.Features.Subscription.GetAllSubscribedOn;
using GetAllJobTitle = WAS.Application.Features.Subscription.GetAllJobTitle;
using UnsubscribeEmail = WAS.Application.Features.Subscription.UnsubscribeEmail;
using GetMaskedById = WAS.Application.Features.Subscription.GetById;
using AutoMapper;
using Delete = WAS.Application.Features.Subscription.Delete;
using WAS.Application.Interface.Services;
using UnsubscribeMobile = WAS.Application.Features.Subscription.UnsubscribeMobile;
using SubscriptionFeedback = WAS.Application.Features.Subscription.SubscriptionFeedback;
using BlockUser = WAS.Application.Features.Subscription.BlockUser;
using BlockUserCheck = WAS.Application.Features.Subscription.IsBlockedUser;
using GetOcrSubscriptionList = WAS.Application.Features.Subscription.GetOcrSubscriptionList;
using GetOcrSubscriptionById = WAS.Application.Features.Subscription.GetOcrSubscriptionById;
using DiscardOcrSubscription = WAS.Application.Features.Subscription.DiscardOcrSubscription;

namespace WAS.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IGraphService _graphService;

        /// <summary>
        /// Create new SubscriptionController
        /// </summary>
        /// <param name="mediator"></param>
        public SubscriptionController(
            ILogger<SubscriptionController> logger,
            IMapper mapper,
            IMediator mediator,
            IGraphService graphService)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _graphService = graphService;
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("", Name = "CreateSubscription")]
        [AllowAnonymous]
        public async Task<ActionResult<ConversationSubscription.Response>> CreateSubscription([FromBody] ConversationSubscription.Request request)
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
        [HttpPost("CreateConversationSubscription", Name = "CreateConversationSubscription")]
        public async Task<ActionResult<ConversationSubscription.Response>> CreateConversationSubscription([FromBody] ConversationSubscriptionData conversationSubscriptionData)
        {
            try
            {
                var userResponse = await _graphService.GetUser(conversationSubscriptionData.Upn);
                _mapper.Map(userResponse, conversationSubscriptionData);
                var request = _mapper.Map<ConversationSubscription.Request>(conversationSubscriptionData);

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
        [HttpPost("CreateOcrSubscriptionData", Name = "CreateOcrSubscriptionData")]
        public async Task<ActionResult<ConversationSubscription.Response>> CreateOcrSubscriptionData([FromBody] OcrSubscriptionData.Request request)
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
        [HttpGet("GetByMail/{email}", Name = "GetById")]
        public async Task<ActionResult<GetByMail.Response>> GetById([FromRoute] GetByMail.Request request)
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
        [HttpDelete("Unsubscribe/{OfficialEmail}", Name = "Unsubscribe")]
        public async Task<ActionResult<Unsubscribe.Response>> Unsubscribe([FromRoute] Unsubscribe.Request request)
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
        [HttpPost("GetSubscriptions", Name = "GetSubscriptions")]
        public async Task<ActionResult<GetAll.Response>> GetSubscriptions([FromBody] GetAll.Request request)
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
        [HttpPut("UnsubscribeEmail", Name = "UnsubscribeEmail")]
        public async Task<ActionResult<UnsubscribeEmail.Response>> VerifyMailOtp([FromBody] UnsubscribeEmail.Request request)
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
        [HttpGet("GetByMailUnMasked/{email}", Name = "GetByMailUnMasked")]
        public async Task<ActionResult<GetMaskedByMail.Response>> GetByMailUnMasked([FromRoute] GetMaskedByMail.Request request)
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
        [HttpGet("GetSubscriberById/{id}", Name = "GetSubscriberById")]
        public async Task<ActionResult<GetMaskedById.Response>> GetBySubscriberId([FromRoute] GetMaskedById.Request request)
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
        [HttpDelete("DeleteSubscription/{OfficialEmail}", Name = "DeleteSubscription")]
        public async Task<ActionResult<Delete.Response>> DeleteSubscription([FromRoute] Delete.Request request)
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
        [HttpGet("GetAllEmployeeType", Name = "GetAllEmployeeType")]
        public async Task<ActionResult<GetAllEmployeeType.Response>> GetAllEmployeeType()
        {
            try
            {
                var response = await _mediator.Send(new GetAllEmployeeType.Request());

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
        [HttpGet("GetAllEmployeeGroup", Name = "GetAllEmployeeGroup")]
        public async Task<ActionResult<GetAllEmployeeGroup.Response>> GetAllEmployeeGroup()
        {
            try
            {
                var response = await _mediator.Send(new GetAllEmployeeGroup.Request());

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
        [HttpGet("GetAllJobTitle", Name = "GetAllJobTitle")]
        public async Task<ActionResult<GetAllJobTitle.Response>> GetAllJobTitle()
        {
            try
            {
                var response = await _mediator.Send(new GetAllJobTitle.Request());

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
        [HttpGet("GetAllSubscribedOn", Name = "GetAllSubscribedOn")]
        public async Task<ActionResult<GetAllSubscribedOn.Response>> GetAllSubscribedOn()
        {
            try
            {
                var response = await _mediator.Send(new GetAllSubscribedOn.Request());

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
        [HttpPost("UnsubscribeMobile", Name = "UnsubscribeMobile")]
        public async Task<ActionResult<UnsubscribeMobile.Response>> UnsubscribeMobile([FromBody] UnsubscribeMobile.Request request)
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
        [HttpGet("GetUserDetails", Name = "GetAdUserDetails")]
        public async Task<ActionResult<User>> GetAdUserDetails(string emailId)
        {
            try
            {
                User userResponse = new User();
                if (emailId != "")
                {
                    var response = await _graphService.GetUser(emailId);

                    if (response == null)
                        return NotFound();

                    userResponse.FirstName = response.FirstName;
                    userResponse.LastName = response.LastName;
                    userResponse.UserPrincipalName = response.UserPrincipalName;
                }

                return Ok(userResponse);
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
        [HttpPost("SubscriptionFeedback", Name = "SubscriptionFeedback")]
        public async Task<ActionResult<SubscriptionFeedback.Response>> SubscriptionFeedback([FromBody] SubscriptionFeedback.Request request)
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
        [HttpPost("BlockUser", Name = "BlockUser")]
        [AllowAnonymous]
        public async Task<ActionResult<BlockUser.Response>> BlockUser([FromBody] BlockUser.Request request)
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
        [HttpGet("CheckforBlockedUser/{EmailorEmployeeId}", Name = "CheckforBlockedUser")]
        public async Task<ActionResult<BlockUserCheck.Response>> CheckforBlockedUser([FromRoute] BlockUserCheck.Request request)
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
        [HttpGet("GetOcrSubscriptionList", Name = "GetOcrSubscriptionList")]
        public async Task<ActionResult<GetOcrSubscriptionList.Response>> GetOcrSubscriptionList([FromQuery] GetOcrSubscriptionList.Request request)
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
        [HttpGet("GetOcrSubscriptionById/{Id}", Name = "GetOcrSubscriptionById")]
        public async Task<ActionResult<GetOcrSubscriptionById.Response>> GetOcrSubscriptionById([FromRoute] GetOcrSubscriptionById.Request request)
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
        [HttpDelete("DiscardOcrSubscription/{Id}", Name = "DiscardOcrSubscription")]
        public async Task<ActionResult<DiscardOcrSubscription.Response>> DiscardOcrSubscription([FromRoute] DiscardOcrSubscription.Request request)
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
