using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TelerikMovies.Web.Models.Comment;
using TelerikMovies.Web.Models.Movie;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.CommentsControllerTests
{
    [TestClass]
    public class Details
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
                  .WithCallTo(c => c.Details(id))
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
                 cfg.CreateMap<Movies, DetailedMovieViewModel>()
                .ForMember(detailedModel => detailedModel.Likes, cf => cf.MapFrom(movie => movie.Likes.Count))
                .ForMember(detailedModel => detailedModel.Dislikes, cf => cf.MapFrom(movie => movie.Dislikes.Count)));
               
            //Act & Assert
            sut
              .WithCallTo(c => c.Details(id))
              .ShouldRenderDefaultView()
              .WithModel<DetailedMovieViewModel>();
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
                 cfg.CreateMap<Movies, DetailedMovieViewModel>()
                .ForMember(detailedModel => detailedModel.Likes, cf => cf.MapFrom(movie => movie.Likes.Count))
                .ForMember(detailedModel => detailedModel.Dislikes, cf => cf.MapFrom(movie => movie.Dislikes.Count)));

            //Act & Assert
            sut
              .WithCallTo(c => c.Details(id))
              .ShouldRenderView("404");
        }

    }
}
