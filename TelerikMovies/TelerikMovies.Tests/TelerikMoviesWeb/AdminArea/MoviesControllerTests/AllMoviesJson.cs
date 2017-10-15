using AutoMapper;
using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Controllers;
using TelerikMovies.Web.Areas.Admin.Models;
using TelerikMovies.Web.Areas.Admin.Models.Contracts;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.GenresControllerTests
{

    [TestClass]
    public class AllMoviesJson
    {
        [TestMethod]
        public void ShouldReturnAllMoviesAsJson()
        {
            // Arrange
            const int  draw = 1;
            const int start = 0;
            const int length = 10;
            ICollection<Movies> fakeMovies = new List<Movies>();
            fakeMovies.Add(new Movies() { Name = "fakeName"});
            fakeMovies.Add(new Movies() { Name = "shouldShowName"});
            var success = new Result(ResultType.Success);
            var moviesSvMock = new Mock<IMoviesService>();
            moviesSvMock.Setup(x => x.GetAllAndDeleted()).Returns(fakeMovies);
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Movies, GridMovieViewModel>()
                 .ForMember(gridModel => gridModel.Likes, cf => cf.MapFrom(movie => movie.Likes.Count))
                 .ForMember(gridModel => gridModel.Dislikes, cf => cf.MapFrom(movie => movie.Dislikes.Count)));
            var sut = new MoviesControllerForTest(moviesSvMock.Object);

            // Act & Assert
            sut
                .WithCallTo(c => c.AllMoviesJson(draw, start, length))
                .ShouldReturnJson();

            Assert.AreEqual(sut.Called, 1);
        }

        private class MoviesControllerForTest:MoviesController
        {
            public MoviesControllerForTest(IMoviesService service):base(service)
            {
                this.Called = 0;
            }
            public int Called { get; set; }


            protected override void FillDataTable<T>(IDataTableViewModel<T> dataTableData, List<T> allData, int draw, int totalCount, int start = 0, int length = 10)
            {
                this.Called++;
            }
        }

    }
}
