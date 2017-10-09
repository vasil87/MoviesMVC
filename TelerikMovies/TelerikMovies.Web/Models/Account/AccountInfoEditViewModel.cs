using Common;
using System.ComponentModel.DataAnnotations;
using TelerikMovies.Models;
using TelerikMovies.Web.Areas.Admin.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models
{
    public class AccountInfoEditViewModel: CreateResultModel,IMapFrom<Users>
    {
        public string UserName { get; set; }
        [MaxLength(100)]
        [MinLength(3)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [MinLength(3)]
        public string LastName { get; set; }

        [MaxLength(50)]
        [MinLength(3)]
        public string City { get; set; }

        public bool isMale { get; set; }

        [MaxLength(500)]
        [Url(ErrorMessage = Constants.InvalidUrl)]
        public string ImgUrl { get; set; }
    }
}