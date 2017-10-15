using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.GenresControllerTests
{

    [TestClass]
    public class Constructor
    {

        [TestMethod]
        public void ShouldReturnInstanceOfGenresController()
        {
            var moqGenresService = new Mock<IGenreService>();
            var sut = new GenresController(moqGenresService.Object);

            Assert.IsInstanceOfType(sut, typeof(GenresController));

        }

        [TestMethod]
        public void ShouldThrowIfArgumentsAreNull()
        {
            IGenreService moqGenresService = null;

            Assert.ThrowsException<ArgumentNullException>(() => { new GenresController(moqGenresService); });
        }
    }
}
