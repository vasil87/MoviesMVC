using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Enums;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Areas.Admin.Models;
using TelerikMovies.Web.Areas.Admin.Models.Contracts;

namespace TelerikMovies.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : DataTableController
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

        [HttpGet]
        public ActionResult Edit(String id)
        {
            Guid Id;
            var result=Guid.TryParse(id, out Id);
            if (id == null || Id==null || result==false)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Should Send Id");
            }

            var movie = this.moviesSV.GetMovieById(Id, true);

            MovieEditViewModel model;

            if (movie != null)
            {
                model = Mapper.Map<MovieEditViewModel>(movie);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No such id");
            }
           
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(MovieEditViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.moviesSV.UpdateMovie(Mapper.Map<Movies>(model));
                if (result.ResulType == ResultType.Success)
                {
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

        [HttpPost]
        public ActionResult AllMoviesJson(int draw, int start, int length)
        {
            var allMovies = this.moviesSV.GetAllAndDeleted().Select(x => Mapper.Map<GridMovieViewModel>(x)).ToList();
            var totalMoviesCount = allMovies.Count();

            IDataTableViewModel<GridMovieViewModel> dataTableData = new DataTableViewModel<GridMovieViewModel>();

            this.FillDataTable(dataTableData, allMovies, draw, totalMoviesCount, start, length);

            return Json(dataTableData);
        }

        [HttpPost]
        public ActionResult Delete(Guid[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                   var result= this.moviesSV.DeleteByid(id);
                    if (result.ResulType != ResultType.Success)
                    {
                          return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ErrorMsg);
                    }
                }
            }
            else {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No ids selected");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK,"Successfully removed");
        }

        [HttpPost]
        public ActionResult UndoDelete(Guid[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var result = this.moviesSV.UndoDeleteById(id);
                    if (result.ResulType != ResultType.Success)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ErrorMsg);
                    }
                }
            }
            else {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "No ids selected");
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK, "Successfully removed");
        }



    }
}