using AutoMapper;
using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TelerikMovies.Web.Areas.Admin.Models;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.MoviesControllerTests
{

    [TestClass]
    public class Create
    {
        [TestMethod]
        public void ReturnsDefaultView()
        {
            // Arrange
            var moviesSvMock = new Mock<IMoviesService>();
            var sut = new MoviesController(moviesSvMock.Object);

            // Act & Assert
            sut
                .WithCallTo(c => c.Create())
                .ShouldRenderDefaultView()
                .WithModel<MovieCreateViewModel>();
        }
        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModel()
        {
            // Arrange
            var moviesSvMock = new Mock<IMoviesService>();
            var fakeMovie = new MovieCreateViewModel();
            var sut = new MoviesController(moviesSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };

            sut.ModelState.AddModelError("Name", "Ivalid movie name");

            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieCreateViewModel>()
                 .AndModelErrorFor(m => m.Name);
        }

        [TestMethod]
        public void ReturnSameViewIfErrorOnSave()
        {
            // Arrange
            var error = new Result(ResultType.Error);
            var moviesSvMock = new Mock<IMoviesService>();
            moviesSvMock.Setup(x => x.AddMovie(It.IsAny<Movies>())).Returns(error);
            var fakeMovie = new MovieCreateViewModel();
            var sut = new MoviesController(moviesSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<MovieCreateViewModel, Movies>());


            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieCreateViewModel>();

            Assert.AreEqual(fakeMovie.Result.ResulType, ResultType.Error);
        }

        [TestMethod]
        public void ShouldReturnDefaultViewModelWithNoErrorWhenModelIsValid()
        {
            // Arrange
            var success = new Result(ResultType.Success);
            var moviesSvMock = new Mock<IMoviesService>();
            moviesSvMock.Setup(x => x.AddMovie(It.IsAny<Movies>())).Returns(success);
            var fakeMovie = new MovieCreateViewModel();
            var sut = new MoviesController(moviesSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<MovieCreateViewModel, Movies>());


            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieCreateViewModel>(x => Assert.AreEqual(x.Result.ResulType, ResultType.Success));

        }

    }
}
