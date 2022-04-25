namespace xUnitTestController.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class AdminController : Controller
    {
        private readonly IHomeRepository homeRepository;
        ////private IAdUserHelper _adUserHelper;
        ////private IAdminRepository _adminRepository;
        ////private IHomeRepository _homeRepository;

        public AdminController(IHomeRepository homeRepository)
        {
            this.homeRepository = homeRepository;
        }

        ////public async Task<IActionResult> AdminDashboard()
        ////{
        ////    var email = User.Identity.Name;
        ////    AdminViewModel adminViewModel = new AdminViewModel();
        ////    _adUserHelper = new AdUserHelper(_graphSdkHelper, User);
        ////    var loggedInUserDetails = await _adUserHelper.GetAdUserDetails(HttpContext, email);
        ////    adminViewModel.UserDetails = loggedInUserDetails;
        ////    var projectUserAccessList = await _adminRepository.GetAllAuthorizedProjectUsers();
        ////    foreach (var user in projectUserAccessList)
        ////    {
        ////        user.Name = await _adUserHelper.GetDataScienceMemberName(HttpContext, user.EmployeeID);
        ////        user.PictureBase64 = await _adUserHelper.GetTeamAdUserPicture(HttpContext, user.EmployeeID);
        ////    }

        ////    adminViewModel.AllProjects = _adminRepository.GetAllProjects(projectUserAccessList);

        ////    return View(adminViewModel);
        ////}

        ////[HttpPost]
        ////[Route("/Admin/RemoveUserAccess")]
        ////public async Task<IActionResult> RemoveUserAccess(int ProjectID, string EmployeeID, string ProjectAccessList)
        ////{
        ////    if (ProjectID != 0)
        ////    {
        ////        var res = _adminRepository.RemoveUserAccess(EmployeeID, ProjectAccessList, ProjectID);
        ////        TempData.Put("Response", res);
        ////    }
        ////    else
        ////    {
        ////        TempData.Put("Response", new OperationResponse() { StatusCode = 0, Message = $"Project Access could not be retrieved. Please report this bug!" });
        ////    }

        ////    return RedirectToAction("AdminDashboard", "Admin");
        ////}

        ////[HttpPost]
        ////[Route("/Admin/GrantProjectAccess")]
        ////public async Task<IActionResult> GrantProjectAccess(string EmployeeID, List<int> GrantedProjects)
        ////{
        ////    if (GrantedProjects.Count != 0)
        ////    {
        ////        var res = await _adminRepository.GrantProjectAccess(EmployeeID, GrantedProjects);
        ////        TempData.Put("Response", res);
        ////    }
        ////    else
        ////    {
        ////        TempData.Put("Response", new OperationResponse() { StatusCode = 0, Message = $"No project selected. Please select atleast 1 project to grant access to { EmployeeID }!" });
        ////    }

        ////    return RedirectToAction("AdminDashboard", "Admin");
        ////}

        ////[HttpPost]
        ////[Route("/Admin/UpdateProject")]
        ////public IActionResult UpdateProject(ProjectDto projectDto)
        ////{
        ////    if (projectDto.ProjectID != 0)
        ////    {
        ////        var res = _adminRepository.UpdateProject(projectDto);
        ////        TempData.Put("Response", res);
        ////    }
        ////    else
        ////    {
        ////        TempData.Put("Response", new OperationResponse() { StatusCode = 0, Message = $"Some fatal error occured while updating the { projectDto.ProjectName } Project. Please report this through the feedback!" });
        ////    }

        ////    return RedirectToAction("AdminDashboard", "Admin");
        ////}

        [HttpPost]
        [Route("/Admin/RemoveProject")]
        public async Task<IActionResult> RemoveProject(int projectID)
        {
            if (projectID != 0)
            {
                string email = User.Identity.Name;
                ///string email = "test@westpharma.com";
                var res = await homeRepository.RemoveProject(projectID, email);
                TempData.Put("Response", res);
            }
            else
            {
                TempData.Put("Response", new { StatusCode = 0, Message = $"Project could not be deleted. Please report this bug!" });
            }

            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}
