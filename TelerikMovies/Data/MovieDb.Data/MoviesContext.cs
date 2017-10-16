namespace TelerikMovies.Data
{
    using Contracts;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Models.Contracts;
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class MoviesContext : IdentityDbContext<Users>, IMoviesContext
    {
        public MoviesContext()
            :base("MoviesConnection")
        {

        }
        public IDbSet<Genres> Genres { get; set; }
        public IDbSet<Movies> Movies { get; set; }
        public IDbSet<Comments> Comments { get; set; }
        public IDbSet<Likes> Likes { get; set; }
        public IDbSet<Dislikes> Dislikes { get; set; }
        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
        }
        public static MoviesContext Create()
        {
            return new MoviesContext();
        }
        private void ApplyAuditInfoRules()
        {
            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IAuditable && ((e.State == EntityState.Added) || (e.State == EntityState.Modified))))
            {
                var entity = (IAuditable)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == null)
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }

            foreach (var entry in
                this.ChangeTracker.Entries()
                    .Where(
                        e =>
                        e.Entity is IDeletable && e.State == EntityState.Modified))
            {
                var entity = (IDeletable)entry.Entity;

                if (entity.DeletedOn != null && entity.IsDeleted == false)
                {
                    entity.DeletedOn = null;
                }
            }
        }
    }
}
