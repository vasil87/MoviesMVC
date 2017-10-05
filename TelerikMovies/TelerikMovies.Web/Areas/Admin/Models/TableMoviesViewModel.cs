using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class TableMoviesViewModel : PageableModel
    {
        public TableMoviesViewModel()
        {

        }
        public TableMoviesViewModel(ICollection<GridMovieViewModel> movies)
        {
            this.Movies = movies;
        }
        ICollection<GridMovieViewModel> Movies { get; set; }
    }
}