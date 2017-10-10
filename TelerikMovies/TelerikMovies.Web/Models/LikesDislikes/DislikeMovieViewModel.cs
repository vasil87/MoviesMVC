using System;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models.LikesDislikes
{
    public class DislikeMovieViewModel : IMapFrom<Dislikes>, IHaveCustomMappings,IVoteModel
    {
        public Guid MovieId { get; set; }
        public String UserName { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Likes, DislikeMovieViewModel>()
               .ForMember(likeModel => likeModel.UserName, cfg => cfg.MapFrom(like => like.User.UserName))
               .ForMember(likeModel => likeModel.MovieId, cfg => cfg.MapFrom(like => like.Movie.Id));
        }
    }
}