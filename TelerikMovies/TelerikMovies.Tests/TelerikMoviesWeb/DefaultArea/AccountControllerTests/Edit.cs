using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Services.Contracts.Auth;
using TestStack.FluentMVCTesting;
using TelerikMovies.Web.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using TelerikMovies.Models;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Web;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using System.Reflection;
using AutoMapper;
using Common;
using Common.Contracts;

namespace TelerikMovies.Tests.TelerikMoviesWeb.AccountController
{

    [TestClass]
    public class Edit
    {

        [TestMethod]
        public void ReturnViewWithReturnUrlInViewBag()
        {
            // Arrange
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var fakeUser = new Users();
            userSvMock.Setup(x => x.GetByUserName(It.IsAny<string>())).Returns(fakeUser);
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
           

            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Users, AccountInfoEditViewModel>());

            // Act & Assert
            sut
                .WithCallTo(c => c.Edit())
                .ShouldRenderDefaultView()
                .WithModel<AccountInfoEditViewModel>();
        }

        [TestMethod]
        public void ReturnBackWithErrorOnInvalidModelWrongEmail()
        {
            // Arrange
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var fakeUser = new Users();
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);


            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Users, AccountInfoEditViewModel>());
            Mapper.Initialize(cfg =>
                cfg.CreateMap<AccountInfoEditViewModel, Users>());

            var editModel = new AccountInfoEditViewModel() { FirstName="Vasil",
                                                                 LastName ="Kamburov",
                                                                 City ="Bourgas",
                                                                 ImgUrl ="",
                                                                 isMale =true,
                                                                 UserName ="vasil@abv.bg"};
            sut.ModelState.AddModelError("UserName", "Ivalid User");

            // Act & Assert
            sut
                .WithCallTo(c => c.Edit(editModel))
                .ShouldRenderDefaultView()
                 .WithModel<AccountInfoEditViewModel>()
                 .AndModelErrorFor(m =>m.UserName );
        }

        [TestMethod]
        public void ShouldReturnDefaultViewModelWithNoErrorWhenModelIsValid()
        {
            // Arrange
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var fakeUser = new Users();
            IResult fakeResult = new Result();
            userSvMock.Setup(x => x.UpdateUser(It.IsAny<Users>())).Returns(fakeResult);
            var sut = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);
            fakeHttpContext.Setup(t => t.User).Returns(principal);
            sut.ControllerContext = new ControllerContext
            {
                Controller = sut,
                HttpContext = fakeHttpContext.Object
            };
            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
            Mapper.Initialize(cfg =>
                 cfg.CreateMap<Users, AccountInfoEditViewModel>());
            Mapper.Initialize(cfg =>
                cfg.CreateMap<AccountInfoEditViewModel, Users>());
            var editModel = new AccountInfoEditViewModel()
            {
                FirstName = "Vasil",
                LastName = "Kamburov",
                City = "Bourgas",
                ImgUrl = "",
                isMale = true,
                UserName = "vasil@abv.bg"
            };

            // Act & Assert
            sut
                .WithCallTo(c => c.Edit(editModel))
                .ShouldRenderDefaultView()
                 .WithModel<AccountInfoEditViewModel>();
        }

        private TelerikMovies.Web.Controllers.AccountController setUpControler()
        {
            var userSvMock = new Mock<IUsersService>();
            var signInServiceManagerMock = new Mock<ISignInManagerService>();
            var userServiceManagerMock = new Mock<IUserManagerService>();
            var accountController = new TelerikMovies.Web.Controllers.AccountController(userSvMock.Object, signInServiceManagerMock.Object, userServiceManagerMock.Object);
            return accountController;
        }
    }
}
