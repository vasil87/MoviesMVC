
using Common.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using TelerikMovies.Web.Areas.Admin.Models.Contracts;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class DataTableViewModel<T>:IDataTableViewModel<T>
    {

        public int draw { get; set; }

        public int recordsFiltered { get; set; }

        public int recordsTotal { get; set; }

        public ICollection<T> data { get; set; }
    }
}
