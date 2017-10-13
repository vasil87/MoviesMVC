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
    public class VoteService : DataBaseService, IVoteService
    {
        public VoteService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
                                  IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
                                  IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
                                  : base(movies, genresRepo, commentsRepo, userRepo, likesRepo, dislikesRepo, saver)
        {

        }

        public IResult LikeOrDislikeAMovie(string userName, Guid movieId, bool isLike)
        {
            var result = new Result();
            if (!isAlreadyLikedOrDislikedAMovie(userName, movieId))
            {

                var currentUser = this.UserRepo.All().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();

                if (currentUser == null)
                {
                    result.ResulType = ResultType.DoesntExists;
                    result.ErrorMsg = Constants.ErorsDict[ResultType.DoesntExists];
                    return result;
                }

                var currentMovie = this.MoviesRepo.GetById(movieId);

                if (currentMovie == null)
                {
                    result.ResulType = ResultType.Error;
                    result.ErrorMsg = Constants.MovieNotExists;

                    return result;
                }

                if (isLike)
                {
                    this.LikesRepo.Add(new Likes { User = currentUser, Movie = currentMovie });
                }
                else
                {
                    this.DislikesRepo.Add(new Dislikes { User = currentUser, Movie = currentMovie });
                }

                try
                {
                    this.Saver.Save();

                }
                catch (Exception ex)
                {
                    result.ResulType = ResultType.Error;
                    result.ErrorMsg = Constants.ErorsDict[ResultType.Error];
                }
            }
            else
            {
                result.ResulType = ResultType.AlreadyExists;
                result.ErrorMsg = Constants.ThisUserAlreadyLikedOrDisliked;
            }

            return result;
        }
        private bool isAlreadyLikedOrDislikedAMovie(string userName, Guid movieId)
        {
            object islikedOrDisliked = this.LikesRepo.All().Where(x => x.Movie.Id == movieId && x.User.UserName.ToLower() == userName.ToLower()).FirstOrDefault();

            if (islikedOrDisliked == null)
            {
                islikedOrDisliked = this.DislikesRepo.All().Where(x => x.Movie.Id == movieId && x.User.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            }

            if (islikedOrDisliked != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

      
    }
}

