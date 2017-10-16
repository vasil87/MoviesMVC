
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
    public class UpdateUser
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
        public void ReturnErrorIfUserDoesntExist()
        {
            List<Users> users = new List<Users>();
            var user1 = new Users
            {
                UserName = "name1"
            };
            var user2 = new Users
            {
                UserName = "name2"
            };
            users.Add(user2);
            userRepo.Setup(x => x.All()).Returns(users.AsQueryable());
            var sut = new UsersService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
           

            //Act
            var result = sut.UpdateUser(user1);

            //Assert

            Assert.AreEqual(result.ErrorMsg, Constants.ErorsDict[ResultType.DoesntExists]);
            Assert.AreEqual(result.ResulType, ResultType.DoesntExists);
        }
        [TestMethod]
        public void ReturnErrorIfUserExistButIsNotChanged()
        {
            List<Users> users = new List<Users>();
            var user1 = new Users
            {  FirstName="test",
                LastName="test2",
                ImgUrl="test3",
                isMale=true,
                City="Bourgas",
                UserName = "name1"
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
            var result = sut.UpdateUser(user1);

            //Assert
            Assert.AreEqual(result.ErrorMsg, Constants.ErorsDict[ResultType.NoChanges]);
            Assert.AreEqual(result.ResulType, ResultType.NoChanges);
        }

        [TestMethod]
        public void IfAllParamsOkCallUsersUpdate()
        {
            //Arrange
            var called = 0;
            List<Users> users = new List<Users>();
            var user1 = new Users
            {
                FirstName = "test",
                LastName = "test2",
                ImgUrl = "test3",
                isMale = true,
                City = "Bourgas",
                UserName = "name1"
            };
            var user2 = new Users
            {
                UserName = "name2"
            };
            var userChanged = new Users
            {
                FirstName = "testChanged",
                LastName = "test2",
                ImgUrl = "test3",
                isMale = true,
                City = "Bourgas",
                UserName = "name1"
            };
            users.Add(user1);
            users.Add(user2);
            userRepo.Setup(x => x.All()).Returns(users.AsQueryable());
            userRepo.Setup(x => x.Update(It.IsAny<Users>())).Callback(() => { called++; });
            var sut = new UsersForTest(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var result = sut.UpdateUser(userChanged);

            //Assert
            Assert.AreEqual(result.ErrorMsg, "Changed");
            Assert.AreEqual(called, 1);
            Assert.AreEqual(sut.SaveChangesCalled, 1);
        }

        private class UsersForTest : UsersService
        {
            public UsersForTest(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
           IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
           IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
            : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
            {
                this.SaveChangesCalled = 0;
            }
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
