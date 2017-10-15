using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TelerikMovies.Web.Models.Movie;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.MoviesControllerTests
{
    [TestClass]
    public class RenderTopMovies
    {
        [TestMethod]
        public void ShouldRenderTopMovies()
        {
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);
            moqMoviesService.Setup(x => x.GetTopMovies()).Returns(new List<Movies>());

            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Movies, SimpleMovieViewModel>());
         
            sut
              .WithCallTo(c => c.RenderTopMovies())
              .ShouldRenderPartialView("_TopMovies")
              .WithModel<ICollection<SimpleMovieViewModel>>();
        }  
    }
}
