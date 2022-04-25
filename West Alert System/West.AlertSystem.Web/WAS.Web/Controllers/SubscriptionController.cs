using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WAS.Web.Models;
using SubscriptionCreate = WAS.Application.Features.Subscription.Create;
using GetbyMail = WAS.Application.Features.Subscription.GetByMail;
using CreateView = WAS.Application.Features.Subscription.CreateView;
using Unsubscribe = WAS.Application.Features.Subscription.Unsubscribe;
using WAS.Application.Interface.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using System;
using Microsoft.AspNetCore.Authorization;
using getById = WAS.Application.Features.Subscription.GetById;
using Delete = WAS.Application.Features.Subscription.Delete;
using Blockuser = WAS.Application.Features.Subscription.BlockUser;
using WAS.Application.Common.Settings;
using Microsoft.Extensions.Options;
using SubscriptionFeedback = WAS.Application.Features.Subscription.SubscriptionFeedback;
using System.Collections.Generic;
using System.Linq;
using WAS.Application.Common.Models;
using GetOcrSubscriptionList = WAS.Application.Features.Subscription.GetOcrSubscriptionList;
using GetOcrSubscription = WAS.Application.Features.Subscription.GetOcrSubscriptionById;
using DiscardOcrSubscription = WAS.Application.Features.Subscription.DiscardOcrSubscription;

namespace WAS.Web.Controllers
{
    [Route("WAS/Subscription")]
    public class SubscriptionController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IGraphService _graphService;
        private readonly INotyfService _notyf;
        private readonly IOptions<UserBlockedInterval> _IntervalOptions;


        public SubscriptionController(
            IMediator mediator,
            IMapper mapper,
            IGraphService graphService,
            INotyfService notyf,
            IOptions<UserBlockedInterval> options
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _graphService = graphService;
            _notyf = notyf;
            _IntervalOptions = options;
        }

        [Route("Subscribe")]
        public async Task<IActionResult> Index()
        {
            var response = await GetUserdetails();

            if (response.Subscription == null && User.Identity.Name == null)
            {
                return RedirectToAction("AnonymousUser");
            }
            else { return View(response); }
        }

        [HttpPost]
        [Route("Subscribe")]
        public async Task<IActionResult> Index(SubscriptionViewModel subscriptionViewModel)
        {
            var request = _mapper.Map<SubscriptionCreate.Request>(subscriptionViewModel);
            if (User.Identity.Name != null)
            {
                subscriptionViewModel.OfficialEmail = User.Identity.Name;
                request.OfficialEmail = User.Identity.Name;
            }
            var adUser = await _graphService.GetUser(subscriptionViewModel.OfficialEmail, User);
            request.OfficialEmail = adUser.Email;
            request.FirstName = adUser.FirstName;
            request.LastName = adUser.LastName;
            request.City = adUser.City;
            request.State = adUser.State;
            request.Country = adUser.Country;
            request.EmployeeId = adUser.EmployeeId;
            request.PostalCode = adUser.PostalCode;
            request.Upn = adUser.UserPrincipalName;
            request.JobTitle = adUser.Designation;
            request.DepartmentName = adUser.Department;
            request.OfficeLocation = adUser.Location;
            request.EmployeeType = adUser.EmployeeType;
            request.EmployeeGroup = adUser.EmployeeGroup;
            request.CostCenter = adUser.CostCenter;
            var result = await _mediator.Send(request);

            if (User.Identity.Name == null)
            {
                return RedirectToAction("AnonymousUser", result);
            }
            else
            {
                var response = await _mediator.Send(new CreateView.Request { OfficialEmail = User.Identity.Name });
                response.ADUser = await _graphService.GetUser(User.Identity.Name, User);
                var adManager = await _graphService.GetUserManagerJson(User.Identity.Name, User);
                if (adManager != null)
                {
                    var managerpicture = await _graphService.GetPictureBase64(adManager.Email, HttpContext, User);
                    response.ADUser.ReportManager = adManager.FullName;
                    response.ADUser.ManagerPictureBase64 = managerpicture;
                }

                if (subscriptionViewModel.Id == Guid.Empty)
                    response.Success = result.Success;
                else
                    _notyf.Success("Profile details updated successfully");

                return View(response);
            }
        }

        [HttpPost]
        [Route("Unsubscribe")]
        public async Task<IActionResult> Unsubscribe(Unsubscribe.Request request)
        {
            await _mediator.Send(request);
            return RedirectToAction("Index");
        }

        [Route("WizardSubscription")]
        public async Task<IActionResult> AnonymousUser(bool success, Guid SubscriptionId)
        {
            var response = await GetUserdetails();
            response.Success = success;
            response.SubscriptionId = SubscriptionId;
            response.UserBlockedInterval = _IntervalOptions.Value.UserBlockedTime;
            return View(response);
        }

        [HttpPost]
        [Route("GetMaskedByMail")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetSubscriptionDetails(string officialEmail)
        {
            var response = await _mediator.Send(new GetbyMail.Request { OfficialEmail = officialEmail });
            return PartialView("~/Views/Group/_SubscriptionDetails.cshtml", response);
        }

        [HttpPost]
        [Route("GetMaskedById")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetSubscriptionDetailsById(Guid id)
        {
            var response = await _mediator.Send(new getById.Request { Id = id });
            var resSubscription = new GetbyMail.Response();
            resSubscription.Subscription = response.Subscription;
            return PartialView("~/Views/Group/_SubscriptionDetails.cshtml", resSubscription);
        }

        [HttpPost]
        [Route("DeleteSubscription")]
        public async Task<IActionResult> DeleteSubscription(Delete.Request request)
        {
           var respose= await _mediator.Send(request);
            if(respose.Success)
            {
                return RedirectToAction("AnonymousSuccessPage");
            }
            else
            {
                return RedirectToAction("Index");
            }            
        }

        [Route("SubscriptionSuccess")]
        public IActionResult AnonymousSuccessPage()
        {
            return View();
        }

        public async Task<CreateView.Response> GetUserdetails()
        {
            var response = await _mediator.Send(new CreateView.Request { OfficialEmail = User.Identity.Name });
            response.ADUser = await _graphService.GetUser(User.Identity.Name, User);

            if (User.Identity.Name != null)
            {
                var adManager = await _graphService.GetUserManagerJson(User.Identity.Name, User);
                if (adManager != null)
                {
                    response.ADUser.ReportManager = adManager.FullName;
                    var managerpicture = await _graphService.GetPictureBase64(adManager.Email, HttpContext, User);
                    response.ADUser.ManagerPictureBase64 = managerpicture;
                }
            }
            return response;
        }

        [HttpGet]
        [Route("getsubscriptiondetails")]
        public async Task<IActionResult> GetExistingSubscriptionDetails(string emailId)
        {
            var response = await _mediator.Send(new CreateView.Request { OfficialEmail = emailId });
            return Ok(response.Subscription);
        }

        [HttpPost]
        [Route("subscriptionFeedback")]
        public async Task<IActionResult> SubscriptionFeedback(SubscriptionFeedback.Request request)
        {
            var response = await _mediator.Send(request);

            if (response == null)
                return NotFound();

            _notyf.Success("Thank you for your invaluable feedback!");
            return Ok();
        }

        [HttpPost]
        [Route("ValidateAnswers")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateAnswers([FromBody] TriangularValidation request)
        {
            string response = "";
            if (request.Questions == null || request.Questions.Count < 3)
            {
                response = "Blocked";
            }
            else
            {
                //getting user details
                var adUser = await _graphService.GetUserDetailsWithManager(request.Email, User);

                if (adUser != null && adUser.Manager != null && adUser.Manager != null)
                {
                    if (!VerifyEachAnswers(request,adUser))
                    {
                        response = "Blocked";
                        await _mediator.Send(
                                new Blockuser.Request() { EmployeeId = adUser.EmployeeId, OfficialEmail = adUser.Email, 
                                Name = adUser.Name ,AttemptON=request.AttemptON , AttemptFrom = request.AttemptFrom });
                    }
                    else
                    {
                        response = "Verified";
                        _notyf.Success("User has been successfully verified");
                    }
                }
                else
                    response = "Failed to authenticate";
            }
            return Json(response);
        }

        private bool VerifyEachAnswers(TriangularValidation request, ADUserForValidation adUser)
        {
            bool flag = true, itemExist = false;
            string selectedItem = "";
            List<string> validationTypes = new List<string>() { "manager", "location", "department" };
            foreach (var item in validationTypes)
            {
                selectedItem = "";
                itemExist = request.Questions.Any(x => x.Type == item);
                if (itemExist)
                    selectedItem = request.Questions.Find(x => x.Type == item).Answer;
                if (
                    (selectedItem == "") ||
                    (item == "manager" && selectedItem.ToUpper() != adUser.Manager.ToUpper()) ||
                    (item == "location" && selectedItem.ToUpper() != adUser.Location.ToUpper()) ||
                    (item == "department" && selectedItem.ToUpper() != adUser.Department.ToUpper())
                  )
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        [HttpGet]
        [Route("SubscriptionReviewList")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> SubscriptionReviewList()
        {
            var response = await _mediator.Send(new GetOcrSubscriptionList.Request { AdminOfficialEmail = User.Identity.Name});
            return View(response);
        }

        [HttpGet]
        [Route("SubscriptionReviewList/{locationId=0}")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> SubscriptionReviewList(int locationId)
        {
            var response = await _mediator.Send(new GetOcrSubscriptionList.Request { AdminOfficialEmail = User.Identity.Name, LocationId = locationId });
            return View(response);
        }

        [HttpGet]
        [Route("SubscriptionReview")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> SubscriptionReview(Guid ocrSubscriptionId)
        {
            var response = await _mediator.Send(new GetOcrSubscription.Request { Id = ocrSubscriptionId});
            return View(response);
        }

        [HttpGet]
        [Route("verifysubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> VerifySubscription(string emailId, string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(emailId))
            {
                return BadRequest("email address is empty");
            }

            SubscriptionReview subscriptionReview = new SubscriptionReview();
            
            var adUser = await _graphService.GetUserWithUserId(emailId, User);

            subscriptionReview.ADUser = adUser;

            if (adUser != null)
            {
                var response = await _mediator.Send(new GetbyMail.Request { OfficialEmail = adUser.Email });
                if (response.Subscription != null)
                {
                    subscriptionReview.SubscriptionId = response.Subscription.Id;
                }
                else if (adUser.FirstName.ToUpper() != firstName.ToUpper() || adUser.LastName.ToUpper() != lastName.ToUpper())
                {
                    _notyf.Error("FirstName or LastName does not match with our records for the provided Employee ID or Official Email or User ID");
                }
                else
                {
                    _notyf.Success("User has been verified successfully");
                }
            }
            else
            {
                _notyf.Error("Employee ID or Official Email or User ID not found");
            }

            return Ok(subscriptionReview);
        }

        [HttpPost]
        [Route("OcrSubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> OcrSubscription(SubscriptionViewModel subscriptionViewModel)
        {
            var request = _mapper.Map<SubscriptionCreate.Request>(subscriptionViewModel);
            var email = request.EmployeeId != null ? request.EmployeeId : request.OfficialEmail != null ? request.OfficialEmail : null;
            var adUser = await _graphService.GetUser(email, User);
            request.OfficialEmail = adUser.Email;
            request.FirstName = adUser.FirstName;
            request.LastName = adUser.LastName;
            request.City = adUser.City;
            request.State = adUser.State;
            request.Country = adUser.Country;
            request.EmployeeId = adUser.EmployeeId;
            request.PostalCode = adUser.PostalCode;
            request.Upn = adUser.UserPrincipalName;
            request.JobTitle = adUser.Designation;
            request.DepartmentName = adUser.Department;
            request.OfficeLocation = adUser.Location;
            request.EmployeeType = adUser.EmployeeType;
            request.EmployeeGroup = adUser.EmployeeGroup;
            request.CostCenter = adUser.CostCenter;

            await _mediator.Send(request);
            _notyf.Success("Subscription added successfully");
            return RedirectToAction("SubscriptionReviewList");
        }

        [HttpPost]
        [Route("discardOcrSubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> DiscardOcrSubscription(Guid subscriptionReviewId)
        {
            await _mediator.Send(new DiscardOcrSubscription.Request { Id = subscriptionReviewId});
            _notyf.Success("Subscription discarded successfully");
            return Ok();
        }
    }
}
