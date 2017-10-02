using System.Linq;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Web.Models;

namespace TelerikMovies.Web.Controllers
{
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
    }
}