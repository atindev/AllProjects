using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GetReports = WAS.Application.Features.Report.GetReports;
using Microsoft.AspNetCore.Authorization;

namespace WAS.Web.Controllers
{
    [Authorize]
    [Route("WAS/Reports")]
    public class ReportPageController : Controller
    {
        private readonly IMediator _mediator;

        public ReportPageController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<IActionResult> Index()
        {
            var response = await _mediator.Send(new GetReports.Request());
            return View(response);
        }

        [HttpGet]
        [Route("location")]
        [Authorize(Policy = "WASAdmin")]
        public async Task<PartialViewResult> Location(int locationId)
        {
            GetReports.Request request = new GetReports.Request();
            request.LocationId = locationId;
            var response = await _mediator.Send(request);
            return PartialView("_reports",response);
        }

    }
}
