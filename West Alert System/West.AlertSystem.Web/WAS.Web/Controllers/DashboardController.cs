using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WAS.Web.Models;
using DashboardView = WAS.Application.Features.Dashboard.DashboardView;
using CreateView = WAS.Application.Features.Subscription.CreateView;
using System.Security.Claims;
using GetAll = WAS.Application.Features.Groups.GetAll;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WAS.Web.Controllers
{
    [Authorize] 
    [Route("WAS/Dashboard")]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;

        public DashboardController(
           IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Route("View")]
        public async Task<IActionResult> Index(string source)
        {
            var emailLegalReg = @"^[a-zA-Z0-9._%+-]+@westpharma\.com$";
            if (Regex.IsMatch(User.Identity.Name, emailLegalReg, RegexOptions.IgnoreCase))
            {
                //checking already subscription is there or not
                var subscription = await _mediator.Send(new CreateView.Request { OfficialEmail = User.Identity.Name });
                if ((User.HasClaim(ClaimTypes.Role, "WASAdmin") || User.HasClaim(ClaimTypes.Role, "GlobalAdministrator")) && ((source != null && source == "Dashboard") || (source != null && source == "IncomingMessageTab") || (subscription != null && subscription.Subscription != null)))
                {
                    bool IsGlobalAdmin = User.HasClaim(ClaimTypes.Role, "GlobalAdministrator");
                    var response = await _mediator.Send(new DashboardView.Request() {Email = User.Identity.Name, IsGlobalAdmin = IsGlobalAdmin });
                    var responseGroup = await _mediator.Send(new GetAll.Request() { IsAccessRequired = true, Email = User.Identity.Name, IsGlobalAdmin=IsGlobalAdmin });
                    response.GroupList = responseGroup;
                    if (source!=null && source == "IncomingMessageTab")
                    {
                        ViewBag.source = "IncomingMessageTab";
                    }
                    TempData["EventNavigatedFrom"] = "/WAS/Dashboard/View";
                    TempData["BackButtonTitle"] = "Back to dashboard";
                    return View(response);
                }
                else
                    return RedirectToAction(nameof(SubscriptionController.Index), "Subscription");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [Route("AccessDenied")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorType = "Access Denied",
                ErrorMessage = "Please use your regular @westpharma.com account to access this portal"
            });
        }
    }
}
