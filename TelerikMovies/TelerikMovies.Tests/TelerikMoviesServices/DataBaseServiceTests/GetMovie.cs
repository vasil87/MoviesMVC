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
    public class SaveChanges
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
        public void ShouldCallSaveSaveIfEverythingOkAndNotChangeResult()
        {
            //Arange
            int called = 0;
            int calledInner = 0;
            IResult result = new Result();
            Action act = () => { calledInner++; };
            this.saver.Setup(x => x.Save()).Callback(() => { called++; });
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.SaveChange(act,ref result);

            //Assert
            Assert.AreEqual(called, 1);
            Assert.AreEqual(calledInner, 1);
            Assert.AreEqual(result.ResulType, ResultType.Success);
        }
        [TestMethod]
        public void ShouldSetResultToErrorWithErrorMessage()
        {
            //Arange
            int called = 0;
            IResult result = new Result();
            Action act = () => { throw new Exception(); };
            this.saver.Setup(x => x.Save()).Callback(() => { called++; });
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.SaveChange(act, ref result);

            //Assert
            Assert.AreEqual(called, 0);
            Assert.AreEqual(result.ResulType, ResultType.Error);
            Assert.AreEqual(result.ErrorMsg, Constants.ErorsDict[ResultType.Error]);
        }
    }
}
