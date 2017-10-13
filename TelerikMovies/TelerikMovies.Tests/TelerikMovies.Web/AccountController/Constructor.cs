using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web;

namespace TelerikMovies.Tests.TelerikMovieWeb.AccountController
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void Whit1ArgumentShouldReturnInstanceOfAccountControllerWhenArgumentsAreValid()
        {
            var moqUserSv = new Mock<IUsersService>();

            var sut = new TelerikMovies.Web.Controllers.AccountController(moqUserSv.Object);

            Assert.IsInstanceOfType(sut, typeof(TelerikMovies.Web.Controllers.AccountController));
        }

        [TestMethod]
        public void With1ArgumentShouldThrowIfArgumentsAreNull()
        {
            IUsersService moqUserSv = null;

            Assert.ThrowsException<NullReferenceException>(()=> { new TelerikMovies.Web.Controllers.AccountController(moqUserSv); });
        }

        [TestMethod]
        public void Whit3ArgumentsShouldThrowIfArgumentsAreNull()
        {
            IUsersService moqUserSv = null;
            IUserManagerService usMnger = null;
            ISignInManagerService sginMnger = null;

            Assert.ThrowsException<NullReferenceException>(() => { new TelerikMovies.Web.Controllers.AccountController(moqUserSv, sginMnger, usMnger ); });
        }

        [TestMethod]
        public void Whit3ArgumentsShouldThrowIfArgumentsManagersAreNull()
        {
            var moqUserSv = new Mock<IUsersService>(); ;
            IUserManagerService usMnger = null;
            ISignInManagerService sginMnger = null;

            Assert.ThrowsException<NullReferenceException>(() => { new TelerikMovies.Web.Controllers.AccountController(moqUserSv.Object, sginMnger, usMnger ); });
        }
    }
}
