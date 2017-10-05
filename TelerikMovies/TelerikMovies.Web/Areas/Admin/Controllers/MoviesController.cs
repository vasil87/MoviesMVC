using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Models;

namespace TelerikMovies.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private IMoviesService moviesSV;
        private IUoW save;

        public MoviesController(IMoviesService movies, IUoW save)
        {
            this.moviesSV = movies;
            this.save = save;
        }
        public ActionResult Index()
        {
            //var movies = this.movies.All().ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new MovieCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(MovieCreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.moviesSV.AddMovie(Mapper.Map<Movies>(model));
                if (result.ResulType == ResultType.Success)
                {
                    model = new MovieCreateViewModel();
                    this.ModelState.Clear();
                }
                model.Result = result;
            }
            else {
                var allErrorsAsString = this.ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage);
                var errorResult = new Result(string.Join(Environment.NewLine, allErrorsAsString), ResultType.Error);
                model.Result = errorResult;
            }

            return View(model);
        }

        public ActionResult All(int pagesize=20,int pageNumber=1)
        {
            var howManyToSkip = (pageNumber - 1)*pagesize;
            var allMovies = this.moviesSV.GetAllAndDeleted().Select(x => Mapper.Map<GridMovieViewModel>(x));
            var elementsCount = allMovies.Count();
            var moviesToRender= allMovies.Skip(howManyToSkip)
                                          .Take(pagesize)
                                          .ToList();
            var model = new TableMoviesViewModel(moviesToRender);
            model.elementsNumber = elementsCount;

            return View(model);
        }
    }
}