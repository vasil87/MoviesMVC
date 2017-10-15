using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.MoviesControllerTests
{

    [TestClass]
    public class Constructor
    {

        [TestMethod]
        public void ShouldReturnInstanceOfMoviesController()
        {
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            Assert.IsInstanceOfType(sut, typeof(MoviesController));

        }

        [TestMethod]
        public void ShouldThrowIfArgumentsAreNull()
        {
            IMoviesService moqMoviesService = null;

            Assert.ThrowsException<ArgumentNullException>(() => { new MoviesController(moqMoviesService); });
        }
    }
}
