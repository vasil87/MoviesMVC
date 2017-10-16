using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class SaveComment
    {
        private IMoviesContext context;
        private IEfGenericRepository<Movies> movies;
        private IEfGenericRepository<Genres> genresRepo;
        private IEfGenericRepository<Comments> commentsRepo;
        private IEfGenericRepository<Users>userRepo;
        private IEfGenericRepository<Likes> likesRepo;
        private IEfGenericRepository<Dislikes> dislikesRepo;
        private IUoW saver;

        private static Movies movie = new Movies()
        {
            Id = Guid.NewGuid(),
            Name = "TestMovie",
            ImgUrl="testImgUrl",
            TrailerUrl="TestTrailerUrl",
            Description="TestDescrUrl",
            ReleaseDate=DateTime.Now

        };

        private static Users user = new Users()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "testUser",
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

            userRepo.Add(user);
            context.Movies.Add(movie);
            context.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var whatToDelete=context.Movies.ToList();
            foreach (var genre in whatToDelete)
            {
                context.Movies.Attach(movie);
                context.Movies.Remove(movie);

            };
            context.SaveChanges();
        }

        [TestMethod]
        public void AddGenreToDataBaseIfNoModleWithSameNameExists()
        {
            // Arrange
            ICommentsService commentService = new CommentsService(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver);

            // Act
            commentService.SaveComment(movie.Id,"testUser","test text");
            var result = context.Comments.Where(x => x.Comment== "test text").FirstOrDefault();
            //Assert
            Assert.IsNotNull(result);
        }
    }
}
