using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;

namespace TelerikMovies.Tests.TelerikMoviesWeb.MoviesControllerTests
{

    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void WhitAllArgumentsShouldReturnInstanceOfAccountController()
        {
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            Assert.IsInstanceOfType(sut, typeof(MoviesController));
        }

        [TestMethod]
        public void ShouldThrowIfArgumentsAreNull()
        {
            IMoviesService moqMoviesService = null;

            Assert.ThrowsException<ArgumentNullException>(()=> { new MoviesController(moqMoviesService); });
        }

    }
}
