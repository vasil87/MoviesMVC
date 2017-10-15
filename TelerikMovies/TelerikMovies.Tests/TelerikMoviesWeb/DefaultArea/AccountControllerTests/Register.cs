using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;
using TelerikMovies.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using TelerikMovies.Models;
using Microsoft.AspNet.Identity;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AccountControllerTests
{

    [TestClass]
    public class Register
    {

        [TestMethod]
        public void ReturnViewWithReturnUrlInViewBag()
        {
            // Arrange
            var sut = setUpControler();


            // Act & Assert
            sut
                .WithCallTo(c => c.Register())
                .ShouldRenderDefaultView();


        }

        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModelWrongEmail()
        {
            // Arrange
            var sut = setUpControler();

            var registerModel = new RegisterViewModel() { Email = "vasil@abv.bg", Password = "123456", ConfirmPassword="123456" };
            sut.ModelState.AddModelError("Email", "Ivalid Email");

            // Act & Assert
            sut
                .WithCallTo(c => c.Register(registerModel))
                .ShouldRenderDefaultView()
                 .WithModel<RegisterViewModel>().AndModelErrorFor(m => m.Email);
        }

        [TestMethod]
        public void RedirectsToHomeControllerIndexAndCallsSignIn()
        {
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();       
            var userServiceManagerMock = new Mock<IUserManagerService>();
            userServiceManagerMock.Setup(x => x.CreateAsync(It.IsAny<Users>(),It.IsAny<string>())).ReturnsAsync(IdentityResult.Success); 
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            var registerModel = new RegisterViewModel() { Email = "vasil@abv.bg", Password = "123456", ConfirmPassword = "123456" };

            // Act & Assert
            sut
            .WithCallTo(c => c.Register(registerModel))
           .ShouldRedirectTo<TelerikMovies.Web.Controllers.HomeController>(m=>m.Index());

            signInServiceManagerMock.Verify(x => x.SignInAsync(It.IsAny<Users>(),It.IsAny<bool>(), It.IsAny<bool>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public void ReturnInvalidAndReturnsErrorWhenInValidData()
        {
            // Arrange
            var error = "Error";
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            userServiceManagerMock.Setup(x => x.CreateAsync(It.IsAny<Users>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(error));
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            var registerModel = new RegisterViewModel() { Email = "vasil@abv.bg", Password = "123456", ConfirmPassword = "123456" };

            // Act & Assert
            sut
            .WithCallTo(c => c.Register(registerModel))
            .ShouldRenderDefaultView()
            .WithModel<RegisterViewModel>()
            .AndModelError(string.Empty);
        }

        private TelerikMovies.Web.Controllers.AccountController setUpControler()
        {
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var accountController = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            return accountController;
        }
    }
}
