using Common;
using Common.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;

namespace TelerikMovies.Services
{
    public class MoviesService :DataBaseService, IMoviesService
    {
        public MoviesService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
            IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
            IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver) 
            :base( movies,genresRepo,commentsRepo,userRepo, likesRepo, dislikesRepo, saver)
        {

        }

        public IResult AddMovie(Movies movie)
        {
            var result = new Result("Success", ResultType.Success);

            var currentMovie = this.MoviesRepo.All().Where(x => x.Name.ToLower() == movie.Name.ToLower()).FirstOrDefault();

            if (currentMovie == null)
            {
                var existingGenres = new HashSet<Genres>();

                foreach (var genre in movie.Genres)
                {
                    var genreToAdd = this.GenresRepo.All().Where(x => x.Name.ToLower() == genre.Name.ToLower()).FirstOrDefault();
                    if (genreToAdd != null)
                    {
                        existingGenres.Add(genreToAdd);

                    }
                    else
                    {
                        existingGenres.Add(genre);
                    }
                }

                try
                {

                    movie.Genres = existingGenres;
                    this.MoviesRepo.Add(movie);
                    this.Saver.Save();
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
            return this.MoviesRepo.All().ToList();
        }
        public Movies GetMovieById(Guid id, bool getDeleted = false)
        {
            var curentMovie = this.MoviesRepo.GetById(id);

            if (getDeleted)
            {
                return curentMovie;
            }
            else
            {
                if (curentMovie.IsDeleted == true)
                {
                    return null;
                }
                else
                {
                    return curentMovie;
                }
            }
        }

        public IResult DeleteByid(Guid id)
        {
            var result = new Result(ResultType.Success);

            var curentMovie = this.MoviesRepo.GetById(id);
            var isDeleted = curentMovie.IsDeleted;

            if (isDeleted)
            {
                result = new Result(ResultType.AlreadyDeleted);
                return result;
            }

            if (curentMovie != null)
            {

                try
                {
                    this.MoviesRepo.Delete(curentMovie);
                    this.Saver.Save();
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

            var curentMovie = this.MoviesRepo.GetById(id);
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
                    this.MoviesRepo.Update(curentMovie);
                    this.Saver.Save();
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

        public IResult UpdateMovie(Movies movie)
        {
            var result = new Result("Success", ResultType.Success);

            var currentMovie = this.MoviesRepo.GetById(movie.Id);


            if (currentMovie != null)
            {
                var areSame = movie.CompareMoviesWith(currentMovie);
                if (!areSame)
                {
                    UpdateGenresCollection(currentMovie.Genres, movie.Genres);

                    currentMovie.Name = movie.Name;
                    currentMovie.Description = movie.Description;
                    currentMovie.ReleaseDate = movie.ReleaseDate;
                    currentMovie.ImgUrl = movie.ImgUrl;
                    currentMovie.TrailerUrl = movie.TrailerUrl;

                    try
                    {
                        this.MoviesRepo.Update(currentMovie);
                        this.Saver.Save();
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMsg = ex.Message;
                        result.ResulType = ResultType.Error;
                    }
                }
                else
                {
                    result.ErrorMsg = ErrorMessages.ErorsDict[ResultType.NoChanges];
                    result.ResulType = ResultType.NoChanges;
                }
            }
            else
            {
                result.ErrorMsg = ErrorMessages.ErorsDict[ResultType.DoesntExists];
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }

        public ICollection<Movies> GetTopMovies()
        {
            return this.MoviesRepo.AllNotDeleted().OrderBy(x => x.Likes.Count).Take(10).ToList();
        }

        public ICollection<Movies> GetRandomMovies(int moviesForCarouselCount)
        {
            Random rnd = new Random();

            var movies = this.MoviesRepo.AllNotDeleted().ToList();

            var result = new HashSet<Movies>();

            for (int i = 0; i < moviesForCarouselCount; i++)
            {
                int r = rnd.Next(movies.Count);
                result.Add(movies[r]);
            }

            return result;

        }
    }
}
