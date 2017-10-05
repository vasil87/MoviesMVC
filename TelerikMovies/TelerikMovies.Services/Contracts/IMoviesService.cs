using Common.Contracts;
using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface IMoviesService:IService
    {
        IResult AddMovie(Movies movie);
    }
}
