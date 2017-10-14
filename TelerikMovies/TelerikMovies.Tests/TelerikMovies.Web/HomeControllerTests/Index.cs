using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace TelerikMovies.Tests.TelerikMovieWeb.HomeControllerTests
{

    [TestClass]
    public class Index
    {

        [TestMethod]
        public void ShouldRedirectPermanently()
        {
            var sut = new TelerikMovies.Web.Controllers.HomeController();

            var result = (RedirectToRouteResult)sut.Index();

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Movies",result.RouteValues["controller"]);
            Assert.AreEqual(string.Empty, result.RouteValues["Area"]);
        }
    }
}
