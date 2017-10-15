using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.MoviesControllerTests
{

    [TestClass]
    public class Index
    {

        [TestMethod]
        public void ShouldReturnDefaultView()
        {
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            sut
               .WithCallTo(c => c.Index())
               .ShouldRenderDefaultView();    
        }
    }
}
