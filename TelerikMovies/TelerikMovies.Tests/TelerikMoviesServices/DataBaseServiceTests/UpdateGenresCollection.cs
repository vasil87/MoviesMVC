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
    public class UpdateGenresCollection
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
        public void ShouldAddNewElemetnsToInitialIfDoesntAlreadyExist()
        {
            //Arange
            ICollection<Genres> initial = new List<Genres>();
            ICollection<Genres> newColl = new List<Genres>();
            Genres genreToTest = new Genres()
            {
                Name = "TestGenre"
            };
            newColl.Add(genreToTest);
            this.genresRepo.Setup(x => x.AllNotDeleted()).Returns(new List<Genres>().AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.UpdateGenresCollection(initial, newColl);

            //Assert
            Assert.IsTrue(initial.Contains(genreToTest));
        }
        [TestMethod]
        public void ShouldAddAlreadyExistElementInsteadOfNew()
        {
            //Arange
            ICollection<Genres> initial = new List<Genres>();
            ICollection<Genres> newColl = new List<Genres>();
            Genres genreToTest = new Genres()
            {
                Name = "TestGenre",
                Id = Guid.NewGuid()
            };
            newColl.Add(genreToTest);

            ICollection<Genres> exist = new List<Genres>();
            Genres existGenre = new Genres()
            {
                Name = "TestGenre",
                Id = Guid.NewGuid()
            };
            exist.Add(existGenre);


            this.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.UpdateGenresCollection(initial, newColl);

            //Assert
            Assert.IsTrue(initial.Select(x=>x.Id).Contains(existGenre.Id));
        }

        [TestMethod]
        public void ShouldRemoveDeletedGenresFromInitial()
        {
            //Arange
            ICollection<Genres> initial = new List<Genres>();
            Genres genreToDel = new Genres()
            {
                Name = "toDelete",
                Id = Guid.NewGuid()
            };
            initial.Add(genreToDel);

            ICollection<Genres> newColl = new List<Genres>();
            Genres genreToTest = new Genres()
            {
                Name = "TestGenre",
                Id = Guid.NewGuid()
            };
            newColl.Add(genreToTest);

            ICollection<Genres> exist = new List<Genres>();
            Genres existGenre = new Genres()
            {
                Name = "TestGenre",
                Id = Guid.NewGuid()
            };
            exist.Add(existGenre);


            this.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.UpdateGenresCollection(initial, newColl);

            //Assert
            Assert.IsFalse(initial.Select(x=>x.Id).Contains(genreToDel.Id));
        }
        [TestMethod]
        public void ShouldNotAddIfElementContainedInInitial()
        {
            //Arange
            ICollection<Genres> initial = new List<Genres>();
            ICollection<Genres> newColl = new List<Genres>();
            ICollection<Genres> exist = new List<Genres>();
            Genres existGenre = new Genres()
            {
                Name = "TestGenre",
                Id = Guid.NewGuid()
            };
            exist.Add(existGenre);
            initial.Add(existGenre);
            newColl.Add(existGenre);

            this.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.UpdateGenresCollection(initial, newColl);

            //Assert
            Assert.IsTrue(initial.Select(x => x.Id).Contains(existGenre.Id));
        }
    }
}
