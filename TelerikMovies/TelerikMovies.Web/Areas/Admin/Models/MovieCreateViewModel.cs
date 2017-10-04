using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class MovieCreateViewModel:IMapFrom<Movies>
    {
        public MovieCreateViewModel()
        {
            this.Genres = new List<GenresViewModel>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImgUrl { get; set; }

        [Required]
        [MaxLength(500)]
        public string TrailerUrl { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public IList<GenresViewModel> Genres { get; set; }

    }
}