using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class GridMovieViewModel : CreateResultModel,IMapFrom<Movies>,IHaveCustomMappings
    {
        public GridMovieViewModel()
        {
            this.Result = null;
        }
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool isDeleted { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
        public Guid Id { get; set; }
        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Movies, GridMovieViewModel>()
                .ForMember(gridModel => gridModel.Likes, cfg => cfg.MapFrom(movie => movie.Likes.Count))
                .ForMember(gridModel => gridModel.Dislikes, cfg => cfg.MapFrom(movie => movie.Dislikes.Count));
        }

    }
}