namespace TelerikMovies.Data.Repositories
{
    using Contracts;
    using Models.Contracts;
    using System;
    using System.Data.Entity;
    using System.Linq;
    public class EfGenericRepository<T> : IEfGenericRepository<T> 
        where T : class , IDeletable
    {
        protected IDbSet<T> dbSet;
        protected readonly IMoviesContext context; 
        public EfGenericRepository(IMoviesContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.context = context;
            this.dbSet = this.context.Set<T>();
        }

     

        public virtual IQueryable<T> AllNotDeleted()
        {
            return this.dbSet.AsQueryable().Where(x => !x.IsDeleted);
        }

        public virtual IQueryable<T> All()
        {
            return this.dbSet.AsQueryable();
        }


        public virtual T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
            var entry = this.context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.dbSet.Add(entity);
            }
        }

        public virtual void Update(T entity)
        {
            var entry = this.context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.Now;

            var entry = this.context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }


    }
}