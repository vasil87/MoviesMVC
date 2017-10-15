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
    public class CommentsService : DataBaseService,ICommentsService
    {

        public CommentsService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
            IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
            IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver) 
            :base( movies,genresRepo,commentsRepo,userRepo, likesRepo, dislikesRepo, saver)
        {

        }

        public IResult DeleteComment(Guid commentId, string userName)
        {
            IResult result = new Result();

            var currentComent = this.CommentsRepo.GetById(commentId);

            if (currentComent == null || currentComent.IsDeleted==true)
            {
                result.ResulType = ResultType.DoesntExists;
                result.ErrorMsg = Constants.ErorsDict[ResultType.DoesntExists];

                return result;
            }

            var user = this.GetCurrentUser(userName,ref result);


            if (user == null)
            {        
                return result;
            }

            if (currentComent.User.Id != user.Id)
            {
                result.ResulType = ResultType.Error;
                result.ErrorMsg = Constants.ThisUserNotOwnComment;
                return result;
            }

            this.SaveChange(() => { this.CommentsRepo.Delete(currentComent); }, ref result);

            return result;
        }

        public ICollection<Comments> GetCommentsForAMovie(Guid movieId, bool getDeleted)
        {
            //IQueryable<Comments> querry;
            ICollection<Comments> comments;

            if (getDeleted)
            {
                comments = this.CommentsRepo.All().Where(x => x.Movie.Id == movieId).OrderByDescending(x=>x.CreatedOn).ToList();
            }
            else
            {
                comments = this.CommentsRepo.AllNotDeleted().Where(x => x.Movie.Id == movieId).OrderByDescending(x => x.CreatedOn).ToList();
            }

            return comments;
        }

        public IResult SaveComment(Guid movieId, string userName, string text)
        {
            IResult result = new Result();

            var currentUser = this.GetCurrentUser(userName, ref result);

            if (currentUser == null)
            {
                return result;
            }

            var currentMovie = this.GetMovie(movieId, ref result);

            if (currentMovie == null)
            {
                return result;
            }

            var comment = new Comments
            {
                Comment = text,
                User = currentUser,
                Movie = currentMovie
            };

            this.SaveChange(() => { this.CommentsRepo.Add(comment); }, ref result);

            return result;
        }
    }
}

