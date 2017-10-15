using Bytes2you.Validation;
using Common;
using Common.Enums;
using System;
using System.Net;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Infrastructure;
using TelerikMovies.Web.Models.LikesDislikes;

namespace TelerikMovies.Web.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly IVoteService voteSv;

        public VoteController(IVoteService voteSv)
        {
            Guard.WhenArgument(voteSv, ServicesNames.VoteService.ToString()).IsNull().Throw();
            this.voteSv = voteSv;
        }
     
        [HttpPost]
        [AjaxOnlyAttribute]
        public ActionResult Like(VoteViewModel vote)
        {
           return this.LikeOrDIslikeMovie(vote, true);
        }
        [HttpPost]
        [AjaxOnlyAttribute]
        public ActionResult Dislike(VoteViewModel vote)
        {
            return this.LikeOrDIslikeMovie(vote, false);
        }
        
        private ActionResult LikeOrDIslikeMovie(VoteViewModel vote,bool isItLike)
        {
            if (vote == null || vote.MovieId == default(Guid) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Constants.MovieNotExists);
            }

            if (string.IsNullOrWhiteSpace(vote.UserName))
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Constants.UserNotExists);
            }

            var userName = vote.UserName;
            var movieId = vote.MovieId;

            var result = this.voteSv.LikeOrDislikeAMovie(userName,movieId,isItLike);

            HttpStatusCode stCode = HttpStatusCode.OK;

            if (result.ResulType != ResultType.Success)
            {
                stCode = HttpStatusCode.InternalServerError;
            }

            return new HttpStatusCodeResult(stCode, result.ErrorMsg);
           
        }
    }
}
