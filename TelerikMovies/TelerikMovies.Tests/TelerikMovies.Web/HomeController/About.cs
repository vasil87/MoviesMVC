using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace TelerikMovies.Tests.TelerikMovieWeb.HomeController
{

    [TestClass]
    public class About
    {
        [TestMethod]
        public void ShouldReturnView()
        {
            var sut = new TelerikMovies.Web.Controllers.HomeController();

            var result = sut.About();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ShouldReturnDefaultView()
        {
            var sut = new TelerikMovies.Web.Controllers.HomeController();

            var result = (ViewResult)sut.About();

            Assert.IsNull(result.View);
        }
    }
}
