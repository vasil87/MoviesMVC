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
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.CommentsControllerTests
{
    [TestClass]
    public class GetAllNotDeletedCommentsForAMovie
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("asdf")]
        public void ShouldReturn404ViewIfInvalidId(string id)
        {
            var moqUserService = new Mock<ICommentsService>();
            var sut = new CommentsController(moqUserService.Object);

            sut
              .WithCallTo(c => c.GetAllNotDeletedCommentsForAMovie(id))
              .ShouldRenderView("404");
        }
        [TestMethod]
        public void ShouldReturnPartialViewWithRightModel()
        {
            //Arrange
            var id = "b1090055-59de-402d-a067-678277864d56";
            var moqUserService = new Mock<ICommentsService>();
            moqUserService.Setup(x => x.GetCommentsForAMovie(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<Comments>());
            var sut = new CommentsController(moqUserService.Object);
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Comments, CommentForMoviesViewModel>());

            //Act & Assert
            sut
              .WithCallTo(c => c.GetAllNotDeletedCommentsForAMovie(id))
              .ShouldRenderDefaultPartialView()
              .WithModel<List<CommentForMoviesViewModel>>();
        }

    }
}
