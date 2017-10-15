using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.HomeControllerTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Constructor
    {
        [TestMethod]
        public void ShouldReturnInstanceOfHomeController()
        {
            var sut = new TelerikMovies.Web.Controllers.HomeController();

            Assert.IsInstanceOfType(sut, typeof(TelerikMovies.Web.Controllers.HomeController));
        }
    }
}
