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
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
    
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
            GetMovie.movies.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(existingMovie);
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
            GetMovie.movies.Setup(x => x.GetById(It.IsAny<Guid>())).Returns((Movies)null);
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var resultMovie = sut.GetMovie(existingMovie.Id, ref result);

            //Assert
            Assert.AreEqual(resultMovie, null);
            Assert.AreEqual(result.ResulType, ResultType.Error);
        }
    }
}
