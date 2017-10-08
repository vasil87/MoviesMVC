using System;
using System.Collections.Generic;
using TelerikMovies.Web.Areas.Admin.Models;

namespace TelerikMovies.Web.Models.Movie
{
    public class DetailedMovieViewModel
    {
        Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public string TrailerUrl { get; set; }

        public DateTime ReleaseDate { get; set; }

        public IList<GenresViewModel> Genres { get; set; }
    }
}