using Common.Contracts;
using System.Collections.Generic;
using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface IMoviesService:IService
    {
        IResult AddMovie(Movies movie);
        ICollection<Movies> GetAllAndDeleted();
    }
}
