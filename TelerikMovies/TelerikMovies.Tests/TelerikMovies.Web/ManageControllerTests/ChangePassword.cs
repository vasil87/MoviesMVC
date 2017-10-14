using Common;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TelerikMovies.Web.Controllers;
using TelerikMovies.Web.Models;
using TestStack.FluentMVCTesting;

namespace Movies.Tests.TelerikMovies.Web.ManageControllerTests
{

    [TestClass]
    public class ChangePassword
    {
        [TestMethod]
        public void ShouldRetrunView()
        {
            // Arrange
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var sut = new ManageController(userServiceManagerMock.Object, signInServiceManagerMock.Object);

            // Act & Assert
            sut
                .WithCallTo(c => c.ChangePassword())
                .ShouldRenderDefaultView();
        }

        [TestMethod]
        public void ReturnViewWithRightModelAndErrorsIfInvalidModelState()
        {
            // Arrange
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var sut = new ManageController(userServiceManagerMock.Object, signInServiceManagerMock.Object);
            sut.ModelState.AddModelError("NewPassword", "Wrong password");
            var passwordModel = new ChangePasswordViewModel();
            passwordModel.OldPassword = "123456";
            passwordModel.NewPassword = "1234567";
            passwordModel.ConfirmPassword = "1234567";

            // Act & Assert
            sut
                .WithCallTo(c => c.ChangePassword(passwordModel))
                .ShouldRenderDefaultView()
                .WithModel<ChangePasswordViewModel>()
                .AndModelErrorFor(x => x.NewPassword);
        }

        [TestMethod]
        public void ReturnToDefaultViewIfFailToChangePassword()
        {
            // Arrange
            var error = "Error";
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            userServiceManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(error));

            var sut = new ManageController(userServiceManagerMock.Object, signInServiceManagerMock.Object);
            var passwordModel = new ChangePasswordViewModel();
            passwordModel.OldPassword = "123456";
            passwordModel.NewPassword = "1234567";
            passwordModel.ConfirmPassword = "1234567";     
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var data = new Dictionary<string, object>()
                  {
                      {"a", "b"}
                  };
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            fakeHttpContext.Setup(x => x.Items[It.IsAny<string>()]).Returns(data);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };


            // Act & Assert
            sut
                .WithCallTo(c => c.ChangePassword(passwordModel))
                .ShouldRenderDefaultView()
                .WithModel<ChangePasswordViewModel>()
                .AndModelError(string.Empty);
        }
        [TestMethod]
        public void ShouldReturnDefaultViewIfSucceededButCantFindUser()
        {
            // Arrange
            Users fakeUser = null;
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            userServiceManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userServiceManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(fakeUser);


            var sut = new ManageController(userServiceManagerMock.Object, signInServiceManagerMock.Object);
            var passwordModel = new ChangePasswordViewModel();
            passwordModel.OldPassword = "123456";
            passwordModel.NewPassword = "1234567";
            passwordModel.ConfirmPassword = "1234567";
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var data = new Dictionary<string, object>()
                  {
                      {"a", "b"}
                  };
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            fakeHttpContext.Setup(x => x.Items[It.IsAny<string>()]).Returns(data);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };


            // Act & Assert
            sut
                .WithCallTo(c => c.ChangePassword(passwordModel))
                .ShouldRedirectToRoute("");
        }

        [TestMethod]
        public void ShouldReturnIndex()
        {
            // Arrange
            Users mockUser = new Users();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            signInServiceManagerMock.Setup(x => x.SignInAsync(It.IsAny<Users>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(1));
            var userServiceManagerMock = new Mock<IUserManagerService>();
            userServiceManagerMock.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            userServiceManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            var sut = new ManageController(userServiceManagerMock.Object, signInServiceManagerMock.Object);
            var passwordModel = new ChangePasswordViewModel();
            passwordModel.OldPassword = "123456";
            passwordModel.NewPassword = "1234567";
            passwordModel.ConfirmPassword = "1234567";
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            var data = new Dictionary<string, object>()
                  {
                      {"a", "b"}
                  };
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            fakeHttpContext.Setup(x => x.Items[It.IsAny<string>()]).Returns(data);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };


            // Act & Assert
            sut
              .WithCallTo(c => c.ChangePassword(passwordModel))
              .ShouldRedirectToRoute("");
        }
    }
}
