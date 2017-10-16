using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Web;

namespace TelerikMovies.IntegrationTests.TelerikMoviesServices.UserServiceTests
{
    [TestClass]
    public class AddGenre
    {
        //private static Genres genre = new Genres()
        //{
        //    Id = Guid.NewGuid(),
        //    Name = "Action"
        //};

        //private static IKernel kernel;

        //[TestInitialize]
        //public void TestInit()
        //{
        //    kernel = DependencyInjectionConfig.CreateKernel();
        //    IMoviesContext dbContext = kernel.Get<IMoviesContext>();

        //    dbContext.g.Add(User);
        //    dbContext.SaveChanges();

        //    var category = dbContext.Categories.Single();
        //    dbBook.CategoryId = category.Id;
        //    dbBook.Category = category;

        //    dbContext.Books.Add(dbBook);
        //    dbContext.SaveChanges();
        //}

        //[TestCleanup]
        //public void TestCleanup()
        //{
        //    LiveDemoEfDbContext dbContext = kernel.Get<LiveDemoEfDbContext>();

        //    dbContext.Categories.Attach(dbCategory);
        //    dbContext.Categories.Remove(dbCategory);

        //    dbContext.SaveChanges();
        //}
    }
}
