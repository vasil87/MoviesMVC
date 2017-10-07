using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;
using Common;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class MovieCreateViewModel:CreateResultModel,IMapFrom<Movies>
    {
        public MovieCreateViewModel()
        {
            this.Genres = new List<GenresViewModel>();
            this.Result = null;
        }

        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        [MinLength(20)]
        public string Description { get; set; }

        [Required]
        [MaxLength(500)]
        [Url(ErrorMessage = ErrorMessages.InvalidUrl)]
        public string ImgUrl { get; set; }

        [Required]
        [MaxLength(500)]
        [Url(ErrorMessage = ErrorMessages.InvalidUrl)]
        public string TrailerUrl { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public IList<GenresViewModel> Genres { get; set; }

    }
}