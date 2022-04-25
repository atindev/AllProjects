using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WAS.Web.Controllers
{
    [Route("WAS/Account")]
    public class AccountController : Controller
    {

        [HttpGet]
        [Route("SignIn")]
        public IActionResult SignIn(Guid id)
        {
            var redirectUrl = "";
            if (id == Guid.Empty)
                redirectUrl = Url.Action("View", "WAS/Dashboard");
            else
            {
                redirectUrl = Url.Action("StartSurvey", "WAS/Survey");
                HttpContext.Session.SetString("SSOLogin", "true");
                redirectUrl += "?id=" + id;
            }
            
            redirectUrl = redirectUrl.Replace("%2F", "/");
            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("SignOut")]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("SignedOut")]
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if the user is authenticated.
                return RedirectToAction(nameof(SubscriptionController.Index), "Subscription");
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Route("SignInAgain")]
        public IActionResult SignInAgain()
        {
            var callbackUrl = Url.Action(nameof(SignIn), "Account", values: null, protocol: Request.Scheme);
            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        [Route("UnsupportedBrowser")]
        public IActionResult UnsupportedBrowser()
        {
            return View();
        }
    }
}
