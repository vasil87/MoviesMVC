using System;
namespace TelerikMovies.Web.Models.Movie
{
    public class MovieLikeDislikeModel
    {
        Guid MovieId { get; set; }
        String UserName { get; set; }
    }
}