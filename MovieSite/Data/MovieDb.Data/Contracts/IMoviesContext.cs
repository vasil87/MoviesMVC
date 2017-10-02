using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using TelerikMovies.Models;

namespace TelerikMovies.Data.Contracts
{
    public interface IMoviesContext
    {
        IDbSet<Comments> Comments { get; set; }
        IDbSet<Dislikes> Dislikes { get; set; }
        IDbSet<Likes> Likes { get; set; }
        IDbSet<Movies> Movies { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        int SaveChanges();
    }
}