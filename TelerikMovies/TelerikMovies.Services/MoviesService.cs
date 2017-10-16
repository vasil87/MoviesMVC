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
    public class MoviesService : DataBaseService, IMoviesService
    {
        public MoviesService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
            IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
            IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
            : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
        {

        }

        public IResult AddMovie(Movies movie)
        {
            IResult result = new Result("Success", ResultType.Success);

            var currentMovie = this.GetMovie(movie.Id, ref result);

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

                this.SaveChange(() =>
                {
                    movie.Genres = existingGenres;
                    this.MoviesRepo.Add(movie);
                }, ref result);

            }
            else
            {
                result.ErrorMsg = "Already Exists";
                result.ResulType = ResultType.AlreadyExists;
            }

            return result;
        }

        public IResult DeleteByid(Guid id)
        {
            IResult result = new Result(ResultType.Success);

            var curentMovie = this.GetMovie(id, ref result);

            if (curentMovie != null)
            {
                var isDeleted = curentMovie.IsDeleted;

                if (isDeleted)
                {
                    result = new Result(ResultType.AlreadyDeleted);
                    return result;
                }

                if (curentMovie != null)
                {

                    this.SaveChange(() =>
                    {
                        this.MoviesRepo.Delete(curentMovie);
                    }, ref result);
                }
                else
                {
                    result.ResulType = ResultType.DoesntExists;
                }
            }
            else
            {
                result.ErrorMsg = Constants.MovieNotExists;
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }

        public IResult UndoDeleteById(Guid id)
        {
            IResult result = new Result(ResultType.Success);

            var curentMovie = this.GetMovie(id, ref result);

            if (curentMovie != null)
            {
                var isDeleted = curentMovie.IsDeleted;

                if (!isDeleted)
                {
                    result = new Result(ResultType.AlreadyExists);
                    return result;
                }

                if (curentMovie != null)
                {

                    this.SaveChange(() =>
                    {
                        curentMovie.IsDeleted = false;
                        this.MoviesRepo.Update(curentMovie);
                    }, ref result);
                }
                else
                {
                    result.ResulType = ResultType.DoesntExists;
                }
            }
            else
            {
                result.ErrorMsg = Constants.MovieNotExists;
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }

        public IResult UpdateMovie(Movies movie)
        {
            IResult result = new Result("Success", ResultType.Success);

            var currentMovie = this.GetMovie(movie.Id, ref result);

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

                    this.SaveChange(() =>
                    {
                        this.MoviesRepo.Update(currentMovie);
                    }, ref result);
                }
                else
                {
                    result.ErrorMsg = Constants.ErorsDict[ResultType.NoChanges];
                    result.ResulType = ResultType.NoChanges;
                }
            }
            else
            {
                result.ErrorMsg = Constants.ErorsDict[ResultType.DoesntExists];
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
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

        public ICollection<Movies> GetAllAndDeleted()
        {
            return this.MoviesRepo.All().ToList();
        }

        public ICollection<Movies> GetTopMovies()
        {
            return this.MoviesRepo.AllNotDeleted().OrderByDescending(x => x.Likes.Count).Take(8).ToList();
        }

        public ICollection<Movies> GetRandomMovies(int moviesForCarouselCount)
        {
            Random rnd = new Random();

            ICollection<Movies> result = new HashSet<Movies>();

            var movies = this.MoviesRepo.AllNotDeleted().ToList();

            if (movies.Count() <= moviesForCarouselCount)
            {
                result = movies;
            }
            else
            {
                while (result.Count < moviesForCarouselCount)
                {
                    int r = rnd.Next(movies.Count);
                    result.Add(movies[r]);
                }
            }

            return result;
        }

        public ICollection<Movies> SearchForMovies(string search)
        {
            return this.MoviesRepo.AllNotDeleted().Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
        }
    }
}
