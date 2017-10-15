using Common;
using Common.Contracts;
using Common.Enums;
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
            var currentUser = this.UserRepo.All().Where(x => x.UserName.ToLower() == userName.ToLower()).FirstOrDefault();
            return currentUser;
        }

        public IResult UpdateUser(Users user)
        {
            IResult result = new Result("Success", ResultType.Success);

            var currentUser = this.UserRepo.All().Where(x => x.UserName.ToLower() == user.UserName.ToLower()).FirstOrDefault();

            if (currentUser != null)
            {
                var areSame = user.CompareUserWith(currentUser);
                if (!areSame)
                {

                    currentUser.FirstName = user.FirstName;
                    currentUser.LastName = user.LastName;
                    currentUser.ImgUrl = user.ImgUrl;
                    currentUser.isMale = user.isMale;
                    currentUser.City = user.City;

                    this.SaveChange(() => { this.UserRepo.Update(currentUser); }, ref result);
                }
                else
                {
                    result.ErrorMsg = Constants.ErorsDict[ResultType.NoChanges];
                    result.ResulType = ResultType.NoChanges;
                }
            }
            else
            {
                result.ErrorMsg = Constants.ErorsDict[ResultType.DoesntExists];
                result.ResulType = ResultType.DoesntExists;
            }

            return result;
        }
    }
}
