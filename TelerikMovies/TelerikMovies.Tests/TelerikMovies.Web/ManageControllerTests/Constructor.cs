using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web.Controllers;

namespace TelerikMovies.Tests.TelerikMovieWeb.ManageControllerTests
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

            Assert.IsInstanceOfType(sut, typeof(ManageController));
        }

        [TestMethod]
        public void With1ArgumentShouldThrowIfArgumentsAreNull()
        {
            var moqUserManager = new Mock<IUserManagerService>();
            ISignInManagerService moqsingInManager = null;

            Assert.ThrowsException<ArgumentNullException>(()=> { new ManageController(moqUserManager.Object, moqsingInManager); });
        }

        private ManageController SetupController()
        {
            var moqUserManager = new Mock<IUserManagerService>();
            var moqsingInManager = new Mock<ISignInManagerService>();

            var result = new TelerikMovies.Web.Controllers.ManageController(moqUserManager.Object,moqsingInManager.Object);
            return result;
        }
    }
}
