using AutoMapper;
using System;
using System.Linq;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Models.Comment;

namespace TelerikMovies.Web.Controllers
{
    public class CommentsController : Controller
    {
        private ICommentsService commentsSv;

        public CommentsController(ICommentsService commentsSv)
        {
            this.commentsSv = commentsSv;
        }

        [HttpGet]
        public ActionResult GetAllNotDeletedCommentsForAMovie(string id)
        {
            Guid Id;
            var result = Guid.TryParse(id, out Id);
            if (id == null || Id == null || result == false)
            {
                return View("404");
            }

            var comments = this.commentsSv.GetCommentsForAMovie(Id, false).Select(x => Mapper.Map<CommentForMoviesViewModel>(x)).ToList();

            return PartialView(comments);
        }

    }
}