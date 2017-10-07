
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Models.Contracts;


namespace TelerikMovies.Services
{
    public class DataBaseService
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

        protected void UpdateGenresCollection(ICollection<Genres> initial, ICollection<Genres> newCollection)
        {
            var existingElements = new HashSet<Genres>();

            foreach (var element in newCollection)
            {

                var newElementToAdd = this.GenresRepo.AllNotDeleted().Where(x => x.Name == element.Name).FirstOrDefault();

                if (newElementToAdd != null)
                {
                    existingElements.Add(newElementToAdd);

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
        protected void UpdateCollection <T> (ICollection<T> initial , ICollection<T> newCollection ) where T:class,IDeletable,IAuditable,ITraceable
        {
            var existingElements = new HashSet<T>();

            foreach (var element in newCollection)
            {
                var type = typeof(T);

                var newElementToAdd = this.getRepo<T>().AllNotDeleted().Where(x => x.Id == element.Id).FirstOrDefault();

                if (newElementToAdd != null)
                {
                    existingElements.Add(newElementToAdd);

                }
                else
                {
                    existingElements.Add(element);
                }
            }

            var elementsToRemove = new HashSet<T>();

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

        private IEfGenericRepository<G> getRepo<G>() where G : class, IDeletable, IAuditable,ITraceable
        {
            if (typeof(G) == typeof(Movies))
            {
                return this.MoviesRepo as IEfGenericRepository<G>;
            }
            else if (typeof(G) == typeof(Genres))
            {
                return this.GenresRepo as IEfGenericRepository<G>;
            }
            else if(typeof(G) == typeof(Users))
            {
                return this.UserRepo as IEfGenericRepository<G>;
            }
            else if(typeof(G) == typeof(Likes))
            {
                return this.LikesRepo as IEfGenericRepository<G>;
            }
            else if (typeof(G) == typeof(Dislikes))
            {
                return this.DislikesRepo as IEfGenericRepository<G>;
            }
            else if (typeof(G) == typeof(Comments))
            {
                return this.CommentsRepo as IEfGenericRepository<G>;
            }
            return null;

        }

    }
}
