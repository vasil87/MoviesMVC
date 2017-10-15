
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
    public class GetGenreByName
    {
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();

        [TestMethod]
        public void CallsAllReturnsOnlyGenresThatGotSameName()
        {
            //Arrange
            List<Genres> genres = new List<Genres>();
            var genre = new Genres
            {
                Name="name1"
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
            var result = sut.GetGenreByName(genre.Name);

            //Assert
            Assert.AreEqual(result.Name,genre.Name);
            genresRepo.Verify(x => x.All(), Times.Once);
        }
    }
}
