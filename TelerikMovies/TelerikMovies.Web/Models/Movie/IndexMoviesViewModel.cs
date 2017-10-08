using System;
using System.Collections.Generic;

namespace TelerikMovies.Web.Models.Movie
{
    public class IndexMoviesViewModel
    {
        public IndexMoviesViewModel()
        {
            this.MoviesForCarousel = new HashSet<SimpleMovieViewModel>();
            this.TopNewMovies = new HashSet<SimpleMovieViewModel>();
        }
        public ICollection<SimpleMovieViewModel> MoviesForCarousel { get; set; }
        public ICollection<SimpleMovieViewModel> TopNewMovies { get; set; }

    }
}