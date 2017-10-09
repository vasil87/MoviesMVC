using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TelerikMovies.Data.Migrations;
using TelerikMovies.Data;
using TelerikMovies.Web.ForumSystem.Web.App_Start;
using System;
using System.Web;
using TelerikMovies.Services.Contracts;
using Common;

namespace TelerikMovies.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MoviesContext, Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var mapper = new AutoMapperConfig();
            mapper.Execute(Assembly.GetExecutingAssembly());
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            var userSv= (IUsersService)System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUsersService));
            var userName = this.User.Identity.Name;
            var dbUser = userSv.GetByUserName(userName);

            this.Session[Constants.UserImgUrl] = dbUser.ImgUrl;
            this.Session[Constants.UserId] = dbUser.Id;
        }
        protected void Application_Error()
        {
            if (Context.IsCustomErrorEnabled)
            {
                ShowCustomErrorPage(Server.GetLastError());
            }
        }

        private void ShowCustomErrorPage(Exception exception)
        {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);

            Response.Clear();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 404:
                    routeData.Values.Add("action", "NotFound");
                    break;
                case 500:
                default:
                    routeData.Values.Add("action", "Index");
                    break;
            }

            Server.ClearError();

            IController controller = ControllerBuilder.Current.GetControllerFactory().CreateController(new RequestContext() { RouteData = routeData }, "Error");
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }

        
    }
}
