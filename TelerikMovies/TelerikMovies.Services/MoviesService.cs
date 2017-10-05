using Common;
using Common.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;

namespace TelerikMovies.Services
{
    public class MoviesService:IMoviesService
    {
        private readonly IEfGenericRepository<Movies> moviesRepo;
        private readonly IUoW saver;

        public MoviesService(IEfGenericRepository<Movies> movies,IUoW saver)
        {
            this.moviesRepo = movies;
            this.saver = saver;
        }

        public IResult AddMovie(Movies movie)
        {

            var result = new Result();
            //var currentMovie = this.moviesRepo
            //                        .All()
            //                        .Where(x => x.Name.ToLower() == movie.Name.ToLower()).FirstOrDefault();
            //if (currentMovie == null)
            //{
            //    try
            //    {
            //        this.moviesRepo.Add(movie);
            //        this.saver.Save();
            //    }
            //    catch (Exception ex)
            //    {
            //        result.ErrorMsg = ex.Message;
            //        result.ResulType = ResultType.Error;
            //    }
            //}
            //else
            //{
            //    result.ErrorMsg = "Already Exists";
            //    result.ResulType = ResultType.AlreadyExists;
            //}
            result.ErrorMsg = "error";
            result.ResulType = ResultType.Error;
       
            return result;
        }
    }
}
