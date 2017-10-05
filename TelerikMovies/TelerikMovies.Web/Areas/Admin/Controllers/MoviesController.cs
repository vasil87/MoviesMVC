﻿using AutoMapper;
using Common.Enums;
using System;
using System.Linq;
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
            this.ModelState.AddModelError("hello", "EXCEPTION");
            this.ModelState.AddModelError("hello", "EXCEPTION2");
            this.ModelState.AddModelError("hello", "EXCEPTION3");
            if (this.ModelState.IsValid)
            {

                var result = this.moviesSV.AddMovie(Mapper.Map<Movies>(model));

                if (result.ResulType != ResultType.Success)
                {
                    return PartialView("_Errors", result.ErrorMsg);
                }
                else
                {
                    var newModel = new MovieCreateViewModel();
                    return PartialView("_Errors", result.ResulType.ToString());
                }
            }
            else
            {
                var errors = this.ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage);
                return PartialView("_Errors", string.Join("\r\n", errors));
            }

        }
    }
}