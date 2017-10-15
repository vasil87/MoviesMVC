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

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.GenresControllerTests
{

    [TestClass]
    public class Create
    {
        [TestMethod]
        public void ReturnsDefaultView()
        {
            // Arrange
            var genreSvMock = new Mock<IGenreService>();
            var sut = new GenresController(genreSvMock.Object);

            // Act & Assert
            sut
                .WithCallTo(c => c.Create())
                .ShouldRenderDefaultView();
        }
        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModel()
        {
            // Arrange
            var genreSvMock = new Mock<IGenreService>();
            var fakeGenre = new GenreCreateViewModel();
            var sut = new GenresController(genreSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };

            sut.ModelState.AddModelError("Name", "Ivalid genre name");

            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeGenre))
                .ShouldRenderDefaultView()
                 .WithModel<GenreCreateViewModel>()
                 .AndModelErrorFor(m => m.Name);
        }

        [TestMethod]
        public void ReturnSameViewIfErrorOnSave()
        {
            // Arrange
            var error = new Result(ResultType.Error);
            var genreSvMock = new Mock<IGenreService>();
            genreSvMock.Setup(x => x.AddGenre(It.IsAny<Genres>())).Returns(error);
            var fakeGenre = new GenreCreateViewModel();
            var sut = new GenresController(genreSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<GenreCreateViewModel, Genres>());


            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeGenre))
                .ShouldRenderDefaultView()
                 .WithModel<GenreCreateViewModel>();

            Assert.AreEqual(fakeGenre.Result.ResulType, ResultType.Error);
        }

        [TestMethod]
        public void ShouldReturnDefaultViewModelWithNoErrorWhenModelIsValid()
        {
            // Arrange
            var success = new Result(ResultType.Success);
            var genreSvMock = new Mock<IGenreService>();
            genreSvMock.Setup(x => x.AddGenre(It.IsAny<Genres>())).Returns(success);
            var fakeGenre = new GenreCreateViewModel();
            var sut = new GenresController(genreSvMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<GenreCreateViewModel, Genres>());

            // Act & Assert
            sut
                .WithCallTo(c => c.Create(fakeGenre))
                .ShouldRenderDefaultView()
                 .WithModel<GenreCreateViewModel>(x => Assert.AreEqual(x.Result.ResulType, ResultType.Success));

        }

    }
}
