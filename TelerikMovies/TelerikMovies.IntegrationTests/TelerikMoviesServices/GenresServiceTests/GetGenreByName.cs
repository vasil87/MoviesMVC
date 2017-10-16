using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web;

namespace TelerikMovies.IntegrationTests.TelerikMoviesServices.UserServiceTests
{
    [TestClass]
    public class GetGenreByName
    {
        private static Genres genre = new Genres()
        {
            Id = Guid.NewGuid(),
            Name = "Action"
        };

        private static IKernel kernel;

        [TestInitialize]
        public void TestInit()
        {
            kernel = DependencyInjectionConfig.CreateKernel();
            IMoviesContext dbContext = kernel.Get<IMoviesContext>();

            dbContext.Genres.Add(genre);
            dbContext.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            IMoviesContext dbContext = kernel.Get<IMoviesContext>();

            dbContext.Genres.Attach(genre);
            dbContext.Genres.Remove(genre);

            dbContext.SaveChanges();
        }

        [TestMethod]
        public void ReturnGenreWhenItIsInTheBase()
        {
            // Arrange
            IGenreService genreService = kernel.Get<IGenreService>();

            // Act
            Genres result = genreService.GetGenreByName(genre.Name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(genre.Id, result.Id);
            Assert.AreEqual(genre.Name, result.Name);
        }

        [TestMethod]
        public void ReturnNull_WhenThereIsNoModelWithThePassedName()
        {
            // Arrange
            IGenreService genreService = kernel.Get<IGenreService>();

            // Act
            Genres result = genreService.GetGenreByName("Random Name");

            // Assert
            Assert.IsNull(result);
        }
       
    }
}
