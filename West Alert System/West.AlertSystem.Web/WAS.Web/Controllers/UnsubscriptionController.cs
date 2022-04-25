using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vereyon.Web;
using WAS.Web.Models;
using UnsubscribeMail = WAS.Application.Features.Subscription.UnsubscribeMail;
namespace WAS.Web.Controllers
{
    [Route("WAS/Unsubscription")]
    public class UnsubscriptionController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly IMediator _mediator;

        public UnsubscriptionController(
            IFlashMessage flashMessage,
            IMediator mediator
            )
        {
            _flashMessage = flashMessage;
            _mediator = mediator;
        }

        [Route("Email")]
        public IActionResult Index(string unsubscriberemail)
        {
            return View((object)unsubscriberemail);
        }

        [Route("UnsubscribeMail")]
        [HttpPost]
        public IActionResult UnsubscribeMail(string email,string removePersonalEmail)
        {
            if (email.Contains("@westpharma.com"))
            {
                _flashMessage.Danger($"Unsubscribing from work email is not allowed");
                return RedirectToAction("Index", new { unsubscriberemail = email });
            }
            return RedirectToAction("VerifyOtp", new UnsubscribeMail.Request() { Email = email, RemovePersonalEmail= removePersonalEmail });
        }

        [Route("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(UnsubscribeMail.Request request)
        {
            var response = await _mediator.Send(request);

            if (response.Success)
            {
                return View(request);
            }
            else
            {
                _flashMessage.Danger($"We don't see any active subscription for the email id {request.Email}");
                return RedirectToAction("Index", new { unsubscriberemail = request.Email });
            }
        }

        [Route("EmailVerifyOtp")]
        [HttpPost]
        public async Task<IActionResult> EmailVerifyOtp(VerifyOtp verifyOtp)
        {
            await _mediator.Send(new UnsubscribeMail.Request
            {
                Otp = $"{verifyOtp.OtpFirstDigit}{verifyOtp.OtpSecondDigit}{verifyOtp.OtpThirdDigit}{verifyOtp.OtpFourthDigit}{verifyOtp.OtpFifthDigit}{verifyOtp.OtpSixthDigit}",
                Email = verifyOtp.Email,
                RemovePersonalEmail = verifyOtp.RemovePersonalEmail
            });

            return RedirectToAction("VerifyOtpSuccess", new { email = verifyOtp.Email });
        }

        [Route("SuccessPage")]
        public IActionResult VerifyOtpSuccess(string email)
        {
            return View((object)email);
        }

    }
}
