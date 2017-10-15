﻿
using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services;

namespace TelerikMovies.Tests.TelerikMoviesServices.CommentsServiceTests
{
    [TestClass]
    public class SaveComment
    {
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
 
        [TestMethod]
        public void CallGetUserAndReturnChangedResultIfError()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var commentToReturn = new Comments()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false
            };
            commentsRepo.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(commentToReturn);
            var sut = new CommentsForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = (Users)null;

            //Act
            var result = sut.SaveComment(guid, "test","Some Text");

            //Assert

            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(sut.GetCurrentUserCalled, 1);
        }
        [TestMethod]
        public void CallGetMovieAndReturnChangedResultIfError()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var sut = new CommentsForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = new Users()
            {
                Id = Guid.NewGuid().ToString()
            };
            sut.MovieToReturn = (Movies)null;

            //Act
            var result = sut.SaveComment(guid, "test", "Some Text");

            //Assert

            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void IfAllParamsOkCallCommentsRepoAdd()
        {
            //Arrange
            var called = 0;
            var guid = Guid.NewGuid();
            var sut = new CommentsForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.UserToReturn = new Users()
            {
                Id = Guid.NewGuid().ToString()
            };
            sut.MovieToReturn = new Movies()
            {
                Id = Guid.NewGuid()
            };
                
            commentsRepo.Setup(x => x.Add(It.IsAny<Comments>())).Callback(() => { called++; });
            //Act
            var result = sut.SaveComment(guid, "test", "Some Text");

            //Assert
            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.SaveChangesCalled, 1);
        }

        private class CommentsForTest : CommentsService
        {
            public CommentsForTest(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
           IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
           IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
            : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
            {
                this.GetCurrentUserCalled = 0;
                this.SaveChangesCalled = 0;
                this.GetMovieCount = 0;
            }

            public int GetCurrentUserCalled { get; set; }

            public int GetMovieCount { get; set; }
            public int SaveChangesCalled { get; set; }

            public Action SaveChangesActionCalled { get; set; }

            public Users UserToReturn { get; set; }
            public Movies MovieToReturn { get; set; }
            public override Users GetCurrentUser(string userName, ref IResult result)
            {
                this.GetCurrentUserCalled++;
                result.ErrorMsg = "Changed";
                return UserToReturn;
            }
            public override Movies GetMovie(Guid movieId, ref IResult result)
            {
                this.GetMovieCount++;
                result.ErrorMsg = "Changed";
                return MovieToReturn;
            }
            public override void SaveChange(Action action, ref IResult result)
            {
                action();
                result.ErrorMsg = "Changed";
                this.SaveChangesCalled++;
            }
        }
    }
}
