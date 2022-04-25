using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WAS.Web.Models;
using Model = WAS.Application.Common.Models;
using CreateNotification = WAS.Application.Features.Notification.Create;
using GetGroups = WAS.Application.Features.Notification.CreateView;
using List = WAS.Application.Features.Notification.GetByStatus;
using ApproveReject = WAS.Application.Features.Notification.ApproveReject;
using View = WAS.Application.Features.Notification.View;
using ViewAttachment = WAS.Application.Features.Notification.ViewAttachment;
using GetActiveEvents = WAS.Application.Features.Events.GetAtive;
using GetDistinctSubscribers = WAS.Application.Features.Groups.GetDistinctSubscriberCount;
using GetDeliveryStatus = WAS.Application.Features.Notification.DeliveryReport;
using GetWhatsAppTemplates = WAS.Application.Features.Notification.GetWhatsAppTemplate;
using CreateTemplate = WAS.Application.Features.Template.Create;
using GetTemplateById = WAS.Application.Features.Template.GetById;
using GetAllTemplates = WAS.Application.Features.Template.GetAll;
using GetAllTemplateCategories = WAS.Application.Features.Template.GetAllCategories;
using GetFailedNotifications = WAS.Application.Features.Notification.GetFailedDetails;
using WAS.Application.Features.Notification.GetPaged;
using System.Linq;
using System.IO;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using GetAll = WAS.Application.Features.Groups.GetAll;

namespace WAS.Web.Controllers
{
    [Authorize]
    [Route("WAS/Notification")]
    public class NotificationController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly bool IsGlobalAdmin;

        public NotificationController(
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

        [Route("Create")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> Index(int groupId)
        {
            if (groupId != 0)
            {
                ViewBag.groupId = groupId;
            }
            var response = await _mediator.Send(new GetGroups.Request() { Email = User.Identity.Name, IsGlobalAdmin=IsGlobalAdmin }) ;           
            return View(response);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> Create(NotificationViewModel viewModel)
        {
            if (viewModel.EventType == "New")
                viewModel.EventId = Guid.Empty;

            if (viewModel.TimeZone == null)
            {
                viewModel.TimeZone = viewModel.CreatedTimeZone;
                viewModel.ScheduledTime = DateTime.UtcNow;
            }
            else
            {
                viewModel.ScheduledTime = viewModel.ScheduledTime?.AddMinutes(viewModel.TimeZoneOffset);
            }

            var request = _mapper.Map<CreateNotification.Request>(viewModel);
            request.CreatedBy = User.Identity.Name;

            viewModel.EmailAttachment.ForEach(emailAttachment =>
            {
                if (emailAttachment.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    emailAttachment.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    request.EmailAttachments.Add(new Model.AttachmentData
                    {
                        FileName = emailAttachment.FileName,
                        Content = Convert.ToBase64String(fileBytes),
                        ContentType = emailAttachment.ContentType
                    });
                }
            });

            if(request.IsApprovalRequired)
                request.Status = Application.Common.Enum.Status.Submitted;
            else
                request.Status = Application.Common.Enum.Status.SecondLevelApproved;

            var response = await _mediator.Send(request);
            _notyf.Success("Notification created successfully");
            return RedirectToAction("View", new { NotificationId = response.Id });
        }

        [Route("Review")]
        [Authorize(Policy = "CommunicationTeam")]
        public async Task<IActionResult> Review(Guid NotificationId)
        {
            var response = await _mediator.Send(new View.Request { Id = NotificationId });
            return View(response);
        }

        [Route("View")]
        [Authorize(Policy = "WASAdmin")]
#pragma warning disable S4144 // Methods should not have identical implementations
        public async Task<IActionResult> View(Guid NotificationId, string RequestFrom)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var response = await _mediator.Send(new View.Request { Id = NotificationId });
            if (RequestFrom == null || RequestFrom == "")
                ViewBag.value = "notification";
            else
                ViewBag.value = RequestFrom;

            return View(response);

        }

        [HttpGet]
        [Route("GetNotificationById/{NotificationId}")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetNotificationById(Guid NotificationId)
        {
            var response = await _mediator.Send(new View.Request { Id = NotificationId });
            int i = 0;
            string typeClass = "active";
            string contentClass = "active show";
            string areaSelected = "true";

            ViewData.Add(new KeyValuePair<string, object>
                ("i", i));
            ViewData.Add(new KeyValuePair<string, object>
                ("typeClass", typeClass));
            ViewData.Add(new KeyValuePair<string, object>
                ("areaSelected", areaSelected));
            ViewData.Add(new KeyValuePair<string, object>
                ("contentClass", contentClass));
            return PartialView("~/Views/Event/_Notification.cshtml", response);
        }

        [HttpPost]
        [Route("Approve")]
        [Authorize(Policy = "CommunicationTeam")]
        public async Task<IActionResult> Approve(ApproveReject.Request request)
        {
            if (request.ApprovalLevel == Application.Common.Enum.ApprovalLevel.First)
                request.Status = Application.Common.Enum.Status.FirstLevelApproved;
            else
                request.Status = Application.Common.Enum.Status.SecondLevelApproved;
            request.ApprovedBy = User.Identity.Name;
            await _mediator.Send(request);
            _notyf.Success("Notification Approved");
            return RedirectToAction("ListSubmitted");
        }

        [HttpPost]
        [Route("Reject")]
        [Authorize(Policy = "CommunicationTeam")]
        public async Task<IActionResult> Reject(ApproveReject.Request request)
        {
            if (request.ApprovalLevel == Application.Common.Enum.ApprovalLevel.First)
                request.Status = Application.Common.Enum.Status.FirstLevelRejected;
            else
                request.Status = Application.Common.Enum.Status.SecondLevelRejected;
            request.ApprovedBy = User.Identity.Name;
            await _mediator.Send(request);
            _notyf.Error("Notification Rejected");
            return RedirectToAction("ListSubmitted");
        }

        [Route("ListSubmitted")]
        [Authorize(Policy = "CommunicationTeam")]
        public async Task<IActionResult> ListSubmitted()
        {
            List.Response response;

            var request = new List.Request();
            request.IsOnlyPrivateApprover = true;
            request.Email = User.Identity.Name;
            request.IsGlobalAdmin = IsGlobalAdmin;
            request.HavingBothApprovalLevel = false;

            if (User.HasClaim(ClaimTypes.Role, "Approver"))
            {
                request.Status = Application.Common.Enum.Status.Submitted;
                request.IsOnlyPrivateApprover = false;
            }
            else if (User.HasClaim(ClaimTypes.Role, "CommunicationTeam"))
            {
                request.Status = Application.Common.Enum.Status.FirstLevelApproved;
                request.IsOnlyPrivateApprover = false;
            }
            else
            {
                request.Status = Application.Common.Enum.Status.FirstLevelApproved;
            }

            if(User.HasClaim(ClaimTypes.Role, "Approver") && User.HasClaim(ClaimTypes.Role, "CommunicationTeam"))
                request.HavingBothApprovalLevel = true;

            response = await _mediator.Send(request);
            var responseGroup = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name , IsGlobalAdmin= IsGlobalAdmin });
            response.GroupList = responseGroup;
            return View(response);
        }

        [Route("List")]
        public async Task<IActionResult> List()
        {
            var response = await _mediator.Send(new GetActiveEvents.Request());
            var responseGroup = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name, IsGlobalAdmin = IsGlobalAdmin });
            response.GroupList = responseGroup;
            TempData["EventNavigatedFrom"]="/WAS/Notification/List";
            TempData["BackButtonTitle"]="Back to notification list";
            return View(response);
        }

        [HttpPost]
        [Route("GetPages")]
        public async Task<object> GetPagesAsync([FromBody] FilterRequest filterRequest)
        {

            Request request = new Request();
            request.PageType = "Paged";
            request.PageIndex = filterRequest.Skip;
            request.RowCount = (filterRequest.Take == 0 ? 7 : filterRequest.Take);
            request.Email = User.Identity.Name;
            request.IsGlobalAdmin = IsGlobalAdmin;

            if (filterRequest.Params != null && filterRequest.Params.Count > 0)
            {
                string EventFilter = null, StatusFilter = null, MessageFilter = null;

                if (filterRequest.Params.TryGetValue("EventFilter", out EventFilter) && EventFilter != "")
                {
                    request.EventFilter = EventFilter;
                }

                if (filterRequest.Params.TryGetValue("StatusFilter", out StatusFilter) && StatusFilter != "")
                {
                    request.StatusFilter = (Application.Common.Enum.Status)Convert.ToInt16(StatusFilter);
                }

                if (filterRequest.Params.TryGetValue("MessageFilter", out MessageFilter) && MessageFilter != "")
                {
                    request.MessageFilter = MessageFilter;
                }

            }
            var response = await _mediator.Send(request);
            return Json(new { result = response.RecentNotifications.ToList(), count = response.Count });
        }

        public async Task<IActionResult> ViewAttachment(ViewAttachment.Request request)
        {
            var response = await _mediator.Send(request);

            Response.Headers.Add(HeaderNames.ContentDisposition, response.ContentDisposition.ToString());
            return File(Convert.FromBase64String(response.Content), response.ContentType);
        }

        [Route("EmailView")]
        [AllowAnonymous]
        public async Task<IActionResult> ViewAnonymous(string NotificationId)
        {
            var response = await _mediator.Send(new View.Request { Id = Guid.Parse(NotificationId) });
            return View(response);
        }

        [HttpPost]
        [Route("GetSubscriptionCount")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetSubscriptionCount([FromBody] GroupModel groupList)
        {
            var response = await _mediator.Send(_mapper.Map<GetDistinctSubscribers.Request>(groupList));
            return Json(response);
        }


        [HttpGet]
        [Route("getlatestdeliverystatus")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetLatestDeliveryStatus(Guid notificationId)
        {
            var response = await _mediator.Send(new GetDeliveryStatus.Request{ Id = notificationId});
            return Ok(response);
        }

        [HttpGet]
        [Route("getwhatsapptemplate")]
        public async Task<IActionResult> GetWhatsAppTemplate(int id)
        {
            var response = await _mediator.Send(new GetWhatsAppTemplates.Request());

            if (response.WhatsAppTemplates != null && response.WhatsAppTemplates.Count > 0)
            {
                var template = response.WhatsAppTemplates.SingleOrDefault(t => t.Id == id);
                return Ok(template);
            }

            return null;
        }

       
        [HttpPost]
        [Route("CreateTemplate")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> CreateTemplate(string data, List<IFormFile> files)
        {
            var request = JsonConvert.DeserializeObject<CreateTemplate.Request>(data);

            request.EmailAttachments = new List<Model.AttachmentData>();
             
            files.ForEach(emailAttachment =>
            {
                if (emailAttachment.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    emailAttachment.CopyTo(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    request.EmailAttachments.Add(new Model.AttachmentData
                    {
                        FileName = emailAttachment.FileName,
                        Content = Convert.ToBase64String(fileBytes),
                        ContentType = emailAttachment.ContentType
                    });
                }
            });

            var response =  await _mediator.Send(request);
            _notyf.Success("Template Created successfully");
            return Json(response);
        }

        [HttpPost]
        [Route("GetTemplateById")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetTemplateById(string Id)
        {
            Guid templateId = Guid.Parse(Id);
            var response = await _mediator.Send(new GetTemplateById.Request { Id = templateId });
            return Json(response.TemplateContent);

        }

        [HttpPost]
        [Route("GetAllTemplates")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetAllTemplates()
        {
            var response = await _mediator.Send(new GetAllTemplates.Request());
            return PartialView("~/Views/Notification/_TemplateContent.cshtml", response);
        }

        [HttpPost]
        [Route("GetTemplateCategories")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetTemplateCategories()
        {
            var response = await _mediator.Send(new GetAllTemplateCategories.Request());
            return Json(response);
        }

        [HttpPost]
        [Route("GetFailedNotifications")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetFailedNotifications([FromBody] GetFailedNotifications.Request request)
        {
            var response = await _mediator.Send(request);
            return Json(response);
        }
    }

}
