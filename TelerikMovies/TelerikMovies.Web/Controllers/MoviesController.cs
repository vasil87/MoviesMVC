using AutoMapper;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Models;
using TelerikMovies.Web.Models.Movie;

namespace TelerikMovies.Web.Controllers
{
    public class MoviesController : Controller
    {
        private IMoviesService moviesSv;
        private static readonly int MoviesForCarouselCount;

        static MoviesController()
        {
            MoviesForCarouselCount = int.Parse(ConfigurationManager.AppSettings["MoviesForCarouselCount"]);
        }

        public MoviesController(IMoviesService moviesSv)
        {
            this.moviesSv = moviesSv;
           

        }
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 600)]
        public ActionResult RenderCarousel()
        {
            var RandomMovies = this.moviesSv.GetRandomMovies(MoviesForCarouselCount).Select(x => Mapper.Map<SimpleMovieViewModel>(x)).ToList();
            return PartialView("_Carousel", RandomMovies);
        }
        [ChildActionOnly]
        //[OutputCache(Duration = 300)]
        public ActionResult RenderTopMovies()
        {
            var topMovies = this.moviesSv.GetTopMovies().Select(x => Mapper.Map<SimpleMovieViewModel>(x)).ToList();
            return PartialView("_TopMovies", topMovies);
        }

        public ActionResult Details()
        {
            return null;
        }

        [HttpPost]
        public ActionResult Search(string searchValue)
        {
            var movies = this.moviesSv.SearchForMovies(searchValue).Select(x => Mapper.Map<SimpleMovieViewModel>(x)).ToList();
            return PartialView("_TopMovies",movies);
        }




    }
}