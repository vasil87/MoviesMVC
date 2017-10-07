using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TelerikMovies.Models;
using TelerikMovies.Web.Infrastructure;

namespace TelerikMovies.Web.Models
{
    public class AccountInfoEditViewModel:IMapFrom<Users>
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
    }
}