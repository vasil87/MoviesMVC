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

        [HttpPost]
        public ActionResult SaveComment(CreateCommentViewModel model)
        {
            var text = model.Comment;
            var userName = model.UserName;
            var imdbId = model.MovieId;

            if (string.IsNullOrEmpty(text))
            {
                return this.BadRequest("Comment can`t be empty");
            }
            var currentUser = this.GetUser(userId);
            if (currentUser == null)
            {
                return this.BadRequest("No such user");
            }
            var currentMovie = this.GetMovie(imdbId);
            if (currentUser == null)
            {
                return this.BadRequest("No such movie");
            }

            var comment = new Comments
            {
                Comment = text,
                UsersId = userId,
                MoviesId = currentMovie.Id
            };
            try
            {
                this.comments.Add(comment);
                this.comments.SaveChanges();
            }
            catch
            {
                return this.BadRequest("Can`t save this comment");
            }

            var lastCommentId = this.comments.All().Where(x => x.UsersId == userId).OrderByDescending(x => x.CreatedOn).FirstOrDefault().Id;

            return this.Ok(lastCommentId);
        }

    }
}