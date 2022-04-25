using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;
using xUnitTestController;
using xUnitTestController.Controllers;

namespace TestingControllers
{
    public class UnitTest1
    {
        public readonly AdminController adminController;
        public readonly Mock<IHomeRepository> mockRepo;
        public const string User = "test@westpharma.com";

        public UnitTest1()
        {
            mockRepo = new Mock<IHomeRepository>();

            ////ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            ////TempDataDictionaryFactory tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider);
            ////ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            var user = new { UserName = User, Id = "1" };
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("name", user.UserName),
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity);
            mockPrincipal.Setup(x => x.IsInRole(It.IsAny<string>())).Returns(true);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(m => m.User)
                .Returns(claimsPrincipal);

            ////var projectControllerContext = new Mock<ControllerContext>();
            ////projectControllerContext.SetupGet(x => x.HttpContext)
            ////    .Returns(mockHttpContext.Object);

            var tempData = new TempDataDictionary(mockHttpContext.Object, Mock.Of<ITempDataProvider>());
            var projectControllerContext1 = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object,
            };

            adminController = new AdminController(mockRepo.Object);
            ////adminController.ControllerContext = projectControllerContext.Object;
            adminController.ControllerContext = projectControllerContext1;
            adminController.TempData = tempData;
        }

        [Fact]
        public async Task Test1()
        {
            try
            {
                //Setup
                mockRepo.Setup(x => x.RemoveProject(1, It.IsAny<string>()))
                    .ReturnsAsync($"{1} {User}");

                //Act
                var data = await adminController.RemoveProject(1);

                //Verify
                RedirectToActionResult output = Assert.IsType<RedirectToActionResult>(data);
                Assert.Equal("AdminDashboard", output.RouteValues["Action"]);
                Assert.Equal("Admin", output.RouteValues["Controller"]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Assert.Null(ex);
            }
        }
    }
}
