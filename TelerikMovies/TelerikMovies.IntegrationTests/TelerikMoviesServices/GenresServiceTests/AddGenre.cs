﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Data.Repositories;
using TelerikMovies.Data.UoW;
using TelerikMovies.Models;
using TelerikMovies.Services;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web;

namespace TelerikMovies.IntegrationTests.TelerikMoviesServices.UserServiceTests
{
    [TestClass]
    public class AddGenre
    {
        private IMoviesContext context;
        private IEfGenericRepository<Movies> movies;
        private IEfGenericRepository<Genres> genresRepo;
        private IEfGenericRepository<Comments> commentsRepo;
        private IEfGenericRepository<Users>userRepo;
        private IEfGenericRepository<Likes> likesRepo;
        private IEfGenericRepository<Dislikes> dislikesRepo;
        private IUoW saver;

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
            context = kernel.Get<IMoviesContext>();
            movies = new EfGenericRepository<Movies>(context);
            genresRepo = new EfGenericRepository<Genres>(context);
            commentsRepo = new EfGenericRepository<Comments>(context);
            userRepo = new EfGenericRepository<Users>(context);
            likesRepo = new EfGenericRepository<Likes>(context);
            dislikesRepo = new EfGenericRepository<Dislikes>(context);
            saver = new UoW(context);

            context.Genres.Add(genre);
            context.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var whatToDelete=context.Genres.ToList();

            foreach (var genre in whatToDelete)
            {
                context.Genres.Attach(genre);
                context.Genres.Remove(genre);

            }

            context.SaveChanges();
        }

        [TestMethod]
        public void AddGenreToDataBaseIfNoModleWithSameNameExists()
        {
            // Arrange
            IGenreService genreService = new GenreService(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver);

            var genreToAdd = new Genres()
            {
                Name = "genreToAdd",
                IsDeleted = false
            };

            // Act
            genreService.AddGenre(genreToAdd);
            var result = context.Genres.Where(x => x.Name.ToLower() == genreToAdd.Name.ToLower()).FirstOrDefault();
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(genreToAdd.Name, result.Name);
        }

        [TestMethod]
        public void DoNotAddGenreIfItExistsAlready()
        {
            // Arrange
            IGenreService genreService = new GenreService(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver);

            var genreToAdd = new Genres()
            {
                Name = "Action",
                IsDeleted = false
            };

            // Act
            genreService.AddGenre(genreToAdd);
            var result = context.Genres.Where(x => x.Name.ToLower() == genreToAdd.Name.ToLower()).ToList();

            //Assert
            Assert.AreEqual(result.Count,1);
            Assert.AreEqual(genreToAdd.Name, result[0].Name);
        }

    }
}
