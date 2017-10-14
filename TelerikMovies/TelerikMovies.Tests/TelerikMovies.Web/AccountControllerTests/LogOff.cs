using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;

namespace TelerikMovies.Tests.TelerikMovieWeb.AccountControllerTests
{
    [TestClass]
    public class LogOff
    {
        [TestMethod]
        public void ShouldRedirectToIndex()
        {
            //Arange
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();  
            var data = new Dictionary<string, object>()
                  {
                      {"a", "b"} // fake whatever  you need here.
                  };

            fakeHttpContext.Setup(x => x.Items[It.IsAny<string>()]).Returns(data);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };

            //Act and Assert
            sut
                .WithCallTo(c => c.LogOff())
                .ShouldRedirectTo<TelerikMovies.Web.Controllers.HomeController>(c2 => c2.Index());

        }
    }
}
