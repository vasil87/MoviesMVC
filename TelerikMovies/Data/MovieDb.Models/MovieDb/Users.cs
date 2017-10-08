using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TelerikMovies.Models.Contracts;

namespace TelerikMovies.Models
{
    public class Users: IdentityUser,IDeletable, IAuditable
    {
        private ICollection<Comments> comments;
        private ICollection<Likes> likes;
        private ICollection<Dislikes> dislikes;

        public Users()
        {
            this.comments = new HashSet<Comments>();
            this.dislikes = new HashSet<Dislikes>();
            this.likes = new HashSet<Likes>();
        }

        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        public bool isMale { get; set; }

        public string ImgUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
        [Index]
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Users> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public bool CompareUserWith(Users user)
        {
            if (user == null)
            {
                return false;
            }
            else
            {
                if (user.FirstName != this.FirstName || user.LastName != this.LastName
                    || user.ImgUrl != this.ImgUrl || user.isMale != this.isMale
                    || user.City != this.City)
                    return false;
            }

            return true;
        }
    }
}
