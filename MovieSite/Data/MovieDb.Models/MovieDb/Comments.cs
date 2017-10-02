namespace TelerikMovies.Models
{
    using Abstract;
    using Contracts;
    using System.ComponentModel.DataAnnotations;

    public class Comments: BaseInfoModel,IDeletable, IAuditable
    {

        [MaxLength(500)]
        public string Comment { get; set; }
        public virtual Users User { get; set; }
        public virtual Movies Movie { get; set; }

    }
}
