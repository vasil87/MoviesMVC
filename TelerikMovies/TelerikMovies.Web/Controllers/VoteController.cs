using Common;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Models;
using TelerikMovies.Web.Models.LikesDislikes;

namespace TelerikMovies.Web.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly ILikeDislikeService likeDislikeSv;

        public VoteController(ILikeDislikeService likeDIslikeSv)
        {
            this.likeDislikeSv = likeDIslikeSv;
        }
     
        [HttpPost]
        public ActionResult Like(VoteViewModel vote)
        {
           return this.LikeOrDIslikeMovie(vote, true);
        }
        [HttpPost]
        public ActionResult Dislike(VoteViewModel vote)
        {
            return this.LikeOrDIslikeMovie(vote, false);
        }

        private ActionResult LikeOrDIslikeMovie(VoteViewModel vote,bool isItLike)
        {
            if (vote.MovieId == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Constants.MovieNotExists);
            }

            if (vote.UserName == null || string.IsNullOrWhiteSpace(vote.UserName))
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Constants.UserNotExists);
            }

            var userName = vote.UserName;
            var movieId = vote.MovieId;

            var result = this.likeDislikeSv.LikeOrDislikeAMovie(userName,movieId,isItLike);

            HttpStatusCode stCode = HttpStatusCode.OK;

            if (result.ResulType != ResultType.Success)
            {
                stCode = HttpStatusCode.InternalServerError;
            }

            return new HttpStatusCodeResult(stCode, result.ErrorMsg);
           
        }
    }
}
