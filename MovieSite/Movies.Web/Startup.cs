using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TelerikMovies.Web.Startup))]
namespace TelerikMovies.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
