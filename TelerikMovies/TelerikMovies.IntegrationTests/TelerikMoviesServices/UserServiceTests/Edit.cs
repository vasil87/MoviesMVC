using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Models;

namespace TelerikMovies.IntegrationTests.TelerikMoviesServices.UserServiceTests
{
    [TestClass]
    public class Edit
    {
        private static Users User = new Users()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "category"
        };

        private static IKernel kernel;

        [TestInitialize]
        public void TestInit()
        {
            kernel = NinjectWebCommon.CreateKernel();
            LiveDemoEfDbContext dbContext = kernel.Get<LiveDemoEfDbContext>();

            dbContext.Categories.Add(dbCategory);
            dbContext.SaveChanges();

            var category = dbContext.Categories.Single();
            dbBook.CategoryId = category.Id;
            dbBook.Category = category;

            dbContext.Books.Add(dbBook);
            dbContext.SaveChanges();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LiveDemoEfDbContext dbContext = kernel.Get<LiveDemoEfDbContext>();

            dbContext.Categories.Attach(dbCategory);
            dbContext.Categories.Remove(dbCategory);

            dbContext.SaveChanges();
        }
    }
}
