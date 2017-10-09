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

        public ICollection<Comments> GetCommentsForAMovie(Guid movieId, bool getDeleted)
        {
            IQueryable<Comments> querry;

            if (getDeleted)
            {
                querry = this.CommentsRepo.AllNotDeleted();
            }
            else
            {
                querry = this.CommentsRepo.All();
            }

            ICollection<Comments> comments = querry.Where(x => x.Movie.Id == movieId).ToList();

            return comments;
        }

        public IResult SaveComment(Guid movieId, string userName, string text)
        {
            var result = new Result();

            var currentUser = this.UserRepo.All().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            if (currentUser == null)
            {
                return result.ErrorMss=
            }
            var currentMovie = this.GetMovie(imdbId);
            if (currentUser == null)
            {
                return this.BadRequest("No such movie");
            }

            var comment = new Comments
            {
                Comment = text,
                UsersId = userId,
                MoviesId = currentMovie.Id
            };
            try
            {
                this.comments.Add(comment);
                this.comments.SaveChanges();
            }
            catch
            {
                return this.BadRequest("Can`t save this comment");
            }

            var lastCommentId = this.comments.All().Where(x => x.UsersId == userId).OrderByDescending(x => x.CreatedOn).FirstOrDefault().Id;
        }
    }
}

