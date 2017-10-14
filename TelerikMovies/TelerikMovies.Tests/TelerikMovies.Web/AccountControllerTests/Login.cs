using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;
using TelerikMovies.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;

namespace TelerikMovies.Tests.TelerikMovieWeb.AccountControllerTests
{

    [TestClass]
    public class Login
    {

        [TestMethod]
        public void ReturnViewWithReturnUrlInViewBag()
        {
            // Arrange
            var sut = setUpControler();

            string returnUrl = "url";


            // Act & Assert
            sut
                .WithCallTo(c => c.Login(returnUrl))
                .ShouldRenderDefaultView();

            Assert.AreEqual(returnUrl, sut.ViewBag.ReturnUrl);
        }

        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModelWrongEmail()
        {
            // Arrange
            var sut = setUpControler();

            var loginModel = new LoginViewModel() { Email = "vasil@abv.bg", Password = "123456", RememberMe = false };
            sut.ModelState.AddModelError("Email", "Ivalid Email");
            string returnUrl = "url";

            // Act & Assert
            sut
                .WithCallTo(c => c.Login(loginModel, returnUrl))
                .ShouldRenderDefaultView()
                 .WithModel<LoginViewModel>().AndModelErrorFor(m => m.Email);
        }

        [TestMethod]
        public void ReturnSuccessWhenValidData()
        {
            var urlHelperMock = new UrlHelperMocked();
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            signInServiceManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInStatus.Success); ;
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            sut.Url = urlHelperMock;
            var loginModel = new LoginViewModel() { Email = "vasil@abv.bg", Password = "123456", RememberMe = false };
            string returnUrl = "/home/index";

            // Act & Assert
            sut
            .WithCallTo(c => c.Login(loginModel, returnUrl))
           .ShouldRedirectTo(returnUrl);
        }

        [TestMethod]
        public void ReturnInvalidAndReturnsErrorWhenInValidData()
        { 
            // Arrange
            var urlHelperMock = new UrlHelperMocked();
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            signInServiceManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).ReturnsAsync(SignInStatus.Failure); ;
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            sut.Url = urlHelperMock;
            var loginModel = new LoginViewModel() { Email = "vasil@abv.bg", Password = "123456", RememberMe = false };
            string returnUrl = "/home/index";

            // Act & Assert
            sut
            .WithCallTo(c => c.Login(loginModel, returnUrl))
            .ShouldRenderDefaultView()
            .WithModel<LoginViewModel>().AndModelError(string.Empty);
        }

        private TelerikMovies.Web.Controllers.AccountController setUpControler()
        {
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var accountController = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            return accountController;
        }


        private class UrlHelperMocked : UrlHelper
        {
            public override bool IsLocalUrl(string url)
            {
                return true;
            }
        }
    }
}
