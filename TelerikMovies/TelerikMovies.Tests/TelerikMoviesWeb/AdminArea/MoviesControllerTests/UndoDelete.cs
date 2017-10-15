using AutoMapper;
using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
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
    public class Delete
    {
        [TestMethod]
        public void ShouldReturnBadRequestIfEmptyCollection()
        {
            var ids = new Guid[2];
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Delete(ids))
              .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public void ShouldReturnBadRequestIfCollectionIsNull()
        {
            Guid[] ids =null;
            var moqMoviesService = new Mock<IMoviesService>();
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Delete(ids))
              .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfCollectionGotIdsThatAreNull()
        {
            var success = new Result(ResultType.Success);
            var fakeGuid = "b1090055-59de-402d-a067-678277864d56";
            Guid[] ids = new Guid[2];
            ids[0] = new Guid(fakeGuid);

            var moqMoviesService = new Mock<IMoviesService>();
            moqMoviesService.Setup(x => x.DeleteByid(It.IsAny<Guid>())).Returns(success);
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Delete(ids))
              .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfCollectionDeletionReturnsError()
        {
            var error = new Result(ResultType.Error);
            var fakeGuid = "b1090055-59de-402d-a067-678277864d56";
            Guid[] ids = new Guid[1];
            ids[0] = new Guid(fakeGuid);
            var moqMoviesService = new Mock<IMoviesService>();
            moqMoviesService.Setup(x => x.DeleteByid(It.IsAny<Guid>())).Returns(error);
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Delete(ids))
              .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnOkIfCollectionIsOkAndDeletionIsSuccesfull()
        {
            var success = new Result(ResultType.Success);
            var fakeGuid = "b1090055-59de-402d-a067-678277864d56";
            Guid[] ids = new Guid[1];
            ids[0] = new Guid(fakeGuid);
            var moqMoviesService = new Mock<IMoviesService>();
            moqMoviesService.Setup(x => x.DeleteByid(It.IsAny<Guid>())).Returns(success);
            var sut = new MoviesController(moqMoviesService.Object);

            sut
              .WithCallTo(c => c.Delete(ids))
              .ShouldGiveHttpStatus(HttpStatusCode.OK);
        }


    }
}
