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
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AdminArea.GenresControllerTests
{

    [TestClass]
    public class GetAllNamesNotExpired
    {
        [TestMethod]
        public void ShouldReturnAllMoviesNotExpiredNames()
        {
            // Arrange
            ICollection<Genres> fakeGenres = new List<Genres>();
            fakeGenres.Add(new Genres() { Name = "fakeName"});
            fakeGenres.Add(new Genres() { Name = "shouldShowName"});
            var success = new Result(ResultType.Success);
            var genreSvMock = new Mock<IGenreService>();
            genreSvMock.Setup(x => x.GetAllNotExpired()).Returns(fakeGenres);
            var sut = new GenresController(genreSvMock.Object);

            // Act & Assert
            sut
                .WithCallTo(c => c.GetAllNamesNotExpired())
                .ShouldReturnJson();
        }

    }
}
