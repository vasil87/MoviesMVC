using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models.Comment
{
    public class CreateCommentViewModel : IMapFrom<Comments>,IHaveCustomMappings
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
        public Guid MovieId { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Comments, CreateCommentViewModel>()
               .ForMember(commentModel => commentModel.UserName, cfg => cfg.MapFrom(comment => comment.User.UserName))
               .ForMember(commentModel => commentModel.MovieId, cfg => cfg.MapFrom(comment => comment.Movie.Id));
        }
    }
}