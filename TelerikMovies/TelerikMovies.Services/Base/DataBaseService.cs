
using Common;
using Common.Contracts;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Models.Contracts;


namespace TelerikMovies.Services
{
    public abstract class DataBaseService
    {
        private readonly IEfGenericRepository<Movies> moviesRepo;
        private readonly IEfGenericRepository<Genres> genresRepo;
        private readonly IEfGenericRepository<Comments> commentsRepo;
        private readonly IEfGenericRepository<Users> userRepo;
        private readonly IEfGenericRepository<Likes> likesRepo;
        private readonly IEfGenericRepository<Dislikes> dislikesRepo;
        private readonly IUoW saver;

        public DataBaseService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
            IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
            IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver)
        {

            this.moviesRepo = movies;
            this.genresRepo = genresRepo;
            this.commentsRepo = commentsRepo;
            this.userRepo = userRepo;
            this.likesRepo = likesRepo;
            this.dislikesRepo = dislikesRepo;
            this.saver = saver;
        }

        public IEfGenericRepository<Movies> MoviesRepo
        {
            get
            {
                return moviesRepo;
            }
        }

        public IEfGenericRepository<Genres> GenresRepo
        {
            get
            {
                return genresRepo;
            }
        }

        public IEfGenericRepository<Comments> CommentsRepo
        {
            get
            {
                return commentsRepo;
            }
        }

        public IEfGenericRepository<Users> UserRepo
        {
            get
            {
                return userRepo;
            }
        }

        public IEfGenericRepository<Likes> LikesRepo
        {
            get
            {
                return likesRepo;
            }
        }

        public IEfGenericRepository<Dislikes> DislikesRepo
        {
            get
            {
                return dislikesRepo;
            }
        }

        public IUoW Saver
        {
            get
            {
                return saver;
            }
        }

        public virtual void UpdateGenresCollection(ICollection<Genres> initial, ICollection<Genres> newCollection)
        {
            var existingElements = new HashSet<Genres>();

            foreach (var element in newCollection)
            {

                var existingElement = this.GenresRepo.AllNotDeleted().Where(x => x.Name == element.Name).FirstOrDefault();

                if (existingElement != null)
                {
                    existingElements.Add(existingElement);

                }
                else
                {
                    existingElements.Add(element);
                }
            }

            var elementsToRemove = new HashSet<Genres>();

            foreach (var element in initial)
            {
                if (existingElements.Contains(element))
                {
                    continue;
                }
                else
                {
                    elementsToRemove.Add(element);
                }
            }

            foreach (var element in elementsToRemove)
            {
                initial.Remove(element);
            }


            foreach (var element in existingElements)
            {
                if (initial.Contains(element))
                {
                    continue;
                }
                else
                {
                    initial.Add(element);
                }
            }
        }
     
        public virtual Users GetCurrentUser(string userName,ref IResult result)
        {

            var currentUser = this.UserRepo.All().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();


            if (currentUser == null)
            {
                result.ResulType = ResultType.DoesntExists;
                result.ErrorMsg = Constants.UserNotExists;
            }

            return currentUser;
        }

        public virtual Movies GetMovie(Guid movieId, ref IResult result)
        {

            var currentMovie = this.MoviesRepo.GetById(movieId);

            if (currentMovie == null)
            {
                result.ResulType = ResultType.Error;
                result.ErrorMsg = Constants.MovieNotExists;
            }

            return currentMovie;
        }

        public virtual void SaveChange(Action action,ref IResult result)
        {
            try
            {
                action();
                this.Saver.Save();

            }
            catch (Exception ex)
            {
                result.ResulType = ResultType.Error;
                result.ErrorMsg = Constants.ErorsDict[ResultType.Error];
            }
        }
    }
}
