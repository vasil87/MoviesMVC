using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web.Controllers;

namespace TelerikMovies.Tests.TelerikMoviesWeb.VoteControllerTests
{

    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void WhitAllArgumentsShouldReturnInstanceOfAccountController()
        {
            var moqVoteService = new Mock<IVoteService>();
            var sut = new VoteController(moqVoteService.Object);

            Assert.IsInstanceOfType(sut, typeof(VoteController));
        }

        [TestMethod]
        public void ShouldThrowIfArgumentsAreNull()
        {
            IVoteService moqVoteService = null;

            Assert.ThrowsException<ArgumentNullException>(()=> { new VoteController(moqVoteService); });
        }

       
    }
}
