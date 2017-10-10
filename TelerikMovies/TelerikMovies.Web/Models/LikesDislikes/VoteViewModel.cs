using System;
using AutoMapper;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models.LikesDislikes
{
    public class VoteViewModel
    {
        public Guid MovieId { get; set; }
        public String UserName { get; set; }

    }
}