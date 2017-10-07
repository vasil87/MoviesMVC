using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TelerikMovies.Web.Areas.Admin.Models
{
    public class MovieEditViewModel: MovieCreateViewModel
    {
      
        public Guid Id { get; set; }

    }
}