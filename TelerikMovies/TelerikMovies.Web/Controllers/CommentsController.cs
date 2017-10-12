﻿using AutoMapper;
using Common;
using Common.Contracts;
using Common.Enums;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TelerikMovies.Services.Contracts;
using TelerikMovies.Web.Models.Comment;

namespace TelerikMovies.Web.Controllers
{
    [Authorize]
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
            if (model.Comment == null || string.IsNullOrWhiteSpace(model.Comment))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, Constants.EmptyRequest);
            }

            var text = model.Comment.Trim();
            var userName = model.UserName;
            var movieId = model.MovieId;

          
            var result = this.commentsSv.SaveComment(movieId, userName, text);

            HttpStatusCode stCode=HttpStatusCode.OK;

            if (result.ResulType != ResultType.Success)
            {
                stCode = HttpStatusCode.InternalServerError;
            }

            return new HttpStatusCodeResult(stCode, result.ErrorMsg);
        }

        [HttpPost]
        public ActionResult DeleteComment(string commentId, string userName)
        {
            HttpStatusCode stCode = HttpStatusCode.OK;
            IResult result = new Result();

            if (commentId == null || string.IsNullOrWhiteSpace(commentId) || userName == null || string.IsNullOrWhiteSpace(userName))
            {
                    stCode = HttpStatusCode.BadRequest;
                    result.ErrorMsg = Constants.UserNotExists + " or " + Constants.MovieNotExists;
            }
            else
            {
                Guid commentIdGuid;

                if (Guid.TryParse(commentId, out commentIdGuid))
                { 
                
                    result = this.commentsSv.DeleteComment(commentIdGuid, userName);

                    if (result.ResulType != ResultType.Success)
                    {
                        stCode = HttpStatusCode.InternalServerError;
                    }
                }
                else
                {
                    stCode = HttpStatusCode.BadRequest;
                    result.ErrorMsg = Constants.InvalidCommentId;
                }
            }

            return new HttpStatusCodeResult(stCode, result.ErrorMsg);
        }

    }
}