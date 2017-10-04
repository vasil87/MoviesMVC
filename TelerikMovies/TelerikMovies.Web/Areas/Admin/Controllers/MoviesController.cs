using System.Linq;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Web.Areas.Admin.Models;

namespace TelerikMovies.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private IEfGenericRepository<Movies> movies;
        private IUoW save;
        public MoviesController(IEfGenericRepository<Movies> movies, IUoW save)
        {
            this.movies = movies;
            this.save = save;
        }
        public ActionResult Index()
        {
            var movies = this.movies.All().ToList();
            return Content(movies.ToString());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Message = "Your application description page.";
            var model = new MovieCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MovieCreateViewModel model)
        {
            return null;
        }
    }
}