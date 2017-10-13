using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMovieWeb.AccountController
{

    [TestClass]
    public class About
    {
        [TestMethod]
        public void ReturnViewWithReturnUrlInViewBag()
        {
            // Arrange
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();

            string returnUrl = "url";

            var accountController = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);

            // Act & Assert
            accountController
                .WithCallTo(c => c.Login(returnUrl))
                .ShouldRenderDefaultView();

            Assert.AreEqual(returnUrl, accountController.ViewBag.ReturnUrl);
        }
    }
}
