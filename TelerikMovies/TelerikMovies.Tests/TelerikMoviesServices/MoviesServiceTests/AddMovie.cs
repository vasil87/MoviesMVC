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
    public class AddMovie
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
        public void CallGetMovieAndReturnChangedResultIfAlreadyExists()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid()
            };
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = new Movies();

            //Act
            var result = sut.AddMovie(testMovie);

            //Assert
            Assert.AreEqual(result.ErrorMsg, "Already Exists");
            Assert.AreEqual(result.ResulType, ResultType.AlreadyExists);
            Assert.AreEqual(sut.GetMovieCount, 1);
        }
        [TestMethod]
        public void MovieDoesntExistCallGenresRespoAllAndSaveChange()
        {
            //Arrange
            var called = 0;
            var guid = Guid.NewGuid();
            var genre = new Genres()
            {
                Name = "Test"
            };
            var genre2 = new Genres()
            {
                Name = "Test2"
            };
            var genres = new List<Genres>();
            genres.Add(genre);
            genres.Add(genre2);
            var testMovie = new Movies()
            {
                Genres = genres
            };
            var genresInDataBase = new List<Genres>();
            genresInDataBase.Add(genre);
            genresRepo.Setup(x => x.All()).Returns(genresInDataBase.AsQueryable());
            movies.Setup(x => x.Add(It.IsAny<Movies>())).Callback(() => { called++; });
            var sut = new MoviesServiceForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
            sut.MovieToReturn = (Movies)null;

            //Act
            var result = sut.AddMovie(testMovie);

            //Assert
            Assert.AreEqual(called, 1);
            genresRepo.Verify(x => x.All(), Times.Exactly(2));
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
