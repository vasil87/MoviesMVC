using TelerikMovies.Data.Contracts;

namespace TelerikMovies.Data.UoW
{
    public class UoW : IUoW
    {
        private readonly IMoviesContext context;

        public UoW(IMoviesContext context)
        {
            this.context = context;
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}
