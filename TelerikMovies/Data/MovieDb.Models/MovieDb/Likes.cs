
using TelerikMovies.Models.Abstract;
using TelerikMovies.Models.Contracts;

namespace TelerikMovies.Models
{
    public class Likes : BaseInfoModel, IAuditable, IDeletable
    {
        public virtual Users User { get; set; }
        public virtual Movies Movie { get; set; }
    }
}
