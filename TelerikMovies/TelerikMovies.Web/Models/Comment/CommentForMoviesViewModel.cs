using System;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models.Comment
{
    public class CommentForMoviesViewModel : IMapFrom<Comments>, IHaveCustomMappings
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public string UserImgUrl { get; set;}

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Comments, CommentForMoviesViewModel>()
               .ForMember(commentModel => commentModel.UserName, cfg => cfg.MapFrom(comment => comment.User.UserName))
               .ForMember(commentModel => commentModel.UserImgUrl, cfg => cfg.MapFrom(comment => comment.User.ImgUrl));
        }
    }
}