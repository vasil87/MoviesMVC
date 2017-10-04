using System.Linq;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;

namespace TelerikMovies.Web.Controllers
{
    public class HomeController : Controller
    {
        private IEfGenericRepository<Movies> movies;
        private IUoW save;
        public HomeController(IEfGenericRepository<Movies> movies,IUoW save)
        {
            this.movies = movies;
            this.save = save;
        }
        public ActionResult Index()
        {
            //var movies=this.movies.All().ToList();
            //return Content(movies.ToString());
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}