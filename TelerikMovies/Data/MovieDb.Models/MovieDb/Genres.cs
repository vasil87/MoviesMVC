using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelerikMovies.Models.Abstract;

namespace TelerikMovies.Models
{
    public class Genres:BaseInfoModel
    {
        private ICollection<Movies> movies;
        public Genres()
        {
            this.movies = new HashSet<Movies>();
        }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Movies> Movies
        {
            get { return this.movies; }
            set { this.movies = value; }
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
