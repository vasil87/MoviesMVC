using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.MoviesControllerTests
{
    [TestClass]
    public class Index
    {
        [TestMethod]
        public void WhitAllArgumentsShouldReturnInstanceOfAccountController()
        {
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Index())
              .ShouldRenderDefaultView();
        }
    }
}
