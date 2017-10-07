using TelerikMovies.Models;

namespace TelerikMovies.Services.Contracts
{
    public interface IUsersService
    {
        Users GetByUserName(string userName);
    }
}
