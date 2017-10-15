using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AccountControllerTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void WhitAllArgumentsShouldReturnInstanceOfAccountController()
        {
            var sut = this.SetupController();

            Assert.IsInstanceOfType(sut, typeof(TelerikMovies.Web.Controllers.AccountController));
        }

        [TestMethod]
        public void With1ArgumentShouldThrowIfArgumentsAreNull()
        {
            var moqUserSv = new Mock<IUsersService>();
            IUserManagerService moqUserManager = null;
            ISignInManagerService moqsingInManager = null;

            Assert.ThrowsException<ArgumentNullException>(()=> { new TelerikMovies.Web.Controllers.AccountController(moqUserSv.Object, moqsingInManager, moqUserManager); });
        }

        private TelerikMovies.Web.Controllers.AccountController SetupController()
        {
            var moqUserSv = new Mock<IUsersService>();
            var moqUserManager = new Mock<IUserManagerService>();
            var moqsingInManager = new Mock<ISignInManagerService>();

            var result = new TelerikMovies.Web.Controllers.AccountController(moqUserSv.Object, moqsingInManager.Object, moqUserManager.Object);
            return result;
        }
    }
}
