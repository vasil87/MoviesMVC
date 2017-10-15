using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.Models;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.ManageControllerTests
{

    [TestClass]
    public class Index
    {
        [TestMethod]
        public void ShouldReturnViewWithRightModel()
        {
            // Arrange
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>(); 
            var fakeUser = new Users();
            userServiceManagerMock.Setup(x => x.FindById(It.IsAny<string>())).Returns(fakeUser);
            userServiceManagerMock.Setup(x => x.GetPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync("123456");
            userServiceManagerMock.Setup(x => x.GetTwoFactorEnabledAsync(It.IsAny<string>())).ReturnsAsync(false);
            userServiceManagerMock.Setup(x => x.GetLoginsAsync(It.IsAny<string>())).ReturnsAsync(new List<UserLoginInfo>());
            var sut = new ManageController(userServiceManagerMock.Object,signInServiceManagerMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var data = new Dictionary<string, object>()
                  {
                      {"a", "b"}
                  };
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            fakeHttpContext.Setup(x => x.Items[It.IsAny<string>()]).Returns(data);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };

            // Act & Assert
            sut
                .WithCallTo(c => c.Index(null))
                .ShouldRenderDefaultView()
                .WithModel<IndexViewModel>();
        }
    }
}
