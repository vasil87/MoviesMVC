using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMovieWeb.AccountController
{
    [TestClass]
    public class LogOff
    {
        [TestMethod]
        public void ShouldRedirectToIndex()
        {
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var accountController = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);

            accountController.WithCallTo(c => c.LogOff()).ShouldRedirectTo<TelerikMovies.Web.Controllers.HomeController>(c2 => c2.Index());

        }
    }
}
