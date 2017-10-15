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
    public class GenreService :DataBaseService, IGenreService
    {
        public GenreService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
            IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
            IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver) 
            :base( movies,genresRepo,commentsRepo,userRepo, likesRepo, dislikesRepo, saver)
        {

        }
        public Genres GetGenreByName(string name)
        {

            var result = this.GenresRepo.All().Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

            return result;
        }

        public ICollection<Genres> GetAllNotExpired()
        {

            var result = this.GenresRepo.AllNotDeleted().ToList();

            return result;
        }

        public IResult AddGenre(Genres genre)
        {
            IResult result = new Result("Success", ResultType.Success);

            var existingGenre = this.GenresRepo.All().Where(x => x.Name.ToLower() == genre.Name.ToLower()).FirstOrDefault();

            if (existingGenre == null)
            {
                this.SaveChange(() => { this.GenresRepo.Add(genre); }, ref result);
            }
            else
            {
                result.ErrorMsg = "Already Exists";
                result.ResulType = ResultType.AlreadyExists;
            }

            return result;
        }
    }
}

