namespace TelerikMovies.Data.Contracts
{
    using Models.Contracts;
    using System.Linq;
    public interface IEfGenericRepository<T> where T : class,IDeletable
    {
        IQueryable<T> All();
        IQueryable<T> AllNotDeleted();

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(object id);

    }
}