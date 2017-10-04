using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TelerikMovies.Models.Abstract;
using TelerikMovies.Models.Contracts;
namespace TelerikMovies.Models
{
    public class Movies:BaseInfoModel, IAuditable, IDeletable
    {
        private ICollection<Comments> comments;
        private ICollection<Likes> likes;
        private ICollection<Dislikes> dislikes;
        private ICollection<Genres> genres;

        public Movies()
        {
            this.LikesNumber = 0;
            this.DislikesNumber = 0;
            this.comments = new HashSet<Comments>();
            this.dislikes = new HashSet<Dislikes>();
            this.likes = new HashSet<Likes>();
            this.genres = new HashSet<Genres>();

        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImgUrl { get; set; }

        [Required]
        [MaxLength(500)]
        public string TrailerUrl { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int LikesNumber { get; set; }

        public int DislikesNumber { get; set; }
        public virtual ICollection<Comments> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Likes> Likes
        {
            get { return this.likes; }
            set { this.likes = value; }
        }

        public virtual ICollection<Dislikes> Dislikes
        {
            get { return this.dislikes; }
            set { this.dislikes = value; }
        }

        public virtual ICollection<Genres> Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }
    }
}
