﻿using Common;
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
    public class SearchForMovies
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
        public void ReturnsAllThatAreNotDeleteAndContainSearchTerm()
        {
            //Arrange
            var likes = new List<Likes>();
            likes.Add(new Likes());
            var guid = Guid.NewGuid();
            var testMovie = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Likes= likes,
                Name="test1"
            };
            var testMovie2 = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Likes= new List<Likes>(),
                Name = "test12"
            };
            var testMovie3 = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = true,
                Likes = new List<Likes>(),
                Name = "test21"
            };
            var testMovie4 = new Movies()
            {
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Likes = new List<Likes>(),
                Name = "test365"
            };
            var moviesToReturn = new List<Movies>();
            moviesToReturn.Add(testMovie2);
            moviesToReturn.Add(testMovie4);
            moviesToReturn.Add(testMovie);    
            moviesToReturn.Add(testMovie3);
            movies.Setup(x => x.AllNotDeleted()).Returns(moviesToReturn.AsQueryable().Where(x => !x.IsDeleted));
            var sut = new MoviesService(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);
          
            //Act
            var result = sut.SearchForMovies("1");

            //Assert
            movies.Verify(x => x.AllNotDeleted(), Times.Once());
            Assert.IsTrue(result.Select(x => x.Id).Contains(testMovie.Id));
            Assert.IsTrue(result.Select(x => x.Id).Contains(testMovie2.Id));
            Assert.IsFalse(result.Select(x => x.Id).Contains(testMovie3.Id));
            Assert.IsFalse(result.Select(x => x.Id).Contains(testMovie4.Id));
            Assert.AreEqual(result.ToList().Count , 2);
        }
    }
}