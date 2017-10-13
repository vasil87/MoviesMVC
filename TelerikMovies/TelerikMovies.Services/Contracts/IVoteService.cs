using Common.Contracts;
using System;

namespace TelerikMovies.Services.Contracts
{
    public interface IVoteService
    {
        IResult LikeOrDislikeAMovie(string userName, Guid movieId, bool isLike);
    }
}
