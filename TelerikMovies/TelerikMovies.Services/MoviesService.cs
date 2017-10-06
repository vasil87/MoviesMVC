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
    public class MoviesService : IMoviesService
    {
        private readonly IGenreService genresSv;
        private readonly IEfGenericRepository<Movies> moviesRepo;
        private readonly IUoW saver;

        public MoviesService(IEfGenericRepository<Movies> movies, IUoW saver, IGenreService genres)
        {
            this.genresSv = genres;
            this.moviesRepo = movies;
            this.saver = saver;
        }

        public IResult AddMovie(Movies movie)
        {
            var result = new Result("Success", ResultType.Success);

            var currentMovie = this.moviesRepo.All().Where(x => x.Name.ToLower() == movie.Name.ToLower()).FirstOrDefault();

            var existingGenres = new HashSet<Genres>();

            foreach (var genre in movie.Genres)
            {
                var genreToAdd = this.genresSv.GetGenreByName(genre.Name);
                if (genreToAdd != null)
                {
                    existingGenres.Add(genreToAdd);

                }
                else
                {
                    existingGenres.Add(genre);
                }
            }


            if (currentMovie == null)
            {
                try
                {

                    movie.Genres = existingGenres;
                    this.moviesRepo.Add(movie);
                    this.saver.Save();
                }
                catch (Exception ex)
                {
                    result.ErrorMsg = ex.Message;
                    result.ResulType = ResultType.Error;
                }

            }
            else
            {
                result.ErrorMsg = "Already Exists";
                result.ResulType = ResultType.AlreadyExists;
            }

            return result;
        }

        public ICollection<Movies> GetAllAndDeleted()
        {
            return this.moviesRepo.All().ToList();
        }

        public IResult DeleteByid(Guid id)
        {
            var result = new Result( ResultType.Success);

            var curentMovie =this.moviesRepo.GetById(id);
            var isDeleted = curentMovie.IsDeleted;

            if (isDeleted)
            {
                result = new Result(ResultType.AlreadyDeleted);
                return result;
            }

            if (curentMovie != null )
            {
                
                try
                {
                    this.moviesRepo.Delete(curentMovie);
                    this.saver.Save();
                }
                catch (Exception ex)
                {
                    result.ResulType = ResultType.Error;
                    result.ErrorMsg = ex.Message;
                }
            }
            else
            {
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }

        public IResult UndoDeleteById(Guid id)
        {
            var result = new Result(ResultType.Success);

            var curentMovie = this.moviesRepo.GetById(id);
            var isDeleted = curentMovie.IsDeleted;

            if (!isDeleted)
            {
                result = new Result(ResultType.AlreadyExists);
                return result;
            }

            if (curentMovie != null)
            {

                try
                {
                    curentMovie.IsDeleted = false;
                    this.moviesRepo.Update(curentMovie);
                    this.saver.Save();
                }
                catch (Exception ex)
                {
                    result.ResulType = ResultType.Error;
                    result.ErrorMsg = ex.Message;
                }
            }
            else
            {
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }
    }
}
