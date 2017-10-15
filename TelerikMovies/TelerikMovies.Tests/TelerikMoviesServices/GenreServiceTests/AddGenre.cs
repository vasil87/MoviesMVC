
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

namespace TelerikMovies.Tests.TelerikMoviesServices.GenreServiceTests
{
    [TestClass]
    public class AddGenre
    {
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
 

        [TestMethod]
        public void CallGetAllAndReturnExistResultIfAlreadyExists()
        {
            //Arrange
            List<Genres> genres = new List<Genres>();
            var genre = new Genres
            {
                Name = "name1"
            };
            var genre2 = new Genres
            {
                Name = "name2"
            };
            genres.Add(genre);
            genres.Add(genre2);
            genresRepo.Setup(x => x.All()).Returns(genres.AsQueryable());
            var sut = new GenreService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
           

            //Act
            var result = sut.AddGenre(genre);

            //Assert

            Assert.AreEqual(result.ErrorMsg, "Already Exists");
            Assert.AreEqual(result.ResulType, ResultType.AlreadyExists);
        }
        [TestMethod]
        public void IfAllParamsOkCallGenresRepoAdd()
        {
            //Arrange
            int called = 0;
            List<Genres> genres = new List<Genres>();
            var genre = new Genres
            {
                Name = "name1"
            };
            var genre2 = new Genres
            {
                Name = "name2"
            };
            var genre3 = new Genres
            {
                Name = "name3"
            };
            genres.Add(genre);
            genres.Add(genre2);
            genresRepo.Setup(x => x.All()).Returns(genres.AsQueryable());
            genresRepo.Setup(x => x.Add(It.IsAny<Genres>())).Callback(() => { called++; });
            var sut = new GenresForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
         
            //Act
            var result = sut.AddGenre(genre3);

            //Assert
            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.SaveChangesCalled, 1);
        }

        private class GenresForTest : GenreService
        {
            public GenresForTest(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
           IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
           IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
            : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
            {
                this.SaveChangesCalled = 0;
            }


            public int GetMovieCount { get; set; }
            public int SaveChangesCalled { get; set; }

            public Action SaveChangesActionCalled { get; set; }
            public override void SaveChange(Action action, ref IResult result)
            {
                action();
                result.ErrorMsg = "Changed";
                this.SaveChangesCalled++;
            }
        }
    }
}
