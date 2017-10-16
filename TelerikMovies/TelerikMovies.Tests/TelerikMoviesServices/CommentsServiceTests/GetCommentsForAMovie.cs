
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

namespace TelerikMovies.Tests.TelerikMoviesServices.CommentsServiceTests
{
    [TestClass]
    public class GetCommentsForAMovie
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
        public void CallsAllIfNotDeleteIsFalseAndReturnsOnlyMovieThatGotSameId()
        {
            //Arrange
            List<Comments> comments = new List<Comments>();
            var movie1 = new Movies
            {
                Id = Guid.NewGuid()
            };
            var movie2 = new Movies
            {
                Id = Guid.NewGuid()
            };
            comments.Add(new Comments() { Movie= movie1 ,Comment="hey",IsDeleted=false});
            comments.Add(new Comments() { Movie = movie2, Comment = "hey2",IsDeleted=false });
            commentsRepo.Setup(x => x.All()).Returns(comments.AsQueryable());
            var sut = new CommentsService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.GetCommentsForAMovie(movie1.Id, true);

            //Assert
            Assert.IsTrue(result.Select(x => x.Comment).Contains("hey"));
            Assert.IsFalse(result.Select(x => x.Comment).Contains("hey2"));
            commentsRepo.Verify(x => x.All(), Times.Once);
        }
        [TestMethod]
        public void CallsAllNotDeletedIfNotDeleteIsFalseAndReturnsOnlyMovieThatGotSameId()
        {
            //Arrange
            List<Comments> comments = new List<Comments>();
            var movie1 = new Movies
            {
                Id = Guid.NewGuid()
            };
            var movie2 = new Movies
            {
                Id = Guid.NewGuid()
            };
            comments.Add(new Comments() { Movie = movie1, Comment = "hey", IsDeleted = false });
            comments.Add(new Comments() { Movie = movie2, Comment = "hey2", IsDeleted = false });
            commentsRepo.Setup(x => x.AllNotDeleted()).Returns(comments.AsQueryable());
            var sut = new CommentsService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.GetCommentsForAMovie(movie1.Id, false);

            //Assert
            Assert.IsTrue(result.Select(x => x.Comment).Contains("hey"));
            Assert.IsFalse(result.Select(x => x.Comment).Contains("hey2"));
            commentsRepo.Verify(x => x.AllNotDeleted(), Times.Once);
        }
    }
}
