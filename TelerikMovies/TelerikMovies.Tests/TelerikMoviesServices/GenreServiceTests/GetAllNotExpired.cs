
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
    public class GetAllNotExpired
    {
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();

        [TestMethod]
        public void CallsAllNotDeletedReturnsNotDeletedOnly()
        {
            //Arrange
            List<Genres> genres = new List<Genres>();
            var genre = new Genres
            {
                Name="name1",
                IsDeleted=true
            };
            var genre2 = new Genres
            {
                Name = "name2",
                IsDeleted = false
            };
            genres.Add(genre);
            genres.Add(genre2);
            genresRepo.Setup(x => x.AllNotDeleted()).Returns(genres.AsQueryable());
            var sut = new GenreService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.GetAllNotExpired();

            //Assert
            Assert.IsTrue(result.Select(x => x.Name).Contains("name2"));
            Assert.IsFalse(result.Select(x => x.Name).Contains("hey1"));
            genresRepo.Verify(x => x.AllNotDeleted(), Times.Once);
        }
    }
}
