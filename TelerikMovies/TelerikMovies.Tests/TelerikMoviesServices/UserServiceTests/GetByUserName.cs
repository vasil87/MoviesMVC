
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

namespace TelerikMovies.Tests.TelerikMoviesServices.UserServiceTests
{
    [TestClass]
    public class GetByUserName
    {
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();

        [TestMethod]
        public void CallsAllReturnsOnlyUsersThatGotSameUserName()
        {
            //Arrange
            List<Users> users = new List<Users>();
            var user1 = new Users
            {
                UserName="name1"
            };
            var user2 = new Users
            {
                UserName = "name2"
            };
            users.Add(user1);
            users.Add(user2);
            userRepo.Setup(x => x.All()).Returns(users.AsQueryable());
            var sut = new UsersService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.GetByUserName(user1.UserName);

            //Assert
            Assert.AreEqual(result.UserName, user1.UserName);
            userRepo.Verify(x => x.All(), Times.Once);
        }
    }
}
