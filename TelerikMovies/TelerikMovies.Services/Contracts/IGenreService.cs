
using Common.Contracts;
using System.Collections.Generic;
using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface IGenreService:IService
    {
        Genres GetGenreByName(string name);
        ICollection<Genres> GetAllNotExpired();
        IResult AddGenre(Genres genre);
    }
}
