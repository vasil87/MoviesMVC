﻿using Common;
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
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
    
        [TestMethod]
        public void ShouldCallSaveSaveIfEverythingOkAndNotChangeResult()
        {
            //Arange
            int called = 0;
            int calledInner = 0;
            IResult result = new Result();
            Action act = () => { calledInner++; };
            SaveChanges.saver.Setup(x => x.Save()).Callback(() => { called++; });
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
            SaveChanges.saver.Setup(x => x.Save()).Callback(() => { called++; });
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
