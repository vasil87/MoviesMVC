using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelerikMovies.Data.Contracts;
using TelerikMovies.Models;
using TelerikMovies.Services.Contracts;

namespace TelerikMovies.Services
{
    public class UsersService: DataBaseService,IUsersService
    {
        public UsersService(IEfGenericRepository<Movies> movies, IEfGenericRepository<Genres> genresRepo,
             IEfGenericRepository<Comments> commentsRepo, IEfGenericRepository<Users> userRepo,
             IEfGenericRepository<Likes> likesRepo, IEfGenericRepository<Dislikes> dislikesRepo, IUoW saver) 
            :base( movies,genresRepo,commentsRepo,userRepo, likesRepo, dislikesRepo, saver)
        {
        }

        public Users GetByUserName(string userName)
        {
            var currentUser = this.UserRepo.All().Where(x => x.UserName == userName).FirstOrDefault();
            return currentUser;
        }
    }
}
