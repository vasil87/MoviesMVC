using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class GenreCreateViewModel : CreateResultModel ,IMapFrom<Genres>
    {
        [Required]
        [MaxLength(100)]
        [DisplayName("Genre")]
        public string Name { get; set; }
    }
}
