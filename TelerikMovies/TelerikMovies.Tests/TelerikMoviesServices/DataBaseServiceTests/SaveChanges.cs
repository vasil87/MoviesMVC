using Common;
using Common.Contracts;
using Common.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services;
using TelerikMovies.Tests.TelerikMoviesServices.DataBaseServiceTests.TestClass;

namespace TelerikMovies.Tests.TelerikMoviesServices.DataBaseServiceTests
{
    [TestClass]
    public class GetMovie
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
        public void ShouldReturnExistingMovieIfThereIsOneWithSameUserNameInBase()
        {
            //Arange
            IResult result = new Result();
            Movies existingMovie = new Movies()
            {
                Name = "TestUser",
                Id = Guid.NewGuid()
            };
            this.movies.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(existingMovie);
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
           var resultMovie = sut.GetMovie(existingMovie.Id, ref result);

            //Assert
            Assert.AreEqual(resultMovie.Id, existingMovie.Id);
            Assert.AreEqual(result.ResulType, ResultType.Success);
        }

        [TestMethod]
        public void ShouldReturnErrorAndNullMovieIfMovieDoesntExist()
        {
            //Arange
            IResult result = new Result();
            Movies existingMovie = new Movies()
            {
                Name = "TestUser",
                Id = Guid.NewGuid()
            };
            this.movies.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((Movies)null);
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var resultMovie = sut.GetMovie(existingMovie.Id, ref result);

            //Assert
            Assert.AreEqual(resultMovie, null);
            Assert.AreEqual(result.ResulType, ResultType.Error);
        }
    }
}
