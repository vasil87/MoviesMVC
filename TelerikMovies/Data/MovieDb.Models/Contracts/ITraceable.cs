using System;

namespace TelerikMovies.Models.Contracts
{
    public interface ITraceable
    {
        Guid Id { get; set; }
    }
}
