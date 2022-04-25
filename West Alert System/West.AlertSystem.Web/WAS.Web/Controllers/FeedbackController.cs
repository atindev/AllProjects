using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Userfeedback = WAS.Application.Features.Feedback;

namespace WAS.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;

        public FeedbackController(
           IConfiguration configuration,
           IMediator mediator,
           IMapper mapper, INotyfService notyf)
        {
            _configuration = configuration;
            _mediator = mediator;
            _notyf = notyf;
        }

        [HttpPost]
        public async Task<JsonResult> SubmitFeedback(Userfeedback.Request request)
        {
            if (request.UserEmailID == null)
            {
                request.UserEmailID = _configuration["DefaultFeedbackFromEmail"];
                request.Username = "Anonymous";
            }

            var response = new Userfeedback.Response();
            if (request != null)
            {
                request.PictureFileName = DateTime.Now.Ticks.ToString() + ".png";
                if (request.FeedbackPictureFile != null)
                {
                    request.PictureFileName = request.FeedbackPictureFile?.FileName;

                    var fileExtension = Path.GetExtension(request.PictureFileName).ToLower();

                    if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg")
                    {
                        response.StatusCode = 0;
                        response.Message = "The image file type is not accepted.";
                        return Json(response);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        await request.FeedbackPictureFile.CopyToAsync(memoryStream);

                        // Upload the file if less than 2 MB
                        if (memoryStream.Length > 2097152)
                        {
                            response.StatusCode = 0;
                            response.Message = "The file is too large and the size has exceeded 2MB";
                            return Json(response);
                        }
                        request.PictureBase64 = memoryStream.ToArray();
                    }
                }
                else
                {
                    request.Base64String = request.Base64String.Replace("data:image/png;base64,", "");
                    request.PictureBase64 = Convert.FromBase64String(request.Base64String);
                }

                var res = await _mediator.Send(request);
                if (!res.Success)
                {
                    response.StatusCode = 0;
                    response.Message = res.Message;
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Message = $"We appreciate your feeedback, Thank You!";
                    _notyf.Success("We appreciate your feeedback, Thank You!");

                }
            }
            return Json(response);
        }
    }
}
