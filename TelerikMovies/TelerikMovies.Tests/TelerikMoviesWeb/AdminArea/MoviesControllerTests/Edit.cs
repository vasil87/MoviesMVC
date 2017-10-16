using AutoMapper;
using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
    public class Edit
    {
        [TestMethod]
        public void ShouldReturn404ViewIfInvalidId()
        {
            var rows = new List<string>();
            rows.Add(null);
            rows.Add(string.Empty);
            rows.Add("asdf");
            foreach (var id in rows)
            {
                var moqMoviesService = new Mock<IMoviesService>();
                var sut = new MoviesController(moqMoviesService.Object);

                sut
                  .WithCallTo(c => c.Edit(id))
                  .ShouldRenderView("404");
            }
           
        }
        [TestMethod]
        public void ShouldReturnViewWithRightModel()
        {
            //Arrange
            var id = "b1090055-59de-402d-a067-678277864d56";
            var moqMoviesService = new Mock<IMoviesService>();
            moqMoviesService.Setup(x => x.GetMovieById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new Movies());
            var sut = new MoviesController(moqMoviesService.Object);
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Movies, MovieEditViewModel>());

            //Act & Assert
            sut
              .WithCallTo(c => c.Edit(id))
              .ShouldRenderDefaultView()
              .WithModel<MovieEditViewModel>();
        }

        [TestMethod]
        public void ShouldReturn404IfNoMovieFound()
        {
            //Arrange
            var id = "b1090055-59de-402d-a067-678277864d56";
            var moqMoviesService = new Mock<IMoviesService>();
            moqMoviesService.Setup(x => x.GetMovieById(It.IsAny<Guid>(), It.IsAny<bool>())).Returns((Movies)null);
            var sut = new MoviesController(moqMoviesService.Object);
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Movies, MovieEditViewModel>());

            //Act & Assert
            sut
              .WithCallTo(c => c.Edit(id))
              .ShouldRenderView("404");
        }
        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModel()
        {
            // Arrange
            var moviesSvMock = new Mock<IMoviesService>();
            var fakeMovie = new MovieEditViewModel();
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
                .WithCallTo(c => c.Edit(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieEditViewModel>()
                 .AndModelErrorFor(m => m.Name);
        }

        [TestMethod]
        public void ReturnSameViewIfErrorOnUpdate()
        {
            // Arrange
            var error = new Result(ResultType.Error);
            var moviesSvMock = new Mock<IMoviesService>();
            moviesSvMock.Setup(x => x.UpdateMovie(It.IsAny<Movies>())).Returns(error);
            var fakeMovie = new MovieEditViewModel();
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
                 cfg.CreateMap<MovieEditViewModel, Movies>());


            // Act & Assert
            sut
                .WithCallTo(c => c.Edit(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieEditViewModel>();

            Assert.AreEqual(fakeMovie.Result.ResulType, ResultType.Error);
        }

        [TestMethod]
        public void ShouldReturnDefaultViewModelWithNoErrorWhenModelIsValid()
        {
            // Arrange
            var success = new Result(ResultType.Success);
            var moviesSvMock = new Mock<IMoviesService>();
            moviesSvMock.Setup(x => x.UpdateMovie(It.IsAny<Movies>())).Returns(success);
            var fakeMovie = new MovieEditViewModel();
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
                 cfg.CreateMap<MovieEditViewModel, Movies>());


            // Act & Assert
            sut
                .WithCallTo(c => c.Edit(fakeMovie))
                .ShouldRenderDefaultView()
                 .WithModel<MovieEditViewModel>(x => Assert.AreEqual(x.Result.ResulType, ResultType.Success));

        }
    }
}
