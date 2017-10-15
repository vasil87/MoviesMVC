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
        static Mock<IEfGenericRepository<Movies>> movies = new Mock<IEfGenericRepository<Movies>>();
        static Mock<IEfGenericRepository<Genres>> genresRepo = new Mock<IEfGenericRepository<Genres>>();
        static Mock<IEfGenericRepository<Comments>> commentsRepo = new Mock<IEfGenericRepository<Comments>>();
        static Mock<IEfGenericRepository<Users>> userRepo = new Mock<IEfGenericRepository<Users>>();
        static Mock<IEfGenericRepository<Likes>> likesRepo = new Mock<IEfGenericRepository<Likes>>();
        static Mock<IEfGenericRepository<Dislikes>> dislikesRepo = new Mock<IEfGenericRepository<Dislikes>>();
        static Mock<IUoW> saver = new Mock<IUoW>();
    
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
            UpdateGenresCollection.genresRepo.Setup(x => x.AllNotDeleted()).Returns(new List<Genres>().AsQueryable());
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


            UpdateGenresCollection.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
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


            UpdateGenresCollection.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
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

            UpdateGenresCollection.genresRepo.Setup(x => x.AllNotDeleted()).Returns(exist.AsQueryable());
            StubClass sut = new StubClass(movies.Object, genresRepo.Object, commentsRepo.Object, userRepo.Object, likesRepo.Object, dislikesRepo.Object, saver.Object);

            //Act
            sut.UpdateGenresCollection(initial, newColl);

            //Assert
            Assert.IsTrue(initial.Select(x => x.Id).Contains(existGenre.Id));
        }
    }
}
