
using TelerikMovies.Models.Abstract;
using TelerikMovies.Models.Contracts;

namespace TelerikMovies.Models
{
    public class Likes : BaseInfoModel, IAuditable, IDeletable, ITraceable
    {
        public virtual Users User { get; set; }
        public virtual Movies Movie { get; set; }
    }
}
