using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GetVideo = WAS.Application.Features.Training.GetVideo;
using GetVideoById = WAS.Application.Features.Training.GetVideoById;

namespace WAS.Web.Controllers
{
    [Route("WAS/Training")]
    public class TrainingController : Controller
    {
        private readonly IMediator _mediator;

        public TrainingController(
           IMediator mediator
          )
        {
            _mediator = mediator;
        }

        [Route("List/{Language}")]
        public async Task<IActionResult> Index([FromRoute] GetVideo.Request request)
        {
            var response = await _mediator.Send(request);
            return View(response);
        }

        [Route("View")]
        public async Task<IActionResult> View(int videoId)
        {
            var response = await _mediator.Send(new GetVideoById.Request { videoId = videoId});
       
            return View(response);
        }
    }
}
