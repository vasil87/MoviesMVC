using System.Web.Mvc;

namespace TelerikMovies.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }
        public ActionResult Index()
        {
            return this.RedirectToActionPermanent("Index", "Movies",new {Area =""});
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}