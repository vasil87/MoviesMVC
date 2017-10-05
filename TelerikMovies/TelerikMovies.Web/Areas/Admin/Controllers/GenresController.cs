using AutoMapper;
using Common;
using Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class GenresController : Controller
    {
        private IGenreService genresSV;
        private IUoW save;

        public GenresController(IGenreService genres, IUoW save)
        {
            this.genresSV = genres;
            this.save = save;
        }
        public ActionResult Index()
        {
            //var movies = this.movies.All().ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetAllNamesNotExpired()
        {
            var genres = this.genresSV.GetAllNotExpired().Select(x=>x.Name);
            var dict = genres.ToDictionary(x=>x,x=> (string)null);
            return Json(dict, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Create()
        {
            var model = new GenreCreateViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(GenreCreateViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.genresSV.AddGenre(Mapper.Map<Genres>(model));
                if (result.ResulType == ResultType.Success)
                {
                    model = new GenreCreateViewModel();
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

    }
}