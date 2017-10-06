using System.Collections.Generic;

namespace TelerikMovies.Web.Areas.Admin.Models.Contracts
{
   public interface IDataTableViewModel<T>
    {
        int draw { get; set; }
        int recordsFiltered { get; set; }
        int recordsTotal { get; set; }
        ICollection<T> data { get; set; }
    }
}
