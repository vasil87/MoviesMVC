using System;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models.Movie
{
    public class SimpleMovieViewModel: IMapFrom<Movies>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ImgUrl { get; set; }

    }
}