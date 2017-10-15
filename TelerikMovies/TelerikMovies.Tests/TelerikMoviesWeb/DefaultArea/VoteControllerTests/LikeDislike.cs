using AutoMapper;
using Common;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using TelerikMovies.Web.Models.Comment;
using TelerikMovies.Web.Models.LikesDislikes;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.VoteControllerTests
{
    [TestClass]
    public class LikeDislike
    {

        [TestMethod]
        public void ShouldReturnBadRequestIfModelisNull()
        {
            var mockVoteService = new Mock<IVoteService>();
            var sut = new VoteController(mockVoteService.Object);

            sut
              .WithCallTo(c => c.Like(null))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfMovieIdisNull()
        {
            VoteViewModel model = new VoteViewModel();
            model.UserName = "UserName";
            var mockVoteService = new Mock<IVoteService>();
            var sut = new VoteController(mockVoteService.Object);

            sut
              .WithCallTo(c => c.Like(model))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }
        [TestMethod]
        public void ShouldReturnBadRequestIfUserNameIsNull()
        {
            VoteViewModel model = new VoteViewModel();
            model.MovieId = new Guid();
            var mockVoteService = new Mock<IVoteService>();
            var sut = new VoteController(mockVoteService.Object);

            sut
              .WithCallTo(c => c.Like(model))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnInternalServerErrorIfValidModelButErrorLikeDislike()
        {

            //Arrange
            VoteViewModel model = new VoteViewModel();
            model.MovieId = new Guid("cd382690-c68e-42a0-87c0-d4f9ab5fb002");
            model.UserName = "UserName";
            var errorResult = new Result(ResultType.Error);
            var voteService = new Mock<IVoteService>();
            voteService.Setup(x => x.LikeOrDislikeAMovie(model.UserName,model.MovieId,It.IsAny<bool>())).Returns(errorResult);
            var sut = new VoteController(voteService.Object);
           
            //Act & Assert
            sut
              .WithCallTo(c => c.Like(model))
              .ShouldGiveHttpStatus(HttpStatusCode.InternalServerError);
            sut
             .WithCallTo(c => c.Dislike(model))
             .ShouldGiveHttpStatus(HttpStatusCode.InternalServerError);
        }


        [TestMethod]
        public void ShouldReturnOKIfValidModelAndValidSave()
        {

            //Arrange
            VoteViewModel model = new VoteViewModel();
            model.MovieId = new Guid("cd382690-c68e-42a0-87c0-d4f9ab5fb002");
            model.UserName = "UserName";
            var successResult = new Result(ResultType.Success);
            var voteService = new Mock<IVoteService>();
            voteService.Setup(x => x.LikeOrDislikeAMovie(model.UserName, model.MovieId, It.IsAny<bool>())).Returns(successResult);
            var sut = new VoteController(voteService.Object);

            //Act & Assert
            sut
              .WithCallTo(c => c.Like(model))
              .ShouldGiveHttpStatus(HttpStatusCode.OK);
            sut
             .WithCallTo(c => c.Dislike(model))
             .ShouldGiveHttpStatus(HttpStatusCode.OK);
        }

    }
}
