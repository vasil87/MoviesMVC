using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Web.Controllers;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMoviesWeb.ErrorControllerTests
{

    [TestClass]
    public class NotFound
    {
        [TestMethod]
        public void ShouldRenderDefaultView()
        {
            var sut = new ErrorController();
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeResponse = new Mock<HttpResponseBase>();
            fakeResponse.Setup(x => x.ContentType).Returns("");
            fakeHttpContext.Setup(x => x.Response).Returns(fakeResponse.Object);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };

            sut.WithCallTo(c => c.NotFound()).ShouldRenderDefaultView();
        }
    }
}
