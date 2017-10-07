using Common.Contracts;
using System;
using System.Collections.Generic;
using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface IMoviesService : IService
    {
        IResult AddMovie(Movies movie);
        ICollection<Movies> GetAllAndDeleted();

        IResult DeleteByid(Guid id);

        IResult UndoDeleteById(Guid id);

        Movies GetMovieById(Guid id, bool getDeleted = false);

        IResult UpdateMovie(Movies movie);
    }
}
