
using Common.Contracts;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public abstract class PageableModel
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int elementsNumber { get; set; }
    }
}
