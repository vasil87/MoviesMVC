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
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
    
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
            GetCurrentUser.userRepo.Setup(x => x.All()).Returns(existing.AsQueryable());
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
            GetCurrentUser.userRepo.Setup(x => x.All()).Returns(existing.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            var resultUser = sut.GetCurrentUser("Gosho", ref result);

            //Assert
            Assert.AreEqual(resultUser, null);
            Assert.AreEqual(result.ResulType, ResultType.DoesntExists);
        }
    }
}
