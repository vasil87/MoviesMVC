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

    }
}

