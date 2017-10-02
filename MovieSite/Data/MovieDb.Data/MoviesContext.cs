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
        public IDbSet<Movies> Movies { get; set; }
        public IDbSet<Comments> Comments { get; set; }
        public IDbSet<Likes> Likes { get; set; }
        public IDbSet<Dislikes> Dislikes { get; set; }
        public override int SaveChanges()
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges();
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
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTime))
                {
                    entity.CreatedOn = DateTime.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTime.Now;
                }
            }
        }
        public static MoviesContext Create()
        {
            return new MoviesContext();
        }
    }
}
