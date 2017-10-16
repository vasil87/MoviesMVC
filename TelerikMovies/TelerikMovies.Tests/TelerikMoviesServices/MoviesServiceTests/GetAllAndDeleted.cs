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
    public class GetTopMovies
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
        public void GetAllMoviesEvenIfThereAreDeleted()
        {
            //Arrange
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = true
            };
            var testMovie2 = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false
            };
            var moviesToReturn = new List<Movies>();
            moviesToReturn.Add(testMovie);
            moviesToReturn.Add(testMovie2);
            movies.Setup(x => x.All()).Returns(moviesToReturn.AsQueryable());
            var sut = new MoviesService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
          
            //Act
            var result = sut.GetAllAndDeleted();

            //Assert
            movies.Verify(x => x.All(), Times.Once());
            Assert.IsTrue(result.Select(x => x.Id).Contains(testMovie.Id));
            Assert.IsTrue(result.Select(x => x.Id).Contains(testMovie2.Id));
        }
    }
}
