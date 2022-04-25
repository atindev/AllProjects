using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WAS.Web.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Diagnostics;
using createUpdate = WAS.Application.Features.Survey.CreateUpdate;
using AspNetCoreHero.ToastNotification.Abstractions;
using GetAll = WAS.Application.Features.Survey.GetAll;
using System.Linq;
using Delete = WAS.Application.Features.Survey.Delete;
using Share = WAS.Application.Features.Survey.ShareSurvey;
using DeleteBroadcast = WAS.Application.Features.Survey.DeleteBroadcasted;
using GetSurveyById = WAS.Application.Features.Survey.GetById;
using CreateBroadcast = WAS.Application.Features.Survey.GetBroadcastView;
using Broadcast = WAS.Application.Features.Survey.Broadcast;
using CheckSurveyExpiry = WAS.Application.Features.Survey.CheckExpiry;
using GetByBroadcastId = WAS.Application.Features.Survey.GetByBroadcastId;
using GetAllBroadcast = WAS.Application.Features.Survey.GetAllBroadcast;
using CheckAudience = WAS.Application.Features.Survey.CheckAudience;
using GetJsonByBroadcastId = WAS.Application.Features.Survey.GetJsonByBroadcastId;
using SubmitSurvey = WAS.Application.Features.Survey.SubmitSurvey;
using AlreadyFilled = WAS.Application.Features.Survey.IsAlreadyFilled;
using GetSubmissionReportByLocation = WAS.Application.Features.Survey.GetSubmissionReportByLocation;
using GetSubmissionReportByDepartment = WAS.Application.Features.Survey.GetSubmissionReportByDepartment;
using GetAnswerwiseReport = WAS.Application.Features.Survey.GetAnswerwiseReport;
using GetAllSharedPeopleNamesById = WAS.Application.Features.Survey.GetAllSharedPeopleNamesById;
using GetCreatedByList = WAS.Application.Features.Survey.GetCreatedByList;
using System;
using Microsoft.AspNetCore.Http;
using WAS.Application.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using WAS.Application.Interface.Services;
using Newtonsoft.Json;
using WAS.Application.Common.Models;
using System.Collections.Generic;
using CloneSurvey = WAS.Application.Features.Survey.CloneSurvey;
using GetSubjectByBroadcastId = WAS.Application.Features.Survey.GetSubjectByBroadcastId;
using ExtractKeyPhrasesFromShortAnswer = WAS.Application.Features.Survey.ExtractKeyPhrasesFromShortAnswer;

namespace WAS.Web.Controllers
{
    [Authorize]
    [Route("WAS/Survey")]
    public class SurveyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;
        private readonly bool IsGlobalAdmin;
        private readonly IOptions<UserBlockedInterval> _IntervalOptions;
        private readonly IGraphService _graphService;
        const string SessionEmail = "_Email";

        public SurveyController(
            INotyfService notyf,
            IMediator mediator,
            IHttpContextAccessor httpContextAccessor,
            IOptions<UserBlockedInterval> options,
            IGraphService graphService
            )
        {
            _mediator = mediator;
            _notyf = notyf;
            IsGlobalAdmin = httpContextAccessor.HttpContext.User.HasClaim(ClaimTypes.Role, "GlobalAdministrator");
            _IntervalOptions = options;
            _graphService = graphService;
        }

        [Route("Create")]
        [Authorize(Policy = "SurveyAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("List")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> List(string defaultTab)
        {
            var response = await _mediator.Send(new GetCreatedByList.Request { });
            ViewBag.CreatedByList = response.CreatedByList;
            if (defaultTab == null || defaultTab == "")
                ViewBag.value = "survey";
            else
                ViewBag.value = defaultTab;

            return View();
        }

        [Route("EmailSurveyLanding")]
        [AllowAnonymous]
        public async Task<IActionResult> EmailSurveyLanding(Guid id)
        {
            HttpContext.Session.SetString(SessionEmail, "");
            Guid surveyBroadcastId = Guid.Empty;
            var response = await _mediator.Send(new CheckSurveyExpiry.Request { Id = id });
            if (response != null && response.IsActive)
                surveyBroadcastId = id;
            return View(surveyBroadcastId);
        }

        [Route("UserVerification")]
        [AllowAnonymous]
        public IActionResult UserVerification(Guid id)
        {
            HttpContext.Session.SetString("SSOLogin", "false");
            return View(new UserVerificationModel() { BroadcastId = id, UserBlockedInterval = _IntervalOptions.Value.UserBlockedTime });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("AuthenticateSurvey")]
        public IActionResult AuthenticateSurvey(Guid broadcastId, string email)
        {
            HttpContext.Session.SetString("SSOLogin", "false");
            HttpContext.Session.SetString(SessionEmail, email);
            var redirectUrl = Url.Action("StartSurvey", "WAS/Survey");
            redirectUrl += "?id=" + broadcastId;
            redirectUrl = redirectUrl.Replace("%2F", "/");
            return Redirect(redirectUrl);
        }

        [Route("StartSurvey")]
        [AllowAnonymous]
        public async Task<IActionResult> StartSurvey(Guid id)
        {
            string email = "", ssoLogin = "";
            ViewBag.message = "";
            ViewBag.UserName = "";
            ssoLogin = HttpContext.Session.GetString("SSOLogin");
            if (ssoLogin != null && ssoLogin != "" && ssoLogin == "true")
                email = User.Identity.Name;
            else
                email = HttpContext.Session.GetString(SessionEmail);

            HttpContext.Session.SetString("SSOLogin", "false");

            if (email != null && email != "")
            {
                HttpContext.Session.SetString(SessionEmail, email);
                var adUser = await _graphService.GetUserDetailsWithManager(email, User);
                if (adUser != null)
                {
                    var audienceCheck = await _mediator.Send(new CheckAudience.Request { EmployeeId = adUser.EmployeeId, OfficialEmail = adUser.Email, BroadcastId = id });
                    if (audienceCheck.SubscriptionId != Guid.Empty)
                    {
                        //already filled or not
                        ViewBag.UserName = adUser.Name;
                        if (await checkAlreadyFilled(id, adUser))
                        {
                            ViewBag.message = "exist";
                            return View(null);
                        }                        
                        var response = await _mediator.Send(new GetByBroadcastId.Request { Id = id});
                        return View(response);
                    }
                }
            }
            else
            {
                ViewBag.message = "timeout";
                ViewBag.broadcastId = id;
            }

            return View(null);
        }

        [Route("SurveyWizard")]
        [AllowAnonymous]
        public async Task<IActionResult> SurveyWizard(Guid id)
        {
            string emailWizard = HttpContext.Session.GetString(SessionEmail);
            ViewBag.message = "";
            ViewBag.broadcastId = id;
            if (emailWizard != null && emailWizard != "")
            {
                HttpContext.Session.SetString(SessionEmail, emailWizard);
                ViewBag.currentUser = emailWizard;
                ViewBag.surveyStartTime = DateTime.UtcNow;
                var adUserWizard = await _graphService.GetUserDetailsWithManager(emailWizard, User);

                if (adUserWizard == null)
                    return View(null);

                var audienceCheckWizard = await _mediator.Send(new CheckAudience.Request { EmployeeId = adUserWizard.EmployeeId, OfficialEmail = adUserWizard.Email, BroadcastId = id });
                if (audienceCheckWizard.SubscriptionId != Guid.Empty)
                {
                    ViewBag.CurrUser = adUserWizard.Name;
                    //already filled or not
                    if (await checkAlreadyFilled(id, adUserWizard))
                    {
                        ViewBag.message = "exist";
                        return View(null);
                    }

                    var response = await _mediator.Send(new GetJsonByBroadcastId.Request { Id = id });
                    if (response.SurveyContent == "")
                    {
                        var emptyResponse = new WAS.Application.Common.Models.CreateSurvey();
                        return View(emptyResponse);
                    }
                    else
                    {
                        var contentWizard = JsonConvert.DeserializeObject<WAS.Application.Common.Models.CreateSurvey>(response.SurveyContent);
                        return View(contentWizard);
                    }
                }
            }
            else
            {
                ViewBag.message = "timeout";
            }

            return View(null);
        }

        [Route("FillSurvey")]
        [AllowAnonymous]
        public async Task<IActionResult> FillSurvey(Guid id)
        {
            string email = HttpContext.Session.GetString(SessionEmail);
            ViewBag.message = "";
            ViewBag.broadcastId = id;
            if (email != null && email != "")
            {
                HttpContext.Session.SetString(SessionEmail, email);
                ViewBag.currentUser = email;
                ViewBag.surveyStartTime = DateTime.UtcNow;
                var adUser = await _graphService.GetUserDetailsWithManager(email, User);

                if (adUser == null)
                    return View(null);

                var audienceCheck = await _mediator.Send(new CheckAudience.Request { EmployeeId = adUser.EmployeeId, OfficialEmail = adUser.Email, BroadcastId = id });
                if (audienceCheck.SubscriptionId != Guid.Empty)
                {
                    ViewBag.CurrUser = adUser.Name;
                    //already filled or not
                    if (await checkAlreadyFilled(id, adUser))
                    {
                        ViewBag.message = "exist";
                        return View(null);
                    }

                    var response = await _mediator.Send(new GetJsonByBroadcastId.Request { Id = id });
                    if (response.SurveyContent == "")
                    {
                        var emptyResponse = new WAS.Application.Common.Models.CreateSurvey();
                        return View(emptyResponse);
                    }
                    else
                    {
                        var content = JsonConvert.DeserializeObject<WAS.Application.Common.Models.CreateSurvey>(response.SurveyContent);
                        return View(content);
                    }
                }
            }
            else
            {
                ViewBag.message = "timeout";
            }

            return View(null);
        }

        private async Task<bool> checkAlreadyFilled(Guid id, ADUserForValidation adUser)
        {
            bool isFilled = false;
            var alreadyFilled = await _mediator.Send(new AlreadyFilled.Request { EmployeeId = adUser.EmployeeId, Email = adUser.Email, BroadcastId = id });
            if (alreadyFilled == null || (!alreadyFilled.IsNotFilled))
            {
                isFilled = true;
            }
            return isFilled;
        }
        [HttpGet]
        [Route("GetAllSharedPeopleNamesById")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetAllSharedPeopleNamesById(Guid id)
        {

            var response = await _mediator.Send(new GetAllSharedPeopleNamesById.Request { BroadcastId = id });
            return Json(response);
        }

        [Route("Broadcast")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> BroadcastAsync(Guid id, bool isUpdate)
        {
            var request = new CreateBroadcast.Request();
            request.Email = User.Identity.Name;
            request.IsGlobalAdmin = IsGlobalAdmin;

            if (isUpdate)
            {
                var broadcastResponse = await _mediator.Send(new GetByBroadcastId.Request { Id = id });
                request.SurveyId = broadcastResponse.SurveyId;
            }
            else
            {
                request.SurveyId = id;
            }

            var response = await _mediator.Send(request);
            if (isUpdate)
            {
                response.BroadcastId = id;
                response.IsUpdate = true;
            }
            return View(response);
        }

        [Route("View")]
        [Authorize(Policy = "SurveyAdmin")]
        public IActionResult ViewOrUpdate(Guid id)
        {
            return View(id);
        }

        [Route("BroadcastDetails")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> BroadcastDetails(Guid id)
        {
            var response = await _mediator.Send(new GetSubjectByBroadcastId.Request { Id = id });
            ViewBag.subject = "";
            if (response != null)
                ViewBag.subject = response.Subject;
            return View(id);
        }

        [HttpPost]
        [Route("CreateUpdateSurvey")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> CreateUpdateSurvey([FromBody] createUpdate.Request request)
        {
            if (request.Id == Guid.Empty)
                request.CreatedBy = request.ModifiedBy = User.Identity.Name;
            else
                request.ModifiedBy = User.Identity.Name;

            var response = await _mediator.Send(request);
            if (response.Success)
            {
                if (request.Id == Guid.Empty)
                    _notyf.Success("Survey created successfully");
                else
                    _notyf.Success("Survey updated successfully");
            }
            else
            {
                if (request.Id == Guid.Empty)
                    _notyf.Error("Survey creation failed");
                else
                    _notyf.Error("Survey updation failed");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("GetAllSurvey")]
        public async Task<object> GetAllSurvey([FromBody] FilterRequest filterRequest)
        {

            GetAll.Request request = new GetAll.Request();
            request.PageType = "Paged";
            request.PageIndex = filterRequest.Skip;
            request.RowCount = (filterRequest.Take == 0 ? 10 : filterRequest.Take);

            if (filterRequest.Params != null && filterRequest.Params.Count > 0)
            {
                string NameFilter = null;
                string CreatedByFilter = null;

                if (filterRequest.Params.TryGetValue("NameFilter", out NameFilter) && NameFilter != "")
                {
                    request.NameFilter = NameFilter;
                }
                if (filterRequest.Params.TryGetValue("CreatedByFilter", out CreatedByFilter) && CreatedByFilter != "")
                {
                    request.CreatedByFilter = CreatedByFilter;
                }
            }

            var response = await _mediator.Send(request);
            return Json(new { result = response.Surveys.ToList(), count = response.Count });
        }

        [HttpPost]
        [Route("DeleteSurvey")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> DeleteSurvey(Delete.Request request)
        {
            request.ModifiedBy = User.Identity.Name;
            dynamic response = null;
            string textMessage = "";

            if (request.SurveyType == "Survey")
            {
                response = await _mediator.Send(request);
                textMessage = "Survey";
            }
            else
            {
                response = await _mediator.Send(new DeleteBroadcast.Request() { Id = request.Id, ModifiedBy = request.ModifiedBy });
                textMessage = "Broadcasted survey";
            }

            if (response.Success)
                _notyf.Success(textMessage + " deleted successfully");
            else
                _notyf.Error(textMessage + " deletion failed");

            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("ShareSurvey")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> ShareSurvey(Share.Request request)
        {
            dynamic response = null;
            response = await _mediator.Send(new Share.Request() { BroadcastId = request.BroadcastId, PeopleMail = request.PeopleMail, CreatedBy = User.Identity.Name });
            if (response.Success)
                _notyf.Success("Survey shared successfully");
            else
                _notyf.Error("failed to share survey");
            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("GetSurveyById")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetSurveyById(string Id)
        {
            Guid surveyId = Guid.Parse(Id);
            var response = await _mediator.Send(new GetSurveyById.Request { Id = surveyId });
            return Json(response.SurveyContent);
        }

        [HttpPost]
        [Route("BroadcastSurvey")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> BroadcastSurvey([FromBody] Broadcast.Request request)
        {
            request.StartTime = request.StartTime.AddMinutes(request.TimeZoneOffset);
            request.EndTime = request.EndTime.AddMinutes(request.TimeZoneOffset);

            if (request.EndTime < request.StartTime)
                request.EndTime = request.StartTime.AddDays(1);

            if (request.FollowUpTime != null)
            {
                request.FollowUpTime = request.FollowUpTime?.AddMinutes(request.TimeZoneOffset);
                if (request.FollowUpTime > request.EndTime || request.FollowUpTime < request.StartTime)
                {
                    TimeSpan ts = request.EndTime.Subtract(request.StartTime);
                    request.FollowUpTime = request.StartTime.AddMinutes(ts.TotalMinutes / 2);
                }
            }

            request.CreatedBy = User.Identity.Name;
            if (request.StartTime <= DateTime.UtcNow)
                request.Status = Application.Common.Enum.SurveyStatus.SendNow;
            else
                request.Status = Application.Common.Enum.SurveyStatus.Submitted;

            var response = await _mediator.Send(request);
            if (request.BroadcastId == Guid.Empty) //for create
            {
                if (response.Success)
                    _notyf.Success("Survey broadcasted successfully");
                else
                    _notyf.Error("Survey broadcasting failed");
            }
            else
            {
                if (response.Success)
                    _notyf.Success("Broadcasted survey updated successfully");
                else
                    _notyf.Error("Broadcasted survey updation failed");
            }

            return Json(response);
        }

        [HttpPost]
        [Route("GetAllBroadcast")]
        public async Task<object> GetAllBroadcast([FromBody] FilterRequest filterRequest)
        {

            GetAllBroadcast.Request request = new GetAllBroadcast.Request();
            request.PageType = "Paged";
            request.PageIndex = filterRequest.Skip;
            request.RowCount = (filterRequest.Take == 0 ? 10 : filterRequest.Take);
            request.IsGlobalAdmin = IsGlobalAdmin;
            request.UserMailId = User.Identity.Name;

            if (filterRequest.Params != null && filterRequest.Params.Count > 0)
            {
                var filterParam = getFromQueryFilter(filterRequest);
                request.NameFilter = filterParam.NameFilter;
                request.StatusFilter = filterParam.StatusFilter;
                request.StartTime = filterParam.StartTime;
                request.EndTime = filterParam.EndTime;
                request.CreatedByFilter = filterParam.CreatedByFilter;
            }
            var response = await _mediator.Send(request);
            if (response.BroadcastedSurveys != null)
            {
                foreach (var survey in response.BroadcastedSurveys)
                {
                    survey.ADPeopleNames = survey.ADPeople.Where(s => s.FirstName != null && s.LastName != null).Select(s => s.LastName + ", " + s.FirstName).ToList();
                    survey.DistributionGroupNames = survey.DistributionGroups.Where(s => s.Name != null).Select(s => s.Name).ToList();
                }
            }
            return Json(new { result = response.BroadcastedSurveys.ToList(), count = response.Count });
        }

        private GetAllBroadcast.Request getFromQueryFilter(FilterRequest filterRequest)
        {
            GetAllBroadcast.Request request = new GetAllBroadcast.Request();

            string NameFilter = null, StatusFilter = null, StartTime = null, EndTime = null, TimeZoneOffset = null, CreatedByFilter=null;
            int offset = 0;
            if (filterRequest.Params.TryGetValue("NameFilter", out NameFilter) && NameFilter != "")
            {
                request.NameFilter = NameFilter;
            }
            if (filterRequest.Params.TryGetValue("StatusFilter", out StatusFilter) && StatusFilter != "")
            {
                request.StatusFilter = (Application.Common.Enum.SurveyStatus)Convert.ToInt16(StatusFilter);
            }
            if (filterRequest.Params.TryGetValue("TimeZoneOffset", out TimeZoneOffset) && TimeZoneOffset != null && TimeZoneOffset != "")
            {
                offset = Convert.ToInt16(TimeZoneOffset);
            }
            if (filterRequest.Params.TryGetValue("StartTime", out StartTime) && StartTime != "")
            {
                request.StartTime = DateTime.Parse(StartTime).AddMinutes(offset).Date;
            }
            if (filterRequest.Params.TryGetValue("EndTime", out EndTime) && EndTime != "")
            {
                request.EndTime = DateTime.Parse(EndTime).AddMinutes(offset).Date;
            }
            if (filterRequest.Params.TryGetValue("CreatedByFilter", out CreatedByFilter) && CreatedByFilter != "")
            {
                request.CreatedByFilter = CreatedByFilter;
            }
            return request;
        }

        [HttpPost]
        [Route("GetBroadcastById")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetBroadcastById(Guid Id)
        {
            var response = await _mediator.Send(new GetByBroadcastId.Request { Id = Id });
            response.StartTime = response.StartTime.AddMinutes(response.TimeZoneOffset);
            response.EndTime = response.EndTime.AddMinutes(response.TimeZoneOffset);
            if (response.FollowUpTime != null)
                response.FollowUpTime = response.FollowUpTime?.AddMinutes(response.TimeZoneOffset);
            return Json(response);
        }

        [HttpPost]
        [Route("SubmitSurvey")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitSurvey([FromBody] SubmitSurvey.Request request)
        {
            var adUser = await _graphService.GetUserDetailsWithManager(request.Email, User);
            dynamic response = null;
            if (adUser != null)
            {
                request.SurveyEndTime = DateTime.UtcNow;
                request.Email = adUser.Email;
                request.EmployeeId = adUser.EmployeeId;
                response = await _mediator.Send(request);
                if (response.Success)
                    _notyf.Success("Survey submitted successfully");
                else
                    _notyf.Error("Survey submission failed");
            }
            else
                _notyf.Error("Survey submission failed");

            return Json(response);
        }

        [HttpPost]
        [Route("GetSubmissionReportByLocation")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetSubmissionReportByLocation(Guid Id)
        {
            var response = await _mediator.Send(new GetSubmissionReportByLocation.Request { Id = Id });
            return PartialView("~/Views/Survey/_ReportByLocation.cshtml", response);
        }

        [HttpPost]
        [Route("GetBasicBroadcastDetails")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetBasicBroadcastDetails(Guid Id)
        {
            var response = await _mediator.Send(new GetByBroadcastId.Request { Id = Id });
            return PartialView("~/Views/Survey/_BasicBroadcastDetails.cshtml", response);
        }

        [HttpPost]
        [Route("GetSubmissionReportByDepartment")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetSubmissionReportByDepartment(Guid Id)
        {
            var response = await _mediator.Send(new GetSubmissionReportByDepartment.Request { Id = Id });

            return PartialView("~/Views/Survey/_ReportByDepartment.cshtml", response);
        }

        [Route("GetAnswerwiseReport")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetAnswerwiseReport(Guid id)
        {
            var response = await _mediator.Send(new GetSubjectByBroadcastId.Request { Id = id });
            ViewBag.subject = "";
            if (response != null)
                ViewBag.subject = response.Subject;
            return View(id);
        }

        [HttpPost]
        [Route("GetAnswerwiseData")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetAnswerwiseData(Guid Id)
        {
            var response = await _mediator.Send(new GetAnswerwiseReport.Request { Id = Id });
            return PartialView("~/Views/Survey/_AnswerwiseAnalysis.cshtml", response);
        }

        [HttpPost]
        [Route("CloneSurvey")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> CloneSurvey(Guid surveyId)
        {
            var response = await _mediator.Send(new CloneSurvey.Request { SurveyId = surveyId, EmailId = User.Identity.Name });
            if (response.Success)
            {
                _notyf.Success("Survey cloned successfully");
                return RedirectToAction(nameof(SurveyController.ViewOrUpdate), new { id = response.SurveyId });
            }
            else
            {
                _notyf.Error("Survey cloning failed");
                return RedirectToAction(nameof(SurveyController.List));
            }

        }

        [HttpPost]
        [Route("GetBroadcastDetailsToRefresh")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetBroadcastDetailsToRefresh(Guid broadcastId)
        {
            var response = await _mediator.Send(new GetByBroadcastId.Request { Id = broadcastId, IgnoreAudienceResponseCount = true });
            return PartialView("~/Views/Survey/_BroadcastDetailsTimeline.cshtml", response);
        }

        [HttpPost]
        [Route("GetBroadcastInfo")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetBroadcastInfo(Guid broadcastId)
        {
            var response = await _mediator.Send(new GetByBroadcastId.Request { Id = broadcastId });
            return PartialView("~/Views/Survey/_BasicBroadcastInfo.cshtml", response);
        }

        [HttpPost]
        [Route("GetShortAnswerAnalysis")]
        [Authorize(Policy = "SurveyAdmin")]
        public async Task<IActionResult> GetShortAnswerAnalysis(Guid Id)
        {
            var response = await _mediator.Send(new ExtractKeyPhrasesFromShortAnswer.Request { Id = Id });
            return PartialView("~/Views/Survey/_ShortAnswerAnalysis.cshtml", response);
        }
    }
}

