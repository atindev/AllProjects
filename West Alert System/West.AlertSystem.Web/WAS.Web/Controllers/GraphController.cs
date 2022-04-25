using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;
using WAS.Web.Models;
using Validation = WAS.Application.Features.Subscription.TriangularValidation;
using blockedUser = WAS.Application.Features.Subscription.IsBlockedUser;

namespace WAS.Web.Controllers
{
    [Route("WAS/Graph")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly IGraphService _graphService;
        private readonly ITriangularValidationService _triangularValidationService;
        private readonly IMediator _mediator;

        public GraphController(IGraphService graphService, ITriangularValidationService triangularValidationService, IMediator mediator)
        {
            _graphService = graphService;
            _triangularValidationService = triangularValidationService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getuserpicture")]
        public async Task<IActionResult> GetUserPicture(string emailId)
        {
            if (string.IsNullOrEmpty(emailId))
            {
                return BadRequest("email address is empty");
            }

            var pictureBase64 = await _graphService.GetPictureBase64(emailId, HttpContext, User);

            var adUser = new ADUser { PictureBase64 = pictureBase64, Email = emailId };
            return Ok(adUser);
        }

        [HttpGet]
        [Route("getuserdetails")]
        public async Task<IActionResult> GetAdUserDetails(string emailId)
        {
            if (string.IsNullOrEmpty(emailId))
            {
                return BadRequest("email address is empty");
            }

            var adUser = await _graphService.GetUser(emailId, User);
            if (adUser != null)
            {
                var adManager = await _graphService.GetUserManagerJson(adUser.Email, User);
                var pictureBase64 = await _graphService.GetPictureBase64(adUser.Email, HttpContext, User);

                if (adManager != null)
                {
                    adUser.ReportManager = adManager.FullName;
                    var managerpicture = await _graphService.GetPictureBase64(adManager.Email, HttpContext, User);
                    adUser.ManagerPictureBase64 = managerpicture;
                }
                if (pictureBase64 != null)
                {
                    adUser.PictureBase64 = pictureBase64;
                }
            }

            return Ok(adUser);
        }

        [HttpGet]
        [Route("GetValidationList")]
        public async Task<IActionResult> GetValidationList(string emailId)
        {
            var response = new TriangularValidation();

            var adUser = await _graphService.GetUserDetailsWithManager(emailId, User);

            if (adUser != null && adUser.Manager != null && adUser.Manager != null)
            {
                //checking if user is blocked
                var isBlockeduser = await _mediator.Send(new blockedUser.Request() { EmailorEmployeeId = adUser.EmployeeId });
                if (isBlockeduser.IsBlocked)
                {
                    response.IsUserBlocked = true;
                    return Ok(response);
                }

                if (string.IsNullOrEmpty(emailId))
                {
                    return BadRequest("email address is empty");
                }

                //for manager question
                var reporteesList = await _graphService.GetPeopleWorkingWith(adUser.Manager);
                var items = reporteesList.Where(i => (i != null && !i.ToUpper().Contains("EXTERNAL"))).ToList();

                var randomList = _triangularValidationService.GetSelectedRandomNames(items, 3);
                var optionList = _triangularValidationService.InsertToRandomPosition(randomList, adUser.Manager);

                response.Email = adUser.Email;
                response.EmployeeId = adUser.EmployeeId;

                response.Questions.Add(new TriangularValidationQuestion()
                {
                    Type = "manager",
                    Question = "Who is your reporting manager?",
                    Options = optionList
                });

                //for location and department question
                var details = await _mediator.Send(new Validation.Request());

                var locationData = getLocationQuestion(details, adUser);
                if (locationData != null)
                    response.Questions.Add(locationData);

                var departmentData = getDepartmentQuestion(details, adUser);
                if (departmentData != null)
                    response.Questions.Add(departmentData);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("getuserrole")]
        public async Task<IActionResult> GetAdUserRoles(string emailId)
        {
            if (string.IsNullOrEmpty(emailId))
            {
                return BadRequest("email address is empty");
            }

            var role = await _graphService.GetUserRole(emailId);

            return Ok(role);
        }

        [HttpGet]
        [Route("GetADUsers")]
        public async Task<IActionResult> GetMatchingADUsers(string searchString)
        {
            var userNames = await _graphService.GetMatchingUsers(searchString, User);
            return Ok(userNames);
        }

        [HttpGet]
        [Route("GetDistributionLists")]
        public async Task<IActionResult> GetDistributionLists(string searchString)
        {
            var distributionList = await _graphService.GetDistributionLists(searchString, User);
            return Ok(distributionList);
        }

        [HttpGet]
        [Route("GetDistributionListMember")]
        public async Task<IActionResult> GetDistributionListMember(string groupId)
        {
            var distributionListMembers = await _graphService.GetDistributionListMembers(groupId, User);
            return Ok(distributionListMembers);
        }

        private TriangularValidationQuestion getLocationQuestion(Validation.Response details, ADUserForValidation adUser)
        {
            if (details.Locations != null && details.Locations.Count > 0)
            {
                var locationName = details.Locations.Where(i => i.Name.ToUpper() != adUser.Location.ToUpper()).Select(x => x.Name).ToList();
                var locationList = _triangularValidationService.GetSelectedRandomNames(locationName, 3, false);
                var optionLocationList = _triangularValidationService.InsertToRandomPosition(locationList, adUser.Location);

                return new TriangularValidationQuestion()
                {
                    Type = "location",
                    Question = "What is your business location?",
                    Options = optionLocationList
                };
            }
            else
                return null;
        }

        private TriangularValidationQuestion getDepartmentQuestion(Validation.Response details, ADUserForValidation adUser)
        {
            if (details.Departments != null && details.Departments.Count > 0)
            {
                var dapartmentName = details.Departments.Where(i => i.Name.ToUpper() != adUser.Department.ToUpper()).Select(x => x.Name).ToList();
                var departmentList = _triangularValidationService.GetSelectedRandomNames(dapartmentName, 3, false);
                var optionDepartmentList = _triangularValidationService.InsertToRandomPosition(departmentList, adUser.Department);

                return new TriangularValidationQuestion()
                {
                    Type = "department",
                    Question = "In which department are you working?",
                    Options = optionDepartmentList
                };
            }
            else
                return null;
        }
    }
}
