using Microsoft.AspNetCore.Mvc;
using WAS.Web.Models;

namespace WAS.Web.Controllers
{
    [Route("WAS/Forbidden")]
    public class ForbiddenController : Controller
    {
        public ForbiddenController()
        {
        }

        [Route("Error")]
        public IActionResult Index(int? errorStatusCode = null)
        {
            ErrorViewModel errorViewModel = new ErrorViewModel
            {
                ErrorStatusCode = errorStatusCode
            };

            if (errorStatusCode == 403)
            {
                errorViewModel.ErrorType = "Forbidden - Access Denied";
                errorViewModel.ErrorMessage = "You do not have access or You're trying to access the page which is to be accessed only from within West Network. Please connect to West Network either directly or via VPN client.";
            }
            else if (errorStatusCode == 404)
            {
                errorViewModel.ErrorType = "Not Found";
                errorViewModel.ErrorMessage = "The requested page not found.";
            }
            else if (errorStatusCode == 500)
            {
                errorViewModel.ErrorType = "Internal Server Error";
                errorViewModel.ErrorMessage = "Something has gone wrong";
            }
            else if (errorStatusCode == 502)
            {
                errorViewModel.ErrorType = "Bad Gateway";
                errorViewModel.ErrorMessage = "Application is not receiving a valid response from the backend servers";
            }
            else if (errorStatusCode == 503)
            {
                errorViewModel.ErrorType = "Service Unavailable";
                errorViewModel.ErrorMessage = "Application is overloaded or under maintenance";
            }
            else if (errorStatusCode == 504)
            {
                errorViewModel.ErrorType = "Gateway Timeout";
                errorViewModel.ErrorMessage = "The network connection between the servers is poor";
            }
            else
            {
                errorViewModel.ErrorType = "Error";
                errorViewModel.ErrorMessage = "An error occurred while processing your request.";
            }

            return View(errorViewModel);
        }
    }
}
