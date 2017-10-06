
using Common.Contracts;
using Newtonsoft.Json;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public abstract class CreateResultModel 
    {
        [JsonIgnoreAttribute]
        public IResult Result { get; set; }
    }
}
