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
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.CommentsControllerTests
{
    [TestClass]
    public class SaveComment
    {

        [TestMethod]
        public void ShouldReturnBadRequestIfModelisNull()
        {
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            sut
              .WithCallTo(c => c.SaveComment(null))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfInvalidModel()
        {
            CreateCommentViewModel emptyComment = new CreateCommentViewModel();
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            sut
              .WithCallTo(c => c.SaveComment(emptyComment))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnInternalServerErrorIfValidModelButErrorOnSave()
        {
           
            //Arrange
            CreateCommentViewModel mockComment = new CreateCommentViewModel();
            mockComment.Comment = "hello";
            mockComment.UserName = "vasil";
            mockComment.MovieId = new Guid();
            var errorResult = new Result(ResultType.Error);
            var moqUserService = new Mock<ICommentsService>();
            moqUserService.Setup(x => x.SaveComment(mockComment.MovieId, mockComment.UserName, mockComment.Comment)).Returns(errorResult);
            var sut = new CommentsController(moqUserService.Object);
           

            //Act & Assert
            sut
              .WithCallTo(c => c.SaveComment(mockComment))
              .ShouldGiveHttpStatus(HttpStatusCode.InternalServerError);
        }


        [TestMethod]
        public void ShouldReturnOKIfValidModelAndValidSave()
        {
          
            //Arrange
            CreateCommentViewModel mockComment = new CreateCommentViewModel();
            mockComment.Comment = "hello";
            mockComment.UserName = "vasil";
            mockComment.MovieId = new Guid();
            var successResult = new Result(ResultType.Success);
            var moqUserService = new Mock<ICommentsService>();
            moqUserService.Setup(x => x.SaveComment(mockComment.MovieId, mockComment.UserName, mockComment.Comment)).Returns(successResult);
            var sut = new CommentsController(moqUserService.Object);


            //Act & Assert
            sut
              .WithCallTo(c => c.SaveComment(mockComment))
              .ShouldGiveHttpStatus(HttpStatusCode.OK);
        }

    }
}
