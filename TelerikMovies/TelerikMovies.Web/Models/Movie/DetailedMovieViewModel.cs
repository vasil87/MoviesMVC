using System;
using System.Collections.Generic;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Areas.Admin.Models;
using TelerikMovies.Web.Infrastructure;
using TelerikMovies.Web.Models.Comment;

namespace TelerikMovies.Web.Models.Movie
{
    public class DetailedMovieViewModel:IMapFrom<Movies>,IHaveCustomMappings
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public string TrailerUrl { get; set; }

        public DateTime ReleaseDate { get; set; }
   
        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public ICollection<GenresViewModel> Genres { get; set; }

        public ICollection<CommentForMoviesViewModel> Comments { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Movies, DetailedMovieViewModel>()
                .ForMember(detailedModel => detailedModel.Likes, cfg => cfg.MapFrom(movie => movie.Likes.Count))
                .ForMember(detailedModel => detailedModel.Dislikes, cfg => cfg.MapFrom(movie => movie.Dislikes.Count));
        }
    }
}