using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Create = WAS.Application.Features.Survey.CreateUpdate;
using GetAll = WAS.Application.Features.Survey.GetAll;
using Delete = WAS.Application.Features.Survey.Delete;
using DeleteBroadcasted = WAS.Application.Features.Survey.DeleteBroadcasted;
using GetById = WAS.Application.Features.Survey.GetById;
using GetBroadcastView = WAS.Application.Features.Survey.GetBroadcastView;
using Broadcast = WAS.Application.Features.Survey.Broadcast;
using UpdateBroadcast = WAS.Application.Features.Survey.UpdateBroadcast;
using CheckExpiry = WAS.Application.Features.Survey.CheckExpiry;
using GetAllShareSurvey = WAS.Application.Features.Survey.GetAllSharedSurvey;
using GetByBroadcastId = WAS.Application.Features.Survey.GetByBroadcastId;
using GetAllBroadcast = WAS.Application.Features.Survey.GetAllBroadcast;
using ShareSurvey = WAS.Application.Features.Survey.ShareSurvey;
using CheckAudience = WAS.Application.Features.Survey.CheckAudience;
using GetJsonByBroadcastId = WAS.Application.Features.Survey.GetJsonByBroadcastId;
using SubmitSurvey = WAS.Application.Features.Survey.SubmitSurvey;
using IsAlreadyFilled= WAS.Application.Features.Survey.IsAlreadyFilled;
using GetSubmissionReportByLocation = WAS.Application.Features.Survey.GetSubmissionReportByLocation;
using GetSubmissionReportByDepartment = WAS.Application.Features.Survey.GetSubmissionReportByDepartment;
using GetAnswerwiseReport = WAS.Application.Features.Survey.GetAnswerwiseReport;
using GetAllSharedPeopleNamesById = WAS.Application.Features.Survey.GetAllSharedPeopleNamesById;
using CloneSurvey = WAS.Application.Features.Survey.CloneSurvey;
using GetSubjectByBroadcastId= WAS.Application.Features.Survey.GetSubjectByBroadcastId;
using GetCreatedByList = WAS.Application.Features.Survey.GetCreatedByList;
using ExtractKeyPhrasesFromShortAnswer = WAS.Application.Features.Survey.ExtractKeyPhrasesFromShortAnswer;

namespace WAS.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Survey")]
    public class SurveyController : Controller
    {
        private readonly ILogger<SurveyController> _logger;
        private readonly IMediator _mediator;

        public SurveyController(
            ILogger<SurveyController> logger,
            IMediator mediator
            )
        {
            _logger = logger;
            _mediator = mediator;
        }

        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("CreateUpdateSurvey", Name = "CreateUpdateSurvey")]
        public async Task<ActionResult<Create.Response>> CreateUpdateSurvey([FromBody] Create.Request request)
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
        [HttpPost("GetAllSurvey", Name = "GetAllSurvey")]
        public async Task<ActionResult<GetAll.Response>> GetAllSurvey([FromBody] GetAll.Request request)
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
        [HttpPut("DeleteSurvey", Name = "DeleteSurvey")]
        public async Task<ActionResult<Delete.Response>> DeleteSurvey([FromBody] Delete.Request request)
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
        [HttpGet("GetSurveyById/{Id}", Name = "GetSurveyById")]
        public async Task<ActionResult<GetById.Response>> GetSurveyById([FromRoute] GetById.Request request)
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
        [HttpGet("GetCreatedByList", Name = "GetCreatedByList")]
        public async Task<ActionResult<GetById.Response>> GetCreatedByList([FromRoute] GetCreatedByList.Request request)
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
        [HttpPost("GetBroadcastView", Name = "GetBroadcastView")]
        public async Task<ActionResult<GetBroadcastView.Response>> GetBroadcastView([FromBody] GetBroadcastView.Request request)
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
        [HttpPost("BroadcastSurvey", Name = "BroadcastSurvey")]
        public async Task<ActionResult<Broadcast.Response>> BroadcastSurvey([FromBody] Broadcast.Request request)
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
        [HttpPost("UpdateBroadcastedSurvey", Name = "UpdateBroadcastedSurvey")]
        public async Task<ActionResult<UpdateBroadcast.Response>> UpdateBroadcastedSurvey([FromBody] UpdateBroadcast.Request request)
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
        [HttpGet("CheckSurveyExpiry/{Id}", Name = "CheckSurveyExpiry")]
        public async Task<ActionResult<CheckExpiry.Response>> CheckSurveyExpiry([FromRoute] CheckExpiry.Request request)
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
        [HttpGet("GetByBroadcastId/{Id}/{IgnoreAudienceResponseCount}", Name = "GetByBroadcastId")]
        public async Task<ActionResult<GetByBroadcastId.Response>> GetByBroadcastId([FromRoute] GetByBroadcastId.Request request)
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
        [HttpPost("GetAllBroadcast", Name = "GetAllBroadcast")]
        public async Task<ActionResult<GetAllBroadcast.Response>> GetAllBroadcast([FromBody] GetAllBroadcast.Request request)
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
        [HttpPut("DeleteBroadcastedSurvey", Name = "DeleteBroadcastedSurvey")]
        public async Task<ActionResult<DeleteBroadcasted.Response>> DeleteBroadcastedSurvey([FromBody] DeleteBroadcasted.Request request)
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
        [HttpPost("CheckAudience", Name = "CheckAudience")]
        public async Task<ActionResult<CheckAudience.Response>> CheckAudience([FromBody] CheckAudience.Request request)
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
        [HttpGet("GetJsonByBroadcastId/{Id}", Name = "GetJsonByBroadcastId")]
        public async Task<ActionResult<GetJsonByBroadcastId.Response>> GetJsonByBroadcastId([FromRoute] GetJsonByBroadcastId.Request request)
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
        [HttpGet("GetAllSharedPeopleNamesById/{BroadcastId}", Name = "GetAllSharedPeopleNamesById")]
        public async Task<ActionResult<GetAllSharedPeopleNamesById.Response>> GetAllSharedPeopleNamesById([FromRoute] GetAllSharedPeopleNamesById.Request request)
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
        [HttpPost("SubmitSurvey", Name = "SubmitSurvey")]
        public async Task<ActionResult<SubmitSurvey.Response>> SubmitSurvey([FromBody] SubmitSurvey.Request request)
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
        [HttpPost("ShareSurvey", Name = "ShareSurvey")]
        public async Task<ActionResult<ShareSurvey.Response>> ShareSurvey([FromBody] ShareSurvey.Request request)
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
        [HttpGet("GetAllShareSurvey/{OfficialMail}", Name = "GetAllShareSurvey")]
        public async Task<ActionResult<GetAllShareSurvey.Response>> GetAllShareSurvey([FromRoute] GetAllShareSurvey.Request request)
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
        [HttpGet("IsAlreadyFilled/{Email}/{EmployeeId}/{BroadcastId}", Name = "IsAlreadyFilled")]
        public async Task<ActionResult<IsAlreadyFilled.Response>> IsAlreadyFilled([FromRoute] IsAlreadyFilled.Request request)
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
        [HttpGet("GetSubmissionReportByLocation/{Id}", Name = "GetSubmissionReportByLocation")]
        public async Task<ActionResult<GetSubmissionReportByLocation.Response>> GetSubmissionReportByLocation([FromRoute] GetSubmissionReportByLocation.Request request)
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
        [HttpGet("GetSubmissionReportByDepartment/{Id}", Name = "GetSubmissionReportByDepartment")]
        public async Task<ActionResult<GetSubmissionReportByDepartment.Response>> GetSubmissionReportByDepartment([FromRoute] GetSubmissionReportByDepartment.Request request)
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
        [HttpGet("GetAnswerwiseReport/{Id}", Name = "GetAnswerwiseReport")]
        public async Task<ActionResult<GetAnswerwiseReport.Response>> GetAnswerwiseReport([FromRoute] GetAnswerwiseReport.Request request)
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
        [HttpPost("CloneSurvey", Name = "CloneSurvey")]
        public async Task<ActionResult<CloneSurvey.Response>> CloneSurvey([FromBody] CloneSurvey.Request request)
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
        [HttpGet("GetSubjectByBroadcastId/{Id}", Name = "GetSubjectByBroadcastId")]
        public async Task<ActionResult<GetSubjectByBroadcastId.Response>> GetSubjectByBroadcastId([FromRoute] GetSubjectByBroadcastId.Request request)
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
        [HttpGet("ExtractKeyPhrasesFromShortAnswer/{Id}", Name = "ExtractKeyPhrasesFromShortAnswer")]
        public async Task<ActionResult<ExtractKeyPhrasesFromShortAnswer.Response>> ExtractKeyPhrasesFromShortAnswer([FromRoute] ExtractKeyPhrasesFromShortAnswer.Request request)
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
