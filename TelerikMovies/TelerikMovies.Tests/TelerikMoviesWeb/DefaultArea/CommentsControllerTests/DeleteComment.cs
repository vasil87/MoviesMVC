
using Common;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Net;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.Models.Comment;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.CommentsControllerTests
{
    [TestClass]
    public class DeleteComment
    {

        [TestMethod]
        [DataRow(null,null)]
        [DataRow("123", null)]
        public void ShouldReturnBadRequestIfNullParam(string commentId, string userName)
        {
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            sut
              .WithCallTo(c => c.DeleteComment(commentId,userName))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnBadRequestIfCommentIdNotGuid()
        {
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            sut
              .WithCallTo(c => c.DeleteComment("123", "vasil"))
            .ShouldGiveHttpStatus(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void ShouldReturnInternalServerErrorIfValidModelButErrorOnSave()
        {
           
            //Arrange
            var userName = "vasil";
            var commentId = new Guid();
            var errorResult = new Result(ResultType.Error);
            var moqUserService = new Mock<ICommentsService>();
            moqUserService.Setup(x => x.DeleteComment(commentId,userName)).Returns(errorResult);
            var sut = new CommentsController(moqUserService.Object);
           

            //Act & Assert
            sut
              .WithCallTo(c => c.DeleteComment(commentId.ToString(), userName))
              .ShouldGiveHttpStatus(HttpStatusCode.InternalServerError);
        }


        [TestMethod]
        public void ShouldReturnOKIfValidModelAndValidSave()
        {

            //Arrange
            var userName = "vasil";
            var commentId = new Guid();
            var successResult = new Result(ResultType.Success);
            var moqUserService = new Mock<ICommentsService>();
            moqUserService.Setup(x => x.DeleteComment(commentId, userName)).Returns(successResult);
            var sut = new CommentsController(moqUserService.Object);


            //Act & Assert
            sut
              .WithCallTo(c => c.DeleteComment(commentId.ToString(), userName))
              .ShouldGiveHttpStatus(HttpStatusCode.OK);
        }

    }
}
