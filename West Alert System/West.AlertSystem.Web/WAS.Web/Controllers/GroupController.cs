using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GetAll = WAS.Application.Features.Groups.GetAll;
using GetAllSubscriptions = WAS.Application.Features.Subscription.GetAll;
using QueryView = WAS.Application.Features.Groups.QueryView;
using CreateUpdate = WAS.Application.Features.Groups.CreateUpdate;
using AddSubscription = WAS.Application.Features.Groups.AddSubscription;
using GetByGroupId = WAS.Application.Features.Groups.GetByIds;
using CreateView = WAS.Application.Features.Subscription.CreateView;
using Delete = WAS.Application.Features.Groups.Delete;
using RemoveSubscription = WAS.Application.Features.Groups.RemoveSubscription;
using WAS.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using WAS.Application.Common.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using DeletePeople = WAS.Application.Features.Subscription.Delete;
using Active = WAS.Application.Features.Groups.Active;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace WAS.Web.Controllers
{
    [Authorize]
    [Route("WAS/Group")]
    public class GroupController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyf;
        private readonly bool IsGlobalAdmin;

        public GroupController(IMediator mediator, IMapper mapper, INotyfService notyf, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _mapper = mapper;
            _notyf = notyf;
            IsGlobalAdmin = httpContextAccessor.HttpContext.User.HasClaim(ClaimTypes.Role, "GlobalAdministrator");
        }

        [Route("List")]
        public async Task<IActionResult> List(string defaultTab)
        {
            var response = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name, IsArchiveGroupRequired = true , IsGlobalAdmin=IsGlobalAdmin });
            GroupListModel groupResponse = new GroupListModel();
            groupResponse.Response = response;

            if (defaultTab == null || defaultTab == "")
                ViewBag.value = "Groups";
            else
                ViewBag.value = defaultTab;

            return View(groupResponse);
        }

        [HttpPost]
        [Route("GetGropus")]
        public async Task<object> GetGroupsAsync([FromBody] FilterRequest filterRequest)
        {

            GetAll.Request request = new GetAll.Request();
            request.PageType = "Paged";
            request.PageIndex = filterRequest.Skip;
            request.RowCount = (filterRequest.Take == 0 ? 7 : filterRequest.Take);

            if (filterRequest.Params != null && filterRequest.Params.Count > 0)
            {
                string GroupFilter = null;

                if (filterRequest.Params.TryGetValue("GroupFilter", out GroupFilter) && GroupFilter != "")
                {
                    request.GroupFilter = GroupFilter;
                }
            }

            var response = await _mediator.Send(request);
            return Json(new { result = response.Groups.ToList(), count = response.Count });
        }

        [HttpGet]
        [Route("GetFilterValues")]
        public async Task<JsonResult> GetFilterValues()
        {
            var response = await _mediator.Send(new QueryView.Request());
            return Json(response);

        }

        [HttpPost]
        [Route("CreateUpdate")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> CreateorUpdate(CreateUpdate.Request groupModel)
        {
            if (groupModel.Id[0] == 0 && groupModel.CreatedBy == null)
                groupModel.CreatedBy = User.Identity.Name;

            if (groupModel.IsAdminAccessEnabled == "on")
            {
                groupModel.IsAccessToAdmins = true;
            }

            if (groupModel.IsPrivateEnabled == "on")
            {
                groupModel.IsPrivate = true;
            }

            var response = await _mediator.Send(groupModel);

            if (response.IsNameExist)
                _notyf.Warning("Group name already exist");
            else
            {
                if (groupModel.Id[0] == 0)
                    _notyf.Success("Group created successfully");
                else
                    _notyf.Success("Group details updated successfully");
            }

            return RedirectToAction("List");
        }


        [HttpPost]
        [Route("GetPeople")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> GetPeople([FromBody] QueryBuilder peopleFilter)
        {
            var request = _mapper.Map<GetAllSubscriptions.Request>(peopleFilter);
            var response = await _mediator.Send(request);
            return Json(response);
        }

        [HttpPost]
        [Route("AddSubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<JsonResult> AddSubscription([FromBody] AddSubscriptionToGroupModel subscriptionList)
        {
            var response = new AddSubscription.Response();
            List<int> groupId = new List<int>();

            if (subscriptionList.CreatedBy == null)
            {
                subscriptionList.CreatedBy = subscriptionList.ModifiedBy = User.Identity.Name;
            }

            if (subscriptionList.Id.Count == 1 && subscriptionList.Id[0] == 0)
            {
                var createRequest = _mapper.Map<CreateUpdate.Request>(subscriptionList);
                var createResponse = await _mediator.Send(createRequest);
                if (createResponse.IsNameExist)
                {
                    _notyf.Warning("Group name already exist");
                    return Json(createResponse);
                }
                groupId.Add(createResponse.Id);
            }
            else
                groupId = subscriptionList.Id;

            if (groupId.Count > 0 && subscriptionList.SubscriptionId.Count > 0)
            {
                subscriptionList.Id = groupId;
                var addSubscriptionRequest = _mapper.Map<AddSubscription.Request>(subscriptionList);
                response = await _mediator.Send(addSubscriptionRequest);
            }

            _notyf.Success("Member/s added to Group successfully");
            return Json(response);
        }

        [Route("View")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> View(int groupId)
        {
            GetByGroupId.Request request = new GetByGroupId.Request();
            request.Ids = new List<int>() { groupId };
            request.IsArchiveGroupRequired = true;
            var responseGroup = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name, IsGlobalAdmin= IsGlobalAdmin });            
            var response = await _mediator.Send(request);
            response.groupList = responseGroup;
            response.GroupId = groupId;
            //checking if group is active or not
            if (responseGroup.Groups.Any(x => x.Id == groupId))
                ViewBag.isGroupActive = true;
            else
                ViewBag.isGroupActive = false;

            return View(response);
        }

        [HttpPost]
        [Route("DeleteGroup")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> DeleteGroup(Delete.Request request)
        {
            request.ModifiedBy = User.Identity.Name;

            var response = await _mediator.Send(request);

            if (response.Success)
                _notyf.Success("Group deleted successfully");
            else
                _notyf.Error("Group deletion failed");

            return RedirectToAction("List");

        }

        [HttpPost]
        [Route("RemoveSubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> RemoveSubscription([FromBody] RemoveSubscription.Request request)
        {
            request.ModifiedBy = User.Identity.Name;
            var response = await _mediator.Send(request);
            if (response.Success)
                _notyf.Success("Subscriptions removed successfully");
            else
                _notyf.Error("Subscriptions removal failed");
            return Json(response);
        }

        [HttpPost]
        [Route("DeleteSubscription")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> DeleteSubscription(DeletePeople.Request request)
        {
            request.IsDeleteRequestFromSubscriber = false;
            var response = await _mediator.Send(request);
            if (response.Success)
                _notyf.Success("Subscriber deleted successfully");
            else
                _notyf.Error("Subscriber deletion failed");

            return RedirectToAction("List");

        }

        [HttpPost]
        [Route("RestoreGroup")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> RestoreGroup(Active.Request request)
        {
            request.ModifiedBy = User.Identity.Name;
            var response = await _mediator.Send(request);
            if (response.Success)
                _notyf.Success("Group restored successfully");
            else
                _notyf.Error("Group restoration failed");

            return RedirectToAction("List");
        }

        [HttpPost]
        [Route("ShowHideGroups")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> ShowHideGroups(bool hideDeletedGroups)
        {
            var response = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name, IsArchiveGroupRequired = true, IsGlobalAdmin = IsGlobalAdmin });
            if (response != null && response.Groups.Any() && hideDeletedGroups)
                response.Groups = response.Groups.Where(x => x.IsActive).ToList();
            return Json(response);
        }

        [HttpPost]
        [Route("GetPeopleByPages")]
        public async Task<object> GetPeopleByPages([FromBody] PeoplePaginationRequest filterRequest)
        {
            GetAllSubscriptions.Request request = new GetAllSubscriptions.Request();
            request.PageType = "Paged";
            request.PageIndex = filterRequest.Skip;
            request.RowCount = (filterRequest.Take == 0 ? 10 : filterRequest.Take);
           
            if (filterRequest.Params != null && filterRequest.Params.Count > 0)
            {
                QueryBuilderRequest QueryBuilder = null;
                if (filterRequest.Params.TryGetValue("QueryBuilder", out QueryBuilder) && QueryBuilder != null)
                {
                    request.rules = QueryBuilder.rules;
                    request.condition = QueryBuilder.condition;
                }

            }
            var response = await _mediator.Send(request);
            return Json(new { result = response.Subscriptions.ToList(), count = response.Count });
        }
    }
}
