using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.GenresControllerTests
{

    [TestClass]
    public class Index
    {

        [TestMethod]
        public void ShouldRedirect()
        {
            var moqGenresService = new Mock<IGenreService>();
            var sut = new GenresController(moqGenresService.Object);

            sut
               .WithCallTo(c => c.Index())
               .ShouldRedirectTo(x => x.Create);

        }
    }
}
