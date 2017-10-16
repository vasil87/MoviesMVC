using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services;

namespace TelerikMovies.Tests.TelerikMoviesServices.MoviesServiceTests
{
    [TestClass]
    public class UndoDeleteById
    {
        private Mock<IEfGenericRepository<Movies>> movies;
        private Mock<IEfGenericRepository<Genres>> genresRepo;
        private Mock<IEfGenericRepository<Comments>> commentsRepo;
        private Mock<IEfGenericRepository<Users>> userRepo;
        private Mock<IEfGenericRepository<Likes>> likesRepo;
        private Mock<IEfGenericRepository<Dislikes>> dislikesRepo;
        private Mock<IUoW> saver;
        [TestInitialize]
        public void Startup()
        {
            movies = new Mock<IEfGenericRepository<Movies>>();
            genresRepo = new Mock<IEfGenericRepository<Genres>>();
            commentsRepo = new Mock<IEfGenericRepository<Comments>>();
            userRepo = new Mock<IEfGenericRepository<Users>>();
            likesRepo = new Mock<IEfGenericRepository<Likes>>();
            dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
            saver = new Mock<IUoW>();
        }
        [TestMethod]
        public void CallGetMovieAndReturnChangedResultIfDoesntExists()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid()
            };
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = (Movies)null;

            //Act
            var result = sut.UndoDeleteById(testMovie.Id);

            //Assert
            Assert.AreEqual(result.ErrorMsg, Constants.MovieNotExists);
            Assert.AreEqual(result.ResulType, ResultType.DoesntExists);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void CallGetMovieAndReturnChangedResultIfExistsButIsNotDeleted()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid()
            };
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = new Movies()
            {
                IsDeleted = false
            };

            //Act
            var result = sut.UndoDeleteById(testMovie.Id);

            //Assert
            Assert.AreEqual(result.ResulType, ResultType.AlreadyExists);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void CallMoviesRepoDeleteIfMovieExistsAndIsDeleted()
        {
            //Arrange
            var called = 0;
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid()
            };
            var movieToAssert = new Movies()
            {
                IsDeleted = true
            };
            movies.Setup(x => x.Update(It.IsAny<Movies>())).Callback(() => { called++; });
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = movieToAssert;

            //Act
            var result = sut.UndoDeleteById(testMovie.Id);

            //Assert
            Assert.AreEqual(movieToAssert.IsDeleted, false);
            Assert.AreEqual(called, 1);
            Assert.AreEqual(result.ErrorMsg, "Saved");
            Assert.AreEqual(result.ResulType, ResultType.Success);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }

        private class MoviesServiceForTest : MoviesService
        {
            public MoviesServiceForTest(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
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
                result.ErrorMsg = "Saved";
                this.SaveChangesCalled++;
            }
        }
    }
}
