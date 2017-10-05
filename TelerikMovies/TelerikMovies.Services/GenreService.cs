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
    public class GenreService : IGenreService
    {
        private readonly IEfGenericRepository<Genres> genresRepo;
        private readonly IUoW saver;

        public GenreService(IEfGenericRepository<Genres> genres, IUoW saver)
        {
            this.genresRepo = genres;
            this.saver = saver;
        }

        public Genres GetGenreByName(string name)
        {

            var result = this.genresRepo.All().Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();

            return result;
        }

        public ICollection<Genres> GetAllNotExpired()
        {

            var result = this.genresRepo.AllNotDeleted().ToList();

            return result;
        }

        public IResult AddGenre(Genres genre)
        {
            var result = new Result("Success", ResultType.Success);

            var existingGenre = this.genresRepo.All().Where(x => x.Name.ToLower() == genre.Name.ToLower()).FirstOrDefault();

            if (existingGenre == null)
            {
                try
                {
                    this.genresRepo.Add(genre);
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
    }
}

