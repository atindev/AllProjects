using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;
using GetAll = WAS.Application.Features.IncomingMessage.GetAll;
using GetAudio = WAS.Application.Features.IncomingMessage.GetAudio;
using View = WAS.Application.Features.IncomingMessage.View;
using GetAllGroups = WAS.Application.Features.Groups.GetAll;
using System.Security.Claims;

namespace WAS.Web.Controllers
{
    [Authorize]
    [Route("WAS/IncomingMessage")]
    public class IncomingMessageController : Controller
    {
        private readonly IMediator _mediator;

        public IncomingMessageController(
           IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("List")]
        public async Task<IActionResult> List(string source)
        {
            var response = await _mediator.Send(new GetAll.Request());
            bool IsGlobalAdmin = User.HasClaim(ClaimTypes.Role, "GlobalAdministrator");
            var responseGroup = await _mediator.Send(new GetAllGroups.Request() { IsAccessRequired = true, Email = User.Identity.Name , IsGlobalAdmin=IsGlobalAdmin });
            response.GroupList = responseGroup;
            if (source != null && source == "IncomingCallTab")
            {
                ViewBag.source = "IncomingCallTab";
            }
            return View(response);
        }

        [Route("GetAudio")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetAudio(string path)
        {
            var response = await _mediator.Send(new GetAudio.Request { Path = path });
            Response.Headers.Add(HeaderNames.ContentDisposition, response.ContentDisposition.ToString());
            return File(Convert.FromBase64String(response.Content), "audio/wav");
        }

        [Route("View")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> View(Guid incomingMessageId, string requestFrom)
        {
            var response = await _mediator.Send(new View.Request { Id = incomingMessageId });
            if (requestFrom == null || requestFrom == "")
                ViewBag.value = "incomingMessage";
            else
                ViewBag.value = requestFrom;

            return View(response);

        }
    }
}
