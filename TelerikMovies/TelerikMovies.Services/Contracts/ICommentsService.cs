using System;
using System.Collections.Generic;
using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface ICommentsService
    {
        ICollection<Comments> GetCommentsForAMovie(Guid id, bool getDeleted);
    }
}