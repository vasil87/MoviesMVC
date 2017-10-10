using System;

namespace TelerikMovies.Web.Models
{
    public interface IVoteModel
    {
        Guid MovieId { get; set; }
        String UserName { get; set; }

    }
}