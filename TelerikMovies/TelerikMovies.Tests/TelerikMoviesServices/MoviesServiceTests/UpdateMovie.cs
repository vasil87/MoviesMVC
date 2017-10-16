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
    public class UpdateMovie
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
            var result = sut.UpdateMovie(testMovie);

            //Assert
            Assert.AreEqual(result.ErrorMsg, Constants.ErorsDict[ResultType.DoesntExists]);
            Assert.AreEqual(result.ResulType, ResultType.DoesntExists);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void IfMovieIsNotChangedReturnError()
        {
            var date = DateTime.Now;
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Name ="test1",
                ReleaseDate= date,
                TrailerUrl="test2",
                ImgUrl="test3",
                Description="description",
                Id = Guid.NewGuid()
            };
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = testMovie;

            //Act
            var result = sut.UpdateMovie(testMovie);
        
            //Assert
            Assert.AreEqual(result.ErrorMsg, Constants.ErorsDict[ResultType.NoChanges]);
            Assert.AreEqual(result.ResulType, ResultType.NoChanges);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void CallMoviesRepoDeleteIfMovieExistsAndIsNotDeleted()
        {
            //Arrange
            var called = 0;
            var date = DateTime.Now;
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Name = "test1",
                ReleaseDate = date,
                TrailerUrl = "test2",
                ImgUrl = "test3",
                Description = "description",
                Id = guid,
                Genres = new List<Genres>()
            };
            var testMovie2 = new Movies()
            {
                Name = "test12",
                ReleaseDate = date,
                TrailerUrl = "test2",
                ImgUrl = "test3",
                Description = "description",
                Id = guid,
                Genres =new List<Genres>()
            };
            movies.Setup(x => x.Update(It.IsAny<Movies>())).Callback(() => { called++; });
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = testMovie;

            //Act
            var result = sut.UpdateMovie(testMovie2);

            //Assert
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.GetMovieCount, 1);
            Assert.AreEqual(sut.UpdateGenresCalled, 1);
            Assert.AreEqual(result.ErrorMsg, "Saved");
            Assert.AreEqual(result.ResulType, ResultType.Success);
           
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
                this.UpdateGenresCalled = 0;
            }

            public int GetCurrentUserCalled { get; set; }

            public int GetMovieCount { get; set; }
            public int SaveChangesCalled { get; set; }
            public int UpdateGenresCalled { get; set; }

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

            public override void UpdateGenresCollection(ICollection<Genres> initial, ICollection<Genres> newCollection)
            {
                this.UpdateGenresCalled++;
            }
        }
    }
}
