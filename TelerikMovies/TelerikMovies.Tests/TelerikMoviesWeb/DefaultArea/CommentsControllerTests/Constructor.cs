using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web.Controllers;

namespace TelerikMovies.Tests.TelerikMoviesWeb.CommentsControllerTests
{

    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void WhitAllArgumentsShouldReturnInstanceOfAccountController()
        {
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            Assert.IsInstanceOfType(sut, typeof(CommentsController));
        }

        [TestMethod]
        public void ShouldThrowIfArgumentsAreNull()
        {
            ICommentsService moqUserService = null;

            Assert.ThrowsException<ArgumentNullException>(()=> { new CommentsController(moqUserService); });
        }

       
    }
}
