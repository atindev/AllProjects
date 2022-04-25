using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GetAll= WAS.Application.Features.Events.GetAll;
using GetEventType = WAS.Application.Features.Events.GetTypeAndUrgency;
using View = WAS.Application.Features.Events.View;
using CreateUpdate= WAS.Application.Features.Events.CreateUpdate;
using Archive = WAS.Application.Features.Events.Archive;
using WAS.Web.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using GetGroupsAll = WAS.Application.Features.Groups.GetAll;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WAS.Web.Controllers
{
    [Authorize] 
    [Route("WAS/Event")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly bool IsGlobalAdmin;

        public EventController(
           IMediator mediator,
           IMapper mapper,
           INotyfService notyf,
           IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _notyf = notyf;
            IsGlobalAdmin = httpContextAccessor.HttpContext.User.HasClaim(ClaimTypes.Role, "GlobalAdministrator");
        }

        [Route("List")]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetAll.Request());
            var responseGroup = await _mediator.Send(new GetGroupsAll.Request() { IsAccessRequired = true, Email = User.Identity.Name , IsGlobalAdmin= IsGlobalAdmin });          
            response.GroupList = responseGroup;
            TempData["EventNavigatedFrom"] = "/WAS/Event/List";
            TempData["BackButtonTitle"] = "Back to event list";
            return View(response);
        }

        [Route("View")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> View(Guid eventId)
        {
            var response = await _mediator.Send(new View.Request { Id = eventId, Email= User.Identity.Name, IsGlobalAdmin=IsGlobalAdmin});
            return View(response);
        }

        [HttpPost]
        [Route("CreateUpdate")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> CreateorUpdateEvents(EventViewModel eventViewModel)
        {
            if (eventViewModel.Id == Guid.Empty && eventViewModel.CreatedBy == null)
                eventViewModel.CreatedBy = User.Identity.Name;
            var request = _mapper.Map<CreateUpdate.Request>(eventViewModel);
            var response= await _mediator.Send(request);

            if (eventViewModel.PageType == "EventDetails")
            {
                if (response.IsNameExist)
                    _notyf.Warning("Event name already exist");
                else
                    _notyf.Success("Event details updated successfully");
                return RedirectToAction("View", new { eventId = eventViewModel.Id });
            }
            else
            {
                if (response.IsNameExist)
                    _notyf.Warning("Event name already exist");
                else
                    _notyf.Success("Event created successfully");
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        [Route("Archive")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> Archive(EventArchiveModel archiveModel)
        {
            var request = _mapper.Map<Archive.Request>(archiveModel);
            await _mediator.Send(request);
            _notyf.Success("Event Archived successfully");
            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("GetTypeAndUrgency")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetTypeAndUrgency()
        {
            var response = await _mediator.Send(new GetEventType.Request());
            return Json(response);
        }

        [HttpPost]
        [Route("AddEvent")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> AddEvent([FromBody] EventViewModel eventViewModel)
        {
            if (eventViewModel.Id == Guid.Empty && eventViewModel.CreatedBy == null)
                eventViewModel.CreatedBy = User.Identity.Name;

            var request = _mapper.Map<CreateUpdate.Request>(eventViewModel);
            var response=await _mediator.Send(request);
            if (response.IsNameExist)
                _notyf.Warning("Event name already exist");
            else
            {
                if(eventViewModel.Id==Guid.Empty)
                    _notyf.Success("Event created successfully");
                else
                    _notyf.Success("Event details updated successfully");
            }
            return Json(response);
        }
    }
}
