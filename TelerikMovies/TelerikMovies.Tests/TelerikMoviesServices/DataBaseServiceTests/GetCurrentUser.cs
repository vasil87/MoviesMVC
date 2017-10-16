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
    public class GetCurrentUser
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
        public void ShouldReturnExistingUserIfThereIsOneWithSameUserNameInBase()
        {
            //Arange
            IResult result = new Result();
            ICollection<Users> existing = new List<Users>();
            Users existingUser = new Users()
            {
                UserName = "TestUser",
                Id = Guid.NewGuid().ToString()
            };
            existing.Add(existingUser);
            this.userRepo.Setup(x => x.All()).Returns(existing.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
           var resultUser = sut.GetCurrentUser("TestUser", ref result);

            //Assert
            Assert.AreEqual(resultUser.Id, existingUser.Id);
            Assert.AreEqual(result.ResulType, ResultType.Success);
        }

        public void ShouldReturnErrorResultandNullForUserIfNoSuchUserExists()
        {
            //Arange
            IResult result = new Result();
            ICollection<Users> existing = new List<Users>();
            Users existingUser = new Users()
            {
                UserName = "TestUser",
                Id = Guid.NewGuid().ToString()
            };
            existing.Add(existingUser);
            this.userRepo.Setup(x => x.All()).Returns(existing.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var resultUser = sut.GetCurrentUser("Gosho", ref result);

            //Assert
            Assert.AreEqual(resultUser, null);
            Assert.AreEqual(result.ResulType, ResultType.DoesntExists);
        }
    }
}
